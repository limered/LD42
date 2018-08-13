using System;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using Systems.Movement;
using Systems.Player;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.Plugins;

namespace Systems.People.States
{
    public class Loving : PersonState
    {
        public override ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new Type[] { typeof(Angry), typeof(Happy) });
            }
        }
        public override bool Enter<TState>(IStateContext<TState> context)
        {
            var ctx = (PersonStateContext)context;

            var target = ctx.Person.GetComponent<TargetMutator>();
            target.Target = null;

            //cat approved love
            MessageBroker.Default.Receive<MessageCatMadeLoveToPerson>()
                .WaitForFirst(x => x.Person == ctx.Person.GetComponent<Collider>())
                .Subscribe(_ => ctx.GoToState(new Happy()))
                .AddTo(this);

            //cat's love is gone
            ctx.Person
                .OnTriggerExitAsObservable()
                .WaitForFirst(c => c.GetComponent<InnerSpaceColliderComponent>())
                .Subscribe(_ => ctx.GoToState(new Angry()))
                .AddTo(this);

            return true;
        }
    }
}