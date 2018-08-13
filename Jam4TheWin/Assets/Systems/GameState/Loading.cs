using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using UniRx;

namespace Systems.GameState
{
    public class Loading : IGameState
    {
        private IDisposable _loadingDoneDisposable;

        public ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new List<Type>{typeof(StartScreen)});
            }
        }

        public bool Enter<TState>(IStateContext<TState> context) where TState : IState
        {
            _loadingDoneDisposable = MessageBroker.Default.Receive<GameMessageLoadingDone>()
                .Subscribe(LoadingDone(context as BasicGameStateContext));

            return true;
        }

        private Action<GameMessageLoadingDone> LoadingDone(BasicGameStateContext ctx)
        {
            return m => ctx.GoToState(new StartScreen());
        }

        public void Exit()
        {
            _loadingDoneDisposable.Dispose();
        }
    }
}