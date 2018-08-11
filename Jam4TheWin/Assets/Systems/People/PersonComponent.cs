using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemBase;
using Systems.Movement;

namespace Systems.People
{
    [RequireComponent(typeof(TargetedMovementComponent))]
    public class PersonComponent : GameComponent
    {
        public float Speed = 1;

    }
}
