﻿using SystemBase.StateMachineBase;
using Systems.People.States;
using UniRx;

namespace Systems.People
{
    public class PersonStateContext : StateContextBase<PersonState>
    {
        public PersonComponent Person { get; private set; }
        public IntReactiveProperty LovingCount = new IntReactiveProperty(0);
        public PersonStateContext(PersonState initialState, PersonComponent p) : base(initialState)
        {
            Person = p;
        }
    }
}
