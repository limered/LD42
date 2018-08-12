using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using Systems.Movement;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Player.States
{
    public class Hungry : ICatState
    {
        private IDisposable _catEatingDisposable;

        public ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new List<Type> {typeof(Eating)});
            }
        }

        public bool Enter<TState>(IStateContext<TState> context) where TState : IState
        {
            var ctx = (CatStateContext)context;
            ctx.Cat.Hunger.Value = CatComponent.MaxHunger;

            _catEatingDisposable = ctx.Cat.OnTriggerEnterAsObservable()
                .Subscribe(CatStartsEating(ctx));

            return true;
        }

        private Action<Collider> CatStartsEating(CatStateContext context)
        {
            return coll => context.GoToState(new Eating());
        }

        public void Exit()
        {
            _catEatingDisposable.Dispose();
        }
    }
}
