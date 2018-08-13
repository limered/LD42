using System;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using Systems.Movement;
using UniRx.Triggers;
using UniRx;
using UnityEngine;
using Systems.Room;
using Systems.Score;
using Utils.Plugins;
using Systems.Sound;

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
            target.Target = GameObject.FindObjectOfType<DoorComponent>().gameObject;
            target.MaxSpeed = 0.5f;

            //if door is reached -> kill this person
            ctx.Person
                .OnTriggerEnterAsObservable()
                .WaitForFirst(c => c.GetComponent<DoorComponent>())
                .Subscribe(collider =>
                {
                    MessageBroker.Default.Publish(new MessagePersonLoved());
                    "door_close".Play();
                    GameObject.Destroy(ctx.Person.gameObject);
                })
                .AddTo(this);

            return true;
        }
    }
}