using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UniRx.Triggers;
using UniRx;
using UnityEngine;
using Utils.Plugins;

namespace Systems.Room
{
    [GameSystem]
    public class RoomSystem : GameSystem<FloorComponent, WallComponent, KeepInsideRoomComponent>
    {
        private FloorComponent floor;
        public FloorComponent Floor { get { return floor; } }
        private List<WallComponent> walls = new List<WallComponent>();

        public override void Register(FloorComponent comp)
        {
            floor = comp;
        }

        public override void Register(WallComponent comp)
        {
            if (!walls.Contains(comp))
            {
                walls.Add(comp);
            }
        }

        public override void Register(KeepInsideRoomComponent comp)
        {
            //stay on the floor
            comp.UpdateAsObservable()
            .SelectWhereNotNull(_ => floor)
            .Subscribe(floor =>
            {
                var pos = comp.transform.position;

                if (comp.customOffsetToGround == 0f) pos.y = comp.GetComponent<Renderer>().bounds.extents.y / 2f;
                else pos.y = comp.customOffsetToGround;

                comp.transform.position = pos;
            })
            .AddTo(comp);

            //don't go through walls
            // comp.UpdateAsObservable()
            // .SelectMany(_ => walls)
            // .Subscribe(wall =>
            // {
            //     var personCollider = comp.GetComponent<Collider>();
            //     var wallCollider = wall.GetComponent<Collider>();
            //     if (personCollider.bounds.Intersects(wallCollider.bounds))
            //     {
            //         comp.transform.position += (-wall.transform.position).normalized
            //     }
            // })
            // .AddTo(comp);
        }
    }
}