using System;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;

namespace Systems.Player.States
{
    public class Full : ICatState
    {
        public ReadOnlyCollection<Type> ValidNextStates { get; private set; }
        public bool Enter<TState>(IStateContext<TState> context) where TState : IState
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }
    }
}