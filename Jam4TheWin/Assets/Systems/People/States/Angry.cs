using System;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using Systems.Movement;
using UniRx.Triggers;
using UniRx;
using UnityEngine;
using Systems.Room;
using Systems.Player;
using Systems.Score;
using Utils.Plugins;

namespace Systems.People.States
{
    public class Angry : PersonState
    {
        public override ReadOnlyCollection<Type> ValidNextStates { get { return new ReadOnlyCollection<Type>(new Type[] { typeof(Loving) }); } }
        public override bool Enter<TState>(IStateContext<TState> context)
        {
            var ctx = (PersonStateContext)context;

            var target = ctx.Person.GetComponent<TargetMutator>();
            target.Target = GameObject.FindObjectOfType<DoorComponent>().gameObject;

            //Cat gets really close
            ctx.Person
                .OnTriggerEnterAsObservable()
                .WaitForFirst(c => c.GetComponent<InnerSpaceColliderComponent>())
                .Subscribe(_ => ctx.GoToState(new Loving()))
                .AddTo(this);

            //if door is reached -> kill this person
            ctx.Person
                .OnTriggerEnterAsObservable()
                .WaitForFirst(c => c.GetComponent<DoorComponent>())
                .Subscribe(collider =>
                {
                    MessageBroker.Default.Publish(new MessagePersonLeftAngry());
                    GameObject.Destroy(ctx.Person.gameObject);
                })
                .AddTo(this);

            return true;
        }
    }
}