using System;
using SystemBase;
using Systems.Control;
using Systems.GameState;
using Systems.Movement;
using Systems.Player.States;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils;

namespace Systems.Player
{
    [GameSystem(typeof(MouseTargetSystem), typeof(MovementSystem))]
    public class PlayerSystem : GameSystem<MouseControlComponent, CatComponent, PlayerSpawner>
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
            if (m.Cat.IsAngry) return;
            m.Cat.IsAngry = true;
            Observable.Timer(TimeSpan.FromSeconds(m.Cat.AngryTime))
                .Subscribe(t => m.Cat.IsAngry = false);
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
                mouse.MousePressed.Subscribe(b => movement.CanMove.Value = b).AddTo(cat);
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

        public override void Register(PlayerSpawner component)
        {
            IoC.Game.GameStateMachine.CurrentState
                .Where(s => s is Running)
                .Select(s=>component)
                .Subscribe(SpawnPlayer)
                .AddTo(component);
        }

        private void SpawnPlayer(PlayerSpawner component)
        {
            var player = GameObject.Instantiate(component.PlayerPrefab, component.transform);
            player.GetComponent<TargetMutator>().Target = component.Mouse;

            IoC.Game.GameStateMachine.CurrentState
                .Where(s => s is GameOver)
                .Subscribe(s => GameObject.Destroy(player))
                .AddTo(player);
        }
    }
}