using SystemBase;
using Systems.Movement;
using Systems.Room;
using UnityEngine;

namespace Systems.People
{
    [RequireComponent(typeof(KeepInsideRoomComponent))]
    [RequireComponent(typeof(TargetMutator))]
    public class PersonComponent : GameComponent
    {
        public PersonStateContext StateContext;
        public float TimeUntilHappy = 3f; //seconds
    }
}