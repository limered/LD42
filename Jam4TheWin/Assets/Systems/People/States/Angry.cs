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
        private GameObject _cat;
        public Angry(GameObject cat)
        {
            _cat = cat;
        }

        public override ReadOnlyCollection<Type> ValidNextStates { get { return new ReadOnlyCollection<Type>(new Type[] { typeof(Loving) }); } }
        public override bool Enter<TState>(IStateContext<TState> context)
        {
            var ctx = (PersonStateContext)context;

            var target = ctx.Person.GetComponent<TargetMutator>();
            target.Target = GameObject.FindObjectOfType<DoorComponent>().gameObject;
            target.MaxSpeed = 0.02f;

            var runAway = ctx.Person.GetComponent<RunAwayMutator>();
            runAway.Source = _cat;

            //Cat gets really close
            ctx.Person
                .OnTriggerEnterAsObservable()
                .WaitForFirst(c => c.GetComponent<CatComponent>())
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