using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Player.States
{
    public class Eating : ICatState
    {
        private IDisposable _catUpdateDisposable;
        private IDisposable _catTriggerExitDisposable;

        public ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new List<Type> {typeof(Full), typeof(Hungry)});
            }
        }

        public bool Enter<TState>(IStateContext<TState> context) where TState : IState
        {
            var ctx = (CatStateContext) context;
            _catTriggerExitDisposable = ctx.Cat.OnTriggerExitAsObservable()
                .Subscribe(CatStopsEating(ctx));

            _catUpdateDisposable = ctx.Cat.OnTriggerStayAsObservable()
                .Subscribe(CatFeeding(ctx));

            return true;
        }

        private Action<Collider> CatFeeding(CatStateContext ctx)
        {
            return coll =>
            {
                ctx.Cat.Hunger.Value -= ctx.Cat.HungerReduceFactor * Time.deltaTime;
                if (ctx.Cat.Hunger.Value < 0)
                {
                    ctx.GoToState(new Full());
                }
            };
        }

        private Action<Collider> CatStopsEating(CatStateContext context)
        {
            return coll => context.GoToState(new Hungry());
        }

        public void Exit()
        {
            _catUpdateDisposable.Dispose();
            _catTriggerExitDisposable.Dispose();
        }
    }
}