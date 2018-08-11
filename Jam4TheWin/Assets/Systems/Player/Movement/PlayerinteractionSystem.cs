using System;
using SystemBase;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Systems.Player.Movement
{
    [GameSystem]
    public class PlayerinteractionSystem : GameSystem<PlayerInteractionComponent>
    {
        public override void Register(PlayerInteractionComponent component)
        {
            component.UpdateAsObservable()
                .Subscribe(CheckMousePorition(component))
                .AddTo(component);
        }

        private static Action<Unit> CheckMousePorition(PlayerInteractionComponent component)
        {
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
                if (!Physics.Raycast(ray, out hit, Mathf.Infinity)) return;

                component.MousePosition.Value = new Vector2(hit.point.x, hit.point.z);
            };
        }
    }
}
