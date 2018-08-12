using System;
using SystemBase;
using Systems.Player;
using Systems.Player.States;
using UniRx;

namespace Systems.Animation
{
    [GameSystem(typeof(PlayerSystem))]
    public class AnimationSystem : GameSystem<AnimationComponent, CatComponent>
    {
        private CatStateContext _catStateContext;
        private IDisposable dispose;

        public override void Register(AnimationComponent component)
        {
        }

        public override void Register(CatComponent component)
        {
            component.CatStateContext.CurrentState
                .Subscribe(state=> 
                {
                    CatStateChanged(state, component);
                })
                .AddTo(component);

            MessageBroker.Default.Receive<CatGetsHitMessage>()
                .Subscribe(CatHit)
                .AddTo(component);
        }

        private void CatStateChanged(ICatState state, CatComponent component)
        {
            var anim = component.GetComponent<AnimationComponent>();
            
            switch (state.GetType().Name.ToString())
            {
                case "Eating":
                    anim.CatAnimator.Play("cat_eating");
                    anim.BulbAnimator.Play("bulb_eating");
                    dispose = component.Hunger.Subscribe(f => anim.BulbAnimator.SetFloat("Progress", 1 - f / CatComponent.MaxHunger));
                    break;
                case "Full":
                    dispose.Dispose();
                    anim.CatAnimator.Play("cat_walking");
                    anim.BulbAnimator.Play("bulb_full");
                    break;
                case "Hungry":
                    if(dispose != null)
                        dispose.Dispose();
                    anim.CatAnimator.Play("cat_walking");
                    anim.BulbAnimator.Play("bulb_hungry");
                    break;
                case "NeedsLove":
                    dispose.Dispose();
                    anim.CatAnimator.Play("cat_walking");
                    anim.BulbAnimator.Play("bulb_needLove");
                    break;
                case "Loving":
                    anim.CatAnimator.Play("cat_petted");
                    anim.BulbAnimator.Play("bulb_needLove");
                    break;
                case "Pooping":
                    anim.CatAnimator.Play("cat_pooping");
                    anim.BulbAnimator.Play("bulb_pooping");
                    dispose = component.PoopingTimer.Subscribe(f => anim.BulbAnimator.SetFloat("Progress", 1 - f / CatComponent.MaxPoopingTime));
                    break;
            }
        }

        private void CatHit(CatGetsHitMessage m)
        {
            
            var anim = m.Cat.GetComponent<AnimationComponent>();
            var lastState = m.Cat.CatStateContext.CurrentState.Value;
            
            anim.CatAnimator.Play("cat_angry");
            anim.BulbAnimator.Play("bulb_angry");

            Observable.Timer(TimeSpan.FromMilliseconds(800))
                .Subscribe(t =>
                {
                    CatStateChanged(lastState, m.Cat.GetComponent<CatComponent>());
                });
        }
    }
}