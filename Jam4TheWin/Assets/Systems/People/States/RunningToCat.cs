using System;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using UniRx.Triggers;
using UniRx;
using Systems.Player;

namespace Systems.People.States
{
    public class RunningToCat : PersonState
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