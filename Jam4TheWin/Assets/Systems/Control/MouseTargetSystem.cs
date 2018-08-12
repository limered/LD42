using System;
using SystemBase;
using Systems.Room;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Systems.Control
{
    [GameSystem]
    public class MouseTargetSystem : GameSystem<MouseControlComponent>
    {
        public override void Register(MouseControlComponent component)
        {
            component.UpdateAsObservable()
                .Subscribe(CheckMousePorition(component))
                .AddTo(component);
        }

        private static Action<Unit> CheckMousePorition(MouseControlComponent component)
        {
            var floorLayer = LayerMask.NameToLayer("Floor");

            return u =>
            {
                if(Input.GetMouseButtonDown((int) MouseButton.LeftMouse))
                {
                    component.MousePressed.Value = true;
                }
                if (Input.GetMouseButtonUp((int) MouseButton.LeftMouse))
                {
                    component.MousePressed.Value = false;
                }

                if (!Input.GetMouseButton((int) MouseButton.LeftMouse)) return;

                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (!Physics.Raycast(ray, out hit, Mathf.Infinity, 1<<floorLayer)) return;

                component.MousePosition.Value = new Vector3(hit.point.x, 0, hit.point.z);
            };
        }
    }
}
