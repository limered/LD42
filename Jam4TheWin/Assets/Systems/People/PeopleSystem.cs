using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UniRx.Triggers;
using UniRx;
using UnityEngine;
using Utils.Plugins;
using Utils.Math;
using Systems.Movement;

namespace Systems.People
{
    [GameSystem]
    public class PeopleSystem : GameSystem<PersonComponent>
    {
        public override void Register(PersonComponent comp)
        {
            var moveComp = comp.GetComponent<TargetedMovementComponent>();
            comp.UpdateAsObservable()
                .Where((_, i) => i % 60 == 0)
                .Subscribe(_ => MoveFunny(comp, moveComp))
                .AddTo(comp);
        }

        private void MoveFunny(PersonComponent comp, TargetedMovementComponent moveComp)
        {
            var direction = new Vector3().RandomVector(new Vector3(-comp.Speed, -comp.Speed, -comp.Speed), new Vector3(comp.Speed, comp.Speed, comp.Speed)).normalized;
            direction.y = 0;

            moveComp.Direction.Value = direction;
            moveComp.Distance.Value = moveComp.MaxSpeed;

            //CoRoutines are normally only possible on MonoBehaviour/Render Thread. This is again UniRx Magic
            // MainThreadDispatcher.StartUpdateMicroCoroutine(MoveStraight(direction, comp));
        }

        /**
         * CoRoutine
         */

        private IEnumerator MoveStraight(Vector3 direction, Component comp)
        {
            for (var i = 0; i < 30; i++)
            {
                comp.transform.position += direction * Time.deltaTime;
                yield return null;
            }
        }
    }
}