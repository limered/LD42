using System;
using SystemBase;
using Systems.Control;
using Systems.Movement;
using UniRx;
using UniRx.Triggers;

namespace Systems.Player
{
    [GameSystem(typeof(MouseTargetSystem), typeof(MovementSystem))]
    public class PlayerSystem : GameSystem<MouseControlComponent, CatComponent>
    {
        private readonly ReactiveProperty<MouseControlComponent> _mouse = new ReactiveProperty<MouseControlComponent>();

        public override void Register(CatComponent component)
        {
            _mouse.Skip(1).Subscribe(RegisterCatComponent(component)).AddTo(component);
        }

        private static Action<MouseControlComponent> RegisterCatComponent(CatComponent cat)
        {
            return mouse =>
            {
                var movement = cat.GetComponent<TargetedMovementComponent>();
                mouse.MousePressed.Subscribe(b=>movement.IsMoving = b).AddTo(cat);
            };
        }

        public override void Register(MouseControlComponent component)
        {
            _mouse.Value = component;
            component.UpdateAsObservable()
                .Select(_ => component)
                .Subscribe(UpdateMouseTargetPosition)
                .AddTo(component);
        }

        private void UpdateMouseTargetPosition(MouseControlComponent component)
        {
            component.transform.position = component.MousePosition.Value;
        }
    }
}
