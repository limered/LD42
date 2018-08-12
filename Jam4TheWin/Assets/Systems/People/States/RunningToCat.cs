﻿using System;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;

namespace Systems.People.States
{
    public class RunningToCat : IPersonState
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