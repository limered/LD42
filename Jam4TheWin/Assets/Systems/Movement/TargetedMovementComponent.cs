using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Movement
{
    public class TargetedMovementComponent : GameComponent
    {
        [Range(0, 100)]
        public float Acceleration;
        [Range(0.0f, 0.5f)]
        public float MaxSpeed;
        [Range(0, 50)]
        public float MaxDistance;
        public GameObject Target;
        public bool IsMoving;

        public Vector3ReactiveProperty Direction = new Vector3ReactiveProperty();
        public FloatReactiveProperty Distance  = new FloatReactiveProperty();
    }
}