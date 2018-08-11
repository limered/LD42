using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemBase;
using Systems.Movement;
using Systems.Room;

namespace Systems.People
{
    [RequireComponent(typeof(TargetedMovementComponent))]
    [RequireComponent(typeof(KeepInsideRoomComponent))]
    public class PersonComponent : GameComponent
    {
        public float Speed = 1;
    }
}
