using SystemBase;
using UniRx;
using UnityEngine;
using Utils.Math;
using Utils.Unity;

namespace Systems.Movement
{
    public class MovementAccuracyMutator : MovementMutator
    {
        [Tooltip("accuracy with which this gameobject is movig towards target\n(0 = complete random / 1 = 100% accurate)")]
        [Range(0f, 1f)]
        public float Accuracy = 1f;

        public override void Mutate(Vector3 oldDirection, float oldSpeed, out Vector3 newDirection, out float newSpeed)
        {
            var baseDirection = oldDirection;
            var range = 360 - (Accuracy * 360);
            var dir2D = UnityEngine.Random.value > 0.5f
                 ? VectorUtils.Rotate(new Vector2(baseDirection.x, baseDirection.z), UnityEngine.Random.value * range / 2f)
                 : VectorUtils.Rotate(new Vector2(baseDirection.x, baseDirection.z), UnityEngine.Random.value * (-range) / 2f);
            var direction = new Vector3(dir2D.x, 0, dir2D.y);
            
            newDirection = direction;
            newSpeed = oldSpeed;
        }
    }
}