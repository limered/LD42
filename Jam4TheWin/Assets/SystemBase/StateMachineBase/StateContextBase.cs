using System.Linq;
using UniRx;

namespace SystemBase.StateMachineBase
{
    public class StateContextBase<TState> : IStateContext<TState> where TState : IState
    {
        public StateContextBase()
        {
            CurrentState = new ReactiveProperty<TState>();
        }

        public ReactiveProperty<TState> CurrentState { get; private set; }
        public bool GoToState(TState state)
        {
            if (CurrentState.Value.ValidNextStates.All(st => st != state.GetType())) return false;
            if (!CurrentState.HasValue) return false;

            CurrentState.Value.Exit();
            CurrentState.Value = state;
            return CurrentState.Value.Enter(this);
        }
    }
}