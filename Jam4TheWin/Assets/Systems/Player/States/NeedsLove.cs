using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using Systems.People;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Player.States
{
    public class NeedsLove : ICatState
    {
        private IDisposable _lovingStateDisposable;

        public ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new List<Type>{typeof(Loving), typeof(Hungry)});
            }
        }

        public bool Enter<TState>(IStateContext<TState> context) where TState : IState
        {
            var ctx = (CatStateContext) context;
            _lovingStateDisposable = ctx.Cat.OnTriggerEnterAsObservable()
                .Where(coll=>coll.GetComponent<PersonComponent>())
                .Subscribe(CatStartsMakingLove(ctx));
            return true;
        }

        private Action<Collider> CatStartsMakingLove(CatStateContext ctx)
        {
            return coll => { ctx.GoToState(new Loving()); };
        }

        public void Exit()
        {
            _lovingStateDisposable.Dispose();
        }
    }
}