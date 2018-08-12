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
                return new ReadOnlyCollection<Type>(new [] { typeof(Idle) });
            }
        }
        public override bool Enter<TState>(IStateContext<TState> context)
        {
            var ctx = (PersonStateContext)context;

            var target = ctx.Person.GetComponent<TargetMutator>();
            target.Target = _cat;

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