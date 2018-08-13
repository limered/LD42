using System;
using SystemBase;
using Systems.People.States;
using Systems.Score;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Systems.People
{
    [GameSystem]
    public class PeopleSystem : GameSystem<PersonComponent>
    {
        public override void Register(PersonComponent comp)
        {
            var firstState = new Entering();
            comp.StateContext = new PersonStateContext(firstState, comp);
            firstState.Enter(comp.StateContext);
        }
    }
}