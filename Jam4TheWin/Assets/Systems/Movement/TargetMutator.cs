using SystemBase;
using UniRx;
using UnityEngine;
using Utils.Math;

namespace Systems.Movement
{
    public class TargetMutator : MovementMutator
    {
        [Range(0, 100)]
        public float Acceleration;
        [Range(0.0f, 0.5f)]
        public float MaxSpeed;
        [Range(0, 50)]
        public float MaxDistance;
        public GameObject Target;

        public override void Mutate(Vector3 oldDirection, float oldSpeed, out Vector3 newDirection, out float newSpeed)
        {
            if (Target)
            {
                var targetDirection = transform.position.DirectionTo(Target.transform.position);
                var speed = Acceleration * (transform.position.DistanceTo(Target.transform.position) / MaxDistance) * Time.deltaTime;
                speed = Mathf.Min(speed, MaxSpeed);

                newDirection = targetDirection;
                newSpeed = speed;
            }
            else 
            {
                base.Mutate(oldDirection, oldSpeed, out newDirection, out newSpeed);
            }
        }
    }
}