using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SystemBase.StateMachineBase;
using Systems.Movement;
using Systems.Sound;
using UniRx;
using UniRx.Triggers;
using Utils.DotNet;
using Utils.Plugins;
using Object = UnityEngine.Object;

namespace Systems.People.States
{
    public class Entering : PersonState
    {
        public override ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new [] { typeof(Idle) });
            }
        }

        public override bool Enter<TState>(IStateContext<TState> context)
        {
            var ctx = (PersonStateContext)context;
            var spots = Object.FindObjectsOfType<GatheringSpotComponent>();

            "door_close".Play();

            if (spots.Any())
            {
                var movement = ctx.Person.GetComponent<TargetMutator>();
                movement.Target = spots.RandomElement().gameObject;

                ctx.Person.OnTriggerEnterAsObservable()
                    .WaitForFirst(coll => coll.gameObject == movement.Target)
                    .Subscribe(_ => ctx.GoToState(new Idle()))
                    .AddTo(this);
            }

            return true;
        }
    }
}