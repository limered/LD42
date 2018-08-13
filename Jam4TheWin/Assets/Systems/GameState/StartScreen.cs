using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using UniRx;

namespace Systems.GameState
{
    public class StartScreen : IGameState
    {
        private IDisposable _gameStartDisposable;
        public ReadOnlyCollection<Type> ValidNextStates { get{return new ReadOnlyCollection<Type>(new List<Type>{typeof(Running)});} }
        public bool Enter<TState>(IStateContext<TState> context) where TState : IState
        {
            _gameStartDisposable = MessageBroker.Default.Receive<GameMessageStart>()
                .Subscribe(start => ((BasicGameStateContext) context).GoToState(new Running()));
            return true;
        }

        public void Exit()
        {
            _gameStartDisposable.Dispose();
        }
    }
}