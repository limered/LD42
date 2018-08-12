using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UniRx.Triggers;
using UniRx;
using UnityEngine;
using Utils.Plugins;
using System.Linq;

namespace Systems.Room
{
    [GameSystem]
    public class RoomSystem : GameSystem<FloorComponent, WallComponent, KeepInsideRoomComponent, FurnitureComponent>
    {
        private FloorComponent floor;

        private List<WallComponent> walls = new List<WallComponent>();
        private List<FurnitureComponent> furniture = new List<FurnitureComponent>();

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

                if (comp.CustomOffsetToGround == 0f) pos.y = comp.GetComponent<Renderer>().bounds.extents.y / 2f;
                else pos.y = comp.CustomOffsetToGround;

                comp.transform.position = pos;
            })
            .AddTo(comp);

            //push out of objects
            var personCollider = comp.GetComponent<Collider>();
            comp.OnTriggerStayAsObservable()
            // .Where(c => c.tag == "wall" || c.tag == "furniture")
            .Subscribe(c =>
            {
                comp.transform.position += (comp.transform.position - c.transform.position) * Time.deltaTime;
            })
            .AddTo(comp);


            //don't go through walls or furniture
            comp.Dependency.CanMoveInDirection = dir =>
            {
                foreach (var collider in walls.Select(x => x.GetComponent<Collider>()).Concat(furniture.Select(x => x.GetComponent<Collider>())).ToArray())
                {
                    float intersectionDistance;
                    if (collider.bounds.IntersectRay(new Ray(personCollider.transform.position, dir), out intersectionDistance))
                    {
                        if (intersectionDistance <= dir.magnitude)
                        {
                            return false;
                        }
                    }
                }
                return true;
            };
        }

        public override void Register(FurnitureComponent comp)
        {
            if (!furniture.Contains(comp))
            {
                furniture.Add(comp);
            }
        }
    }
}