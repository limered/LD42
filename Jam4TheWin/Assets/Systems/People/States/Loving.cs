using System;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;

namespace Systems.People.States
{
    public class Loving : PersonState
    {
        public override ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return null;
            }
        }
        public override bool Enter<TState>(IStateContext<TState> context) 
        {
            return false;
        }
    }
}