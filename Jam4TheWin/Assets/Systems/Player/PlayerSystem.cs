using System;
using SystemBase;
using Systems.Control;
using Systems.Movement;
using Systems.Player.States;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Systems.Player
{
    [GameSystem(typeof(MouseTargetSystem), typeof(MovementSystem))]
    public class PlayerSystem : GameSystem<MouseControlComponent, CatComponent>
    {
        private readonly ReactiveProperty<MouseControlComponent> _mouse = new ReactiveProperty<MouseControlComponent>();

        public override void Register(CatComponent component)
        {
            if (_mouse.HasValue)
            {
                _mouse.Subscribe(RegisterCatComponent(component)).AddTo(component);
            }
            else
            {
                _mouse.Skip(1).Subscribe(RegisterCatComponent(component)).AddTo(component);
            }

            var firstState = new Hungry();
            component.CatStateContext = new CatStateContext(firstState, component);
            firstState.Enter(component.CatStateContext);

            component.CatStateContext.CurrentState
                .Subscribe(CatStateChanged(component.CatStateContext))
                .AddTo(component);

            MessageBroker.Default.Receive<CatGetsHitMessage>()
                .Subscribe(CatHit)
                .AddTo(component);
        }

        private void CatHit(CatGetsHitMessage m)
        {
            var targetM = m.Cat.GetComponent<TargetMutator>();
            var target = targetM.Target;
            var movement = m.Cat.GetComponent<MovementComponent>();
            var mutator = m.Cat.gameObject.AddComponent<RunAwayMutator>();
            var template = m.Cat.RunAwayMutatorTemplate;

            mutator.Acceleration = template.Acceleration;
            mutator.MaxDistance = template.MaxDistance;
            mutator.MaxSpeed = template.MaxSpeed;
            mutator.Source = m.Collider.gameObject;
            movement.MovementMutators.Add(mutator);

            Observable.Timer(TimeSpan.FromMilliseconds(50))
                .Subscribe(t=>
                {
                    movement.MovementMutators.Remove(mutator);
                    Object.Destroy(mutator);
                    targetM.Target = target;
                });
        }

        private static Action<ICatState> CatStateChanged(CatStateContext ctx)
        {
            return catState =>
            {
                Debug.Log("State Changed to " + catState.GetType());
            };
        }

        private static Action<MouseControlComponent> RegisterCatComponent(CatComponent cat)
        {
            return mouse =>
            {
                var movement = cat.GetComponent<MovementComponent>();
                mouse.MousePressed.Subscribe(b=>movement.CanMove = b).AddTo(cat);
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