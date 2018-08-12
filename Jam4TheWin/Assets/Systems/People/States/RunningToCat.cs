using System;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using UniRx.Triggers;
using UniRx;
using Systems.Player;
using Systems.Movement;
using UnityEngine;

namespace Systems.People.States
{
    public class RunningToCat : PersonState
    {
        private GameObject _cat;
        public RunningToCat(GameObject cat)
        {
            _cat = cat;
        }

        public override ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new [] { typeof(Idle), typeof(Angry) });
            }
        }
        public override bool Enter<TState>(IStateContext<TState> context)
        {
            var ctx = (PersonStateContext)context;

            var target = ctx.Person.GetComponent<TargetMutator>();
            target.Target = _cat;

            //noticing that the cat stinks
            ctx.Person
                .OnTriggerEnterAsObservable()
                .Subscribe(collider =>
                {
                    if (collider.GetComponent<StinkColliderComponent>())
                    {
                        ctx.GoToState(new Angry());
                    }
                })
                .AddTo(this);

            //running out of love radius
            ctx.Person
                .OnTriggerExitAsObservable()
                .Subscribe(collider =>
                {
                    if (collider.GetComponent<LoveColliderComponent>())
                    {
                        ctx.GoToState(new Idle());
                    }
                })
                .AddTo(this);

            return true;
        }
    }
}