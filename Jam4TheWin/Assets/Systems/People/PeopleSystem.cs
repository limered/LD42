using System;
using SystemBase;
using Systems.GameState;
using Systems.Movement;
using Systems.People.States;
using UniRx;
using UnityEngine;
using Utils;

namespace Systems.People
{
    [GameSystem]
    public class PeopleSystem : GameSystem<PersonComponent>
    {
        public override void Register(PersonComponent comp)
        {
            var firstState = new Entering();
            comp.StateContext = new PersonStateContext(firstState, comp);
            firstState.Enter(comp.StateContext);

            IoC.Game.GameStateMachine.CurrentState.Where(s => s is Running)
                .Subscribe(s => comp.GetComponent<MovementComponent>().CanMove.Value = true)
                .AddTo(comp);

            if (IoC.Game.GameStateMachine.CurrentState.Value is Running)
            {
                comp.GetComponent<MovementComponent>().CanMove.Value = true;
            }

            IoC.Game.GameStateMachine.CurrentState
                .Where(s => s is GameOver)
                .Subscribe(Die(comp))
                .AddTo(comp);
        }

        private static Action<IGameState> Die(PersonComponent comp)
        {
            return s => GameObject.Destroy(comp.gameObject);
        }
    }
}