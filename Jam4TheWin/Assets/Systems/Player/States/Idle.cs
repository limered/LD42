using GameState.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using UniRx;

namespace Systems.Player.States
{
    public class Idle : ICatState
    {
        private IDisposable _startGameDisposable;

        public ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new List<Type> { typeof(Hungry) });
            }
        }

        public bool Enter<TState>(IStateContext<TState> context) where TState : IState
        {
            var ctx = (CatStateContext)context;
            _startGameDisposable = MessageBroker.Default.Receive<MessageStartGame>()
                .Subscribe(game => ctx.GoToState(new Hungry()));
            return true;
        }

        public void Exit()
        {
            _startGameDisposable.Dispose();
        }
    }
}