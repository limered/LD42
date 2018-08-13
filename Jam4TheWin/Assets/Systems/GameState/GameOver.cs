using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using UniRx;

namespace Systems.GameState
{
    public class GameOver : IGameState
    {
        private IDisposable _restartDispasable;
        public ReadOnlyCollection<Type> ValidNextStates { get { return new ReadOnlyCollection<Type>(new List<Type> { typeof(StartScreen) }); } }

        public bool Enter<TState>(IStateContext<TState> context) where TState : IState
        {
            _restartDispasable = MessageBroker.Default.Receive<GameMessageRestart>()
                .Subscribe(m => ((BasicGameStateContext)context).GoToState(new StartScreen()));
            return true;
        }

        public void Exit()
        {
            _restartDispasable.Dispose();
        }
    }
}