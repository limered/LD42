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

        [Tooltip("accuracy with which this gameobject is movig towards target\n(0 = complete random / 1 = 100% accurate)")]
        [Range(0f,1f)]
        public float Accuracy = 1f;

        public Vector3ReactiveProperty Direction = new Vector3ReactiveProperty();
        public FloatReactiveProperty Distance  = new FloatReactiveProperty();
    }
}