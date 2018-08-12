using System;
using UnityEngine;
using Utils.Math;

namespace Systems.Movement
{
    public class RunAwayMutator : MovementMutator
    {
        public float Acceleration;
        public float MaxSpeed;
        public float MaxDistance;
        public GameObject Source;

        public override void Mutate(bool canMove, Vector3 oldDirection, float oldSpeed, out Vector3 newDirection, out float newSpeed)
        {
            if (Source)
            {
                var direction = Source.transform.position.DirectionTo(transform.position).XZ();
                var distance = Source.transform.position.DistanceTo(transform.position);

                var speed = (MaxDistance / distance) * Acceleration * Time.deltaTime;
                speed = Mathf.Min(speed, MaxSpeed);

                var angle = VectorUtils.Angle(direction, oldDirection.XZ());

                if (Math.Abs(oldDirection.sqrMagnitude) < float.Epsilon)
                {
                    newDirection = new Vector3(direction.x, 0, direction.y);
                    newSpeed = speed;
                }
                else
                {
                    var newDir = oldDirection.XZ().Rotate(angle);
                    newDirection = new Vector3(newDir.x, 0, newDir.y);
                    newSpeed = oldSpeed + speed;
                }
            }
            else
            {
                base.Mutate(canMove, oldDirection, oldSpeed, out newDirection, out newSpeed);
            }
        }
    }
}
