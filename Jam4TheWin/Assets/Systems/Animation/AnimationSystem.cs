using System;
using SystemBase;
using Systems.Movement;
using Systems.People;
using Systems.People.States;
using Systems.Player;
using Systems.Player.States;
using Systems.Sound;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.DotNet;
using Utils.Plugins;

namespace Systems.Animation
{
    [GameSystem(typeof(PlayerSystem), typeof(PeopleSystem))]
    public class AnimationSystem : GameSystem<AnimationComponent, CatComponent, PersonComponent, StrinkObejcsComponent>
    {
        private readonly ReactiveProperty<CatComponent> _cat = new ReactiveProperty<CatComponent>();
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

        public override void Register(PersonComponent component)
        {
            component.StateContext.CurrentState
                .Subscribe(state => PeopleStateChanged(state, component))
                .AddTo(component);
        }

        public override void Register(StrinkObejcsComponent component)
        {
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
        private void CatHit(CatGetsHitMessage m)
        {
            var anim = m.Cat.GetComponent<AnimationComponent>();
            var lastState = m.Cat.CatStateContext.CurrentState.Value;

            "cat_hiss".Play();

            anim.CharacterAnimator.Play("cat_angry");
            anim.BulbAnimator.Play("bulb_angry");

            Observable
                .Timer(TimeSpan.FromSeconds(m.Cat.AngryTime))
                .Subscribe(t => CatStateChanged(lastState, m.Cat.GetComponent<CatComponent>()));
        }

        private string _lastCatState;

        private void CatStateChanged(ICatState state, CatComponent component)
        {
            var anim = component.GetComponent<AnimationComponent>();
            var stink = component.GetComponent<StrinkObejcsComponent>();

            var newCatState = state.GetType().Name;
            
            Debug.Log("Cat state => "+newCatState);

            switch (newCatState)
            {
                case "Eating":
                    "cat_eat".Play();
                    anim.CharacterAnimator.Play("cat_eating");
                    anim.BulbAnimator.Play("bulb_eating");
                    _dispose = component.Hunger.Subscribe(f => anim.BulbAnimator.SetFloat("Progress", 1 - f / CatComponent.MaxHunger));
                    break;

                case "Full":
                    _dispose.Dispose();

                    if(newCatState != _lastCatState) new []{"cat_meow_1", "cat_meow_2", "cat_meow_3", "cat_meow_4"}.RandomElement().Play();

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

                    if(newCatState != _lastCatState) new []{"cat_meow_1", "cat_meow_2", "cat_meow_3", "cat_meow_4"}.RandomElement().Play();

                    if (component.GetComponent<MovementComponent>().CanMove.Value)
                    {
                        anim.CharacterAnimator.Play("cat_walking");
                    }
                    else
                    {
                        anim.CharacterAnimator.Play("cat_standing");
                    }
                    anim.BulbAnimator.Play("bulb_hungry");
                    stink.Flies.SetActive(false);
                    stink.Smoke.SetActive(false);
                    break;

                case "NeedsLove":
                    _dispose.Dispose();

                    // if(newCatState != _lastCatState) new []{"cat_meow_1", "cat_meow_2", "cat_meow_3", "cat_meow_4"}.RandomElement().Play();

                    if (component.GetComponent<MovementComponent>().CanMove.Value)
                    {
                        anim.CharacterAnimator.Play("cat_walking");
                    }
                    else
                    {
                        anim.CharacterAnimator.Play("cat_standing");
                    }

                    _dispose = component.UpdateAsObservable().Subscribe(f => anim.BulbAnimator.SetFloat("Progress", 1 - (component.InLoveStarted + CatComponent.MaxInLoveTime - Time.realtimeSinceStartup) / CatComponent.MaxInLoveTime));
                    anim.BulbAnimator.Play("bulb_isLoving");
                    stink.Flies.SetActive(true);
                    stink.Smoke.SetActive(true);
                    break;

                case "Loving":
                    anim.CharacterAnimator.Play("cat_petted");

                    "cat_purr".Play();

                    break;

                case "Pooping":
                    anim.CharacterAnimator.Play("cat_pooping");
                    anim.BulbAnimator.Play("bulb_pooping");
                    _dispose = component.PoopingTimer.Subscribe(f => anim.BulbAnimator.SetFloat("Progress", 1 - f / CatComponent.MaxPoopingTime));
                    "cat_poop".Play();
                    break;
            }

            _lastCatState = state.GetType().Name;
        }

        private void PeopleStateChanged(PersonState state, PersonComponent component)
        {
            var anim = component.GetComponent<AnimationComponent>();

            Debug.Log("Person ("+Math.Abs(component.GetHashCode())+") state => "+state.GetType().Name);

            switch (state.GetType().Name)
            {
                case "Angry":
                    "people_yuck".Play();

                    anim.CharacterAnimator.Play("person_angry");
                    anim.BulbAnimator.Play("bulb_stinky");
                    break;

                case "Entering":
                case "Idle":
                    anim.CharacterAnimator.Play("person_standing");
                    anim.BulbAnimator.Play("bulb_music");
                    break;

                case "Happy":
                    "people_yeah".Play();
                    anim.CharacterAnimator.Play("person_standing");
                    anim.BulbAnimator.Play("bulb_hasLove");
                    break;

                case "Loving":
                    "people_aww".Play();
                    anim.CharacterAnimator.Play("person_petting");
                    anim.BulbAnimator.Play("bulb_hasLove");
                    break;

                case "RunningToCat":
                    "people_aww".Play();
                    anim.CharacterAnimator.Play("person_standing");
                    anim.BulbAnimator.Play("bulb_needLove");
                    break;
            }
        }
    }
}