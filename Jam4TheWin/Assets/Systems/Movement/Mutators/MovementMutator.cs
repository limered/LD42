using System;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Movement
{
    [RequireComponent(typeof(MovementComponent))]
    public abstract class MovementMutator : GameComponent
    {
        public virtual void Mutate(bool canMove, Vector3 oldDirection, float oldSpeed, out Vector3 newDirection, out float newSpeed)
        {
            newDirection = oldDirection;
            newSpeed = oldSpeed;
        }
    }
}