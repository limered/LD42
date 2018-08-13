using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using UniRx;

namespace Systems.GameState
{
    public class Paused : IGameState
    {
        private IDisposable _unpauseMessageDisposable;
        public ReadOnlyCollection<Type> ValidNextStates { get{return new ReadOnlyCollection<Type>(new List<Type>{typeof(Running)});} }
        public bool Enter<TState>(IStateContext<TState> context) where TState : IState
        {
            _unpauseMessageDisposable = MessageBroker.Default.Receive<GameMessageUnPause>()
                .Subscribe(m => ((BasicGameStateContext)context).GoToState(new Running()));

            return true;
        }

        public void Exit()
        {
            _unpauseMessageDisposable.Dispose();
        }
    }
}