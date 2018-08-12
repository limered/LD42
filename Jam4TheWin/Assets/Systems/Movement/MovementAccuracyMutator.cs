using UnityEngine;
using Utils.Math;

namespace Systems.Movement
{
    public class MovementAccuracyMutator : MovementMutator
    {
        [Tooltip("accuracy with which this gameobject is movig towards target\n(0 = complete random / 1 = 100% accurate)")]
        [Range(0f, 1f)]
        public float Accuracy = 1f;
        public float Interval = 0f;

        private float _lastRotatedBy;
        private float _lastTimeRotationChanged;

        public override void Mutate(Vector3 oldDirection, float oldSpeed, out Vector3 newDirection, out float newSpeed)
        {
            var baseDirection = oldDirection;
            var range = 360 - (Accuracy * 360);

            if (Time.fixedTime - _lastTimeRotationChanged > Interval)
            {
                _lastRotatedBy = UnityEngine.Random.value > 0.5f ? UnityEngine.Random.value * range / 2f : UnityEngine.Random.value * (-range) / 2f;
                _lastTimeRotationChanged = Time.fixedTime;
            }

            var dir2D = VectorUtils.Rotate(new Vector2(baseDirection.x, baseDirection.z), _lastRotatedBy);
            var direction = new Vector3(dir2D.x, 0, dir2D.y);

            newDirection = direction;
            newSpeed = oldSpeed;
        }
    }
}