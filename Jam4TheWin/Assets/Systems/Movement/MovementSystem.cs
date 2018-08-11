using System;
using SystemBase;
using Systems.Room;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Math;

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

                if (insideRoomComp == null || insideRoomComp.CanMoveInDirection(comp.Direction.Value * velocity + (comp.Direction.Value.normalized * insideRoomComp.obstaclePaddig)))
                    comp.transform.position += comp.Direction.Value * velocity;
            };
        }
    }
}