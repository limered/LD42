using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SystemBase.StateMachineBase;
using Systems.Movement;
using Systems.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.DotNet;
using Utils.Plugins;

namespace Systems.People.States
{
    public class Idle : PersonState
    {
        public override ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new[] { typeof(RunningToCat), typeof(Idle) });
            }
        }
        public override bool Enter<TState>(IStateContext<TState> context)
        {
            var ctx = (PersonStateContext)context;

            var spots = UnityEngine.Object.FindObjectsOfType<GatheringSpotComponent>();
            Observable
                .Interval(TimeSpan.FromSeconds(15))
                .StartWith(0)
                .Subscribe(_ =>
                {
                    if (spots.Any())
                    {
                        var movement = ctx.Person.GetComponent<TargetMutator>();
                        movement.Target = spots.RandomElement().gameObject;
                    }
                })
                .AddTo(this);

            ctx.Person
                .OnTriggerEnterAsObservable()
                .Subscribe(collider =>
                {
                    if (collider.GetComponent<LoveColliderComponent>())
                    {
                        ctx.GoToState(new RunningToCat(collider.transform.parent.gameObject));
                    }
                })
                .AddTo(this);

            return true;
        }
    }
}