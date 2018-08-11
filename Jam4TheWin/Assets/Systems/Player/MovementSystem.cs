using SystemBase;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Math;

namespace Systems.Player
{
    [GameSystem]
    public class MovementSystem : GameSystem<TargetedMovementComponent>
    {
        public override void Register(TargetedMovementComponent component)
        {
            component.UpdateAsObservable()
                .Select(_ => component)
                .Subscribe(MoveTotarget)
                .AddTo(component);
        }

        private static void MoveTotarget(TargetedMovementComponent comp)
        {
            comp.Direction.Value = comp.transform.position.DirectionTo(comp.Target.transform.position);
            comp.Distance.Value = comp.transform.position.DistanceTo(comp.Target.transform.position);

            if (!comp.IsMoving) return;

            var velocity = comp.Acceleration * (comp.Distance.Value / comp.MaxDistance) * Time.deltaTime;
            velocity = Mathf.Min(velocity, comp.MaxSpeed);

            comp.transform.position += comp.Direction.Value * velocity;
        }
    }
}