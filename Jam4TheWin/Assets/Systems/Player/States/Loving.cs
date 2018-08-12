using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Player.States
{
    public class Loving : ICatState
    {
        private IDisposable _loveStayDisposable;
        private IDisposable _lovingStopsDisposable;

        public ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new List<Type> { typeof(NeedsLove) });
            }
        }

        public bool Enter<TState>(IStateContext<TState> context) where TState : IState
        {
            var ctx = (CatStateContext)context;
            ctx.Cat.LoveTimer.Value = CatComponent.MaxLoveTimer;
            _loveStayDisposable = ctx.Cat.OnTriggerStayAsObservable()
                .Subscribe(CatIsLoving(ctx));

            _lovingStopsDisposable = ctx.Cat.OnTriggerExitAsObservable()
                .Subscribe(CatStopsLoving(ctx));

            return true;
        }

        private Action<Collider> CatStopsLoving(CatStateContext ctx)
        {
            return coll => { ctx.GoToState(new NeedsLove()); };
        }

        private Action<Collider> CatIsLoving(CatStateContext ctx)
        {
            return coll =>
            {
                ctx.Cat.LoveTimer.Value -= Time.deltaTime;
                if (ctx.Cat.LoveTimer.Value < 0)
                {
                    ctx.GoToState(new NeedsLove());
                }
            };
        }

        public void Exit()
        {
            _loveStayDisposable.Dispose();
        }
    }
}