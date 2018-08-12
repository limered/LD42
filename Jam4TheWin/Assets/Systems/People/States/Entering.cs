using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SystemBase.StateMachineBase;
using Systems.Movement;
using UniRx;
using UniRx.Triggers;
using Utils.DotNet;
using Object = UnityEngine.Object;

namespace Systems.People.States
{
    public class Entering : IPersonState
    {
        private IDisposable _reachedStartSpot;

        public ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new List<Type> { typeof(Idle) });
            }
        }

        public bool Enter<TState>(IStateContext<TState> context) where TState : IState
        {
            var ctx = (PeapoleStateContext)context;
            var spots = Object.FindObjectsOfType<GatheringSpotComponent>();
            if (spots.Any())
            {
                var movement = ctx.Person.GetComponent<TargetMutator>();
                movement.Target = spots.RandomElement().gameObject;

                _reachedStartSpot = ctx.Person.OnTriggerEnterAsObservable()
                    .Where(coll => coll.gameObject == movement.Target)
                    .Subscribe(coll => ctx.GoToState(new Idle()));
            }

            return true;
        }

        public void Exit()
        {
            _reachedStartSpot.Dispose();
        }
    }
}