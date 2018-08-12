using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using Systems.Interactables;
using UniRx;
using UniRx.Triggers;

namespace Systems.Player.States
{
    public class Full : ICatState
    {
        private IDisposable _poopBoxDisposable;

        public ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new List<Type> { typeof(Pooping) });
            }
        }

        public bool Enter<TState>(IStateContext<TState> context) where TState : IState
        {
            var ctx = (CatStateContext) context;
            ctx.Cat.Hunger.Value = CatComponent.MaxHunger;

            _poopBoxDisposable = ctx.Cat.OnTriggerEnterAsObservable()
                .Where(coll=>coll.GetComponent<LooComponent>())
                .Subscribe(CatStartsPooping(ctx));

            return true;
        }

        private Action<UnityEngine.Collider> CatStartsPooping(CatStateContext context)
        {
            return coll => context.GoToState(new Pooping());
        }

        public void Exit()
        {
            _poopBoxDisposable.Dispose();
        }
    }
}