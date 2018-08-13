using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using UniRx;

namespace Systems.GameState
{
    public class Running : IGameState
    {
        private IDisposable _pauseMessageDisposable;
        private IDisposable _endMessageDisposable;

        public ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new List<Type>
                {
                    typeof(Paused),
                    typeof(GameOver)
                });
            }
        }

        public bool Enter<TState>(IStateContext<TState> context) where TState : IState
        {
            _pauseMessageDisposable = MessageBroker.Default.Receive<GameMessagePause>()
                .Subscribe(m => ((BasicGameStateContext)context).GoToState(new Paused()));

            _endMessageDisposable = MessageBroker.Default.Receive<GameMessageEnd>()
                .Subscribe(m => ((BasicGameStateContext)context).GoToState(new GameOver()));

            return true;
        }

        public void Exit()
        {
            _endMessageDisposable.Dispose();
            _pauseMessageDisposable.Dispose();
        }
    }
}