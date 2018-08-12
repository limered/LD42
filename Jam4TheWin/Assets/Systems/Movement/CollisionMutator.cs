using System;
using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Movement
{
    public class CollisionMutator : MovementMutator
    {
        public LayerMask CollidesWith = new LayerMask { value = 0x7FFFFFFF };
        public float ObstaclePaddig;
        public Func<Vector3, float, bool> CanMoveInDirection { get; set; }

        public override void Mutate(Vector3 oldDirection, float oldSpeed, out Vector3 newDirection, out float newSpeed)
        {
            if (CanMoveInDirection != null)
            {
                if (CanMoveInDirection(oldDirection, oldSpeed+ObstaclePaddig))
                {
                    newDirection = oldDirection;
                    newSpeed = oldSpeed;
                }
                else
                {
                    newDirection = oldDirection;
                    newSpeed = 0f;
                }
            }
            else //if nothing is defined for 'CanMoveInDirection' dont alter anything
            {
                base.Mutate(oldDirection, oldSpeed, out newDirection, out newSpeed);

            }
        }
    }
}