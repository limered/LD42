using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using Systems.People;
using Systems.Sound;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.DotNet;

namespace Systems.Player.States
{
    public class NeedsLove : ICatState
    {
        private IDisposable _lovingStateDisposable;
        private IDisposable _needsLoveStopTimer;

        public ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new List<Type> { typeof(Loving), typeof(Hungry) });
            }
        }

        public bool Enter<TState>(IStateContext<TState> context) where TState : IState
        {
            var ctx = (CatStateContext)context;

            ctx.Cat.StinkCollider.SetActive(true);
            ctx.Cat.InnerSpaceCollider.SetActive(false);

            _lovingStateDisposable = ctx.Cat.OnTriggerEnterAsObservable()
                .Where(coll => coll.GetComponent<PersonComponent>())
                .Subscribe(CatStartsMakingLove(ctx));

            _needsLoveStopTimer = ctx.Cat.UpdateAsObservable()
                .Subscribe(CheckInLoveTimer(ctx));

            return true;
        }

        private static Action<Unit> CheckInLoveTimer(CatStateContext ctx)
        {
            return unit =>
            {
                if (ctx.Cat.InLoveStarted + CatComponent.MaxInLoveTime < Time.realtimeSinceStartup)
                {
                    ctx.GoToState(new Hungry());
                }
            };
        }

        private Action<Collider> CatStartsMakingLove(CatStateContext ctx)
        {
            return coll => { ctx.GoToState(new Loving()); };
        }

        public void Exit()
        {
            _lovingStateDisposable.Dispose();
            _needsLoveStopTimer.Dispose();
        }
    }
}