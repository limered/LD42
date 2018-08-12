using System;
using SystemBase;
using Systems.Room;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Math;
using UnityEngine;

namespace Systems.Movement
{
    [GameSystem]
    public class MovementSystem : GameSystem<TargetedMovementComponent>
    {
        public override void Register(TargetedMovementComponent component)
        {
            component.UpdateAsObservable()
                .Select(_ => component)
                .Subscribe(MoveTotarget(component.GetComponent<KeepInsideRoomComponent>()))
                .AddTo(component);
        }

        private static Action<TargetedMovementComponent> MoveTotarget(KeepInsideRoomComponent insideRoomComp)
        {
            return (TargetedMovementComponent comp) =>
            {
                if (comp.Target)
                {
                    comp.Direction.Value = comp.transform.position.DirectionTo(comp.Target.transform.position);
                    comp.Distance.Value = comp.transform.position.DistanceTo(comp.Target.transform.position);
                }

                if (!comp.IsMoving) return;

                var velocity = comp.Acceleration * (comp.Distance.Value / comp.MaxDistance) * Time.deltaTime;
                velocity = Mathf.Min(velocity, comp.MaxSpeed);

                var range = 360 - (comp.Accuracy * 360);
                var dir2D = UnityEngine.Random.value > 0.5f
                     ? VectorUtils.Rotate(new Vector2(comp.Direction.Value.x, comp.Direction.Value.z), UnityEngine.Random.value * range / 2f)
                     : VectorUtils.Rotate(new Vector2(comp.Direction.Value.x, comp.Direction.Value.z), UnityEngine.Random.value * (-range) / 2f);
                var direction = new Vector3(dir2D.x, 0, dir2D.y);

                if (insideRoomComp == null || insideRoomComp.CanMoveInDirection(direction * velocity + (direction.normalized * insideRoomComp.obstaclePaddig)))
                {
                    Debug.DrawRay(comp.transform.position, direction * velocity * 5, Color.green);
                    comp.transform.position += direction * velocity;
                }
                else
                {
                    Debug.DrawRay(comp.transform.position, direction * velocity * 5, Color.red);
                }
            };
        }
    }
}