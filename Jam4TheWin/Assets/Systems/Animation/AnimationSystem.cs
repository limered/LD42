using System;
using SystemBase;
using Systems.Movement;
using Systems.People;
using Systems.People.States;
using Systems.Player;
using Systems.Player.States;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Animation
{
    [GameSystem(typeof(PlayerSystem), typeof(PeopleSystem))]
    public class AnimationSystem : GameSystem<AnimationComponent, CatComponent, PersonComponent>
    {
        private IDisposable _dispose;

        public override void Register(AnimationComponent component)
        {
        }

        public override void Register(CatComponent component)
        {
            component.CatStateContext.CurrentState
                .Subscribe(state => CatStateChanged(state, component))
                .AddTo(component);

            MessageBroker.Default
                .Receive<CatGetsHitMessage>()
                .Subscribe(CatHit)
                .AddTo(component);

            component.GetComponent<MovementComponent>()
                .CanMove
                .Subscribe(CanMoveChanged(component))
                .AddTo(component);
        }

        private Action<bool> CanMoveChanged(CatComponent cat)
        {
            return b =>
            {
                if (b)
                {
                    cat.GetComponent<AnimationComponent>()
                        .CharacterAnimator.Play("cat_walking");
                }
                else
                {
                    CatStateChanged(cat.CatStateContext.CurrentState.Value, cat);
                }
            };
        }

        public override void Register(PersonComponent component)
        {
            component.StateContext.CurrentState
                .Subscribe(state => PeopleStateChanged(state, component))
                .AddTo(component);
        }

        private void CatStateChanged(ICatState state, CatComponent component)
        {
            var anim = component.GetComponent<AnimationComponent>();

            switch (state.GetType().Name)
            {
                case "Eating":
                    anim.CharacterAnimator.Play("cat_eating");
                    anim.BulbAnimator.Play("bulb_eating");
                    _dispose = component.Hunger.Subscribe(f => anim.BulbAnimator.SetFloat("Progress", 1 - f / CatComponent.MaxHunger));
                    break;

                case "Full":
                    _dispose.Dispose();
                    if (component.GetComponent<MovementComponent>().CanMove.Value)
                    {
                        anim.CharacterAnimator.Play("cat_walking");
                    }
                    else
                    {
                        anim.CharacterAnimator.Play("cat_standing");
                    }
                    anim.BulbAnimator.Play("bulb_full");
                    break;

                case "Hungry":
                    if (_dispose != null) { _dispose.Dispose(); }
                    if (component.GetComponent<MovementComponent>().CanMove.Value)
                    {
                        anim.CharacterAnimator.Play("cat_walking");
                    }
                    else
                    {
                        anim.CharacterAnimator.Play("cat_standing");
                    }
                    anim.BulbAnimator.Play("bulb_hungry");
                    break;

                case "NeedsLove":
                    _dispose.Dispose();
                    if (component.GetComponent<MovementComponent>().CanMove.Value)
                    {
                        anim.CharacterAnimator.Play("cat_walking");
                    }
                    else {
                        anim.CharacterAnimator.Play("cat_standing");
                    }
                    
                    _dispose = component.UpdateAsObservable().Subscribe(f => anim.BulbAnimator.SetFloat("Progress", 1 - (component.InLoveStarted + CatComponent.MaxInLoveTime - Time.realtimeSinceStartup) / CatComponent.MaxInLoveTime));
                    anim.BulbAnimator.Play("bulb_isLoving");
                    break;

                case "Loving":
                    anim.CharacterAnimator.Play("cat_petted");
                    break;

                case "Pooping":
                    anim.CharacterAnimator.Play("cat_pooping");
                    anim.BulbAnimator.Play("bulb_pooping");
                    _dispose = component.PoopingTimer.Subscribe(f => anim.BulbAnimator.SetFloat("Progress", 1 - f / CatComponent.MaxPoopingTime));
                    break;
            }
        }

        private void PeopleStateChanged(PersonState state, PersonComponent component)
        {
            var anim = component.GetComponent<AnimationComponent>();
            switch (state.GetType().Name)
            {
                case "Angry":
                    anim.CharacterAnimator.Play("person1_angry");
                    anim.BulbAnimator.Play("bulb_stinky");
                    break;

                case "Entering":
                case "Idle":
                    anim.CharacterAnimator.Play("person1_standing");
                    anim.BulbAnimator.Play("bulb_music");
                    break;

                case "Happy":
                    anim.CharacterAnimator.Play("person1_standing");
                    anim.BulbAnimator.Play("bulb_hasLove");
                    break;

                case "Loving":
                    anim.CharacterAnimator.Play("person1_standing");
                    anim.BulbAnimator.Play("bulb_hasLove");
                    break;

                case "RunningToCat":
                    anim.CharacterAnimator.Play("person1_standing");
                    anim.BulbAnimator.Play("bulb_needLove");
                    break;
            }
        }

        private void CatHit(CatGetsHitMessage m)
        {
            var anim = m.Cat.GetComponent<AnimationComponent>();
            var lastState = m.Cat.CatStateContext.CurrentState.Value;

            anim.CharacterAnimator.Play("cat_angry");
            anim.BulbAnimator.Play("bulb_angry");

            Observable
                .Timer(TimeSpan.FromSeconds(m.Cat.AngryTime))
                .Subscribe(t => CatStateChanged(lastState, m.Cat.GetComponent<CatComponent>()));
        }
    }
}