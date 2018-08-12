using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemBase;
using System;
using Systems.Movement;

namespace Systems.Room
{
    [RequireComponent(typeof(CollisionMutator))]
    public class KeepInsideRoomComponent : SemanticGameComponent<CollisionMutator>
    {
        [Range(-10, 10)]
        public float CustomOffsetToGround = 0f;

        void Reset()
        {
            Dependency = GetComponent<CollisionMutator>();
        }
    }
}
