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
    public class MovementSystem : GameSystem<MovementComponent>
    {
        public override void Register(MovementComponent comp)
        {
            comp.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if (!comp.CanMove) return;

                var direction = comp.Direction.Value;
                var speed = comp.Speed.Value;

                foreach (var mutator in comp.MovementMutators)
                {
                    mutator.Mutate(direction, speed, out direction, out speed);
                }

                comp.transform.position += direction * speed;
            })
            .AddTo(comp);
        }
    }
}