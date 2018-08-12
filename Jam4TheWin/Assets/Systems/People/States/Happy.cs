using System;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using Systems.Movement;
using UniRx.Triggers;
using UniRx;
using UnityEngine;
using Systems.Room;

namespace Systems.People.States
{
    public class Happy : PersonState
    {
        public override ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new Type[] { });
            }
        }
        public override bool Enter<TState>(IStateContext<TState> context)
        {
            var ctx = (PersonStateContext)context;

            var target = ctx.Person.GetComponent<TargetMutator>();
            target.Target = GameObject.FindGameObjectWithTag("door");

            //if door is reached -> kill this person
            ctx.Person
                .OnTriggerEnterAsObservable()
                .Subscribe(collider =>
                {
                    if (collider.GetComponent<DoorComponent>())
                    {
                        GameObject.Destroy(ctx.Person.gameObject);
                    }
                })
                .AddTo(this);

            return true;
        }
    }
}