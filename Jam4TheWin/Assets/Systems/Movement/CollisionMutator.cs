using System;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Movement
{
    public class CollisionMutator : MovementMutator
    {
        public float ObstaclePaddig;
        public Func<Vector3, bool> CanMoveInDirection { get; set; }

        public override void Mutate(Vector3 oldDirection, float oldSpeed, out Vector3 newDirection, out float newSpeed)
        {
            if (CanMoveInDirection != null && CanMoveInDirection(oldDirection * oldSpeed + (oldDirection.normalized * ObstaclePaddig)))
            {
                newDirection = oldDirection;
                newSpeed = oldSpeed;
                Debug.DrawRay(transform.position, newDirection * newSpeed * 5, Color.green);
            }
            else //if nothing is defined for 'CanMoveInDirection' dont alter anything
            {
                base.Mutate(oldDirection, oldSpeed, out newDirection, out newSpeed);
                Debug.DrawRay(transform.position, newDirection * newSpeed * 5, Color.red);
            }
        }
    }
}