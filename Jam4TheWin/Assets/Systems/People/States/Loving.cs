using System;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using Systems.Player;
using UniRx;
using UniRx.Triggers;

namespace Systems.People.States
{
    public class Loving : PersonState
    {
        public override ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new Type[] { typeof(Angry), typeof(Happy) });
            }
        }
        public override bool Enter<TState>(IStateContext<TState> context)
        {
            var ctx = (PersonStateContext)context;

            //TODO: stop person

            return true;
        }
    }
}