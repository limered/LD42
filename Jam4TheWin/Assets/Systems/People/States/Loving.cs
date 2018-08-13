using System;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using Systems.Movement;
using Systems.Player;
using UniRx;
using UniRx.Triggers;

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

            ctx.LovingCount.Value = 1;
            Disposable.Create(() => ctx.LovingCount.Value = 0).AddTo(this); //reset to 0 when Exiting

            var target = ctx.Person.GetComponent<TargetMutator>();
            target.Target = null;

            //standing long enough near the cat to love it
            ctx.LovingCount
                .Select(x => x > 0)
                .DistinctUntilChanged()
                .Throttle(TimeSpan.FromSeconds(ctx.Person.TimeUntilHappy))
                .Where(x => x == true)
                .Subscribe(_ => ctx.GoToState(new Happy()))
                .AddTo(this);

            //increase LovingCount
            ctx.Person
                .OnTriggerEnterAsObservable()
                .Where(c => c.GetComponent<InnerSpaceColliderComponent>())
                .Subscribe(collider =>
                {
                    ctx.LovingCount.Value++;
                    ctx.GoToState(new Angry());
                })
                .AddTo(this);

            //descrease LovingCount
            ctx.Person
                .OnTriggerExitAsObservable()
                .Where(c => c.GetComponent<InnerSpaceColliderComponent>())
                .Subscribe(collider =>
                {
                    ctx.LovingCount.Value = Math.Max(ctx.LovingCount.Value - 1, 0);
                    if (ctx.LovingCount.Value == 0)
                    {
                        ctx.GoToState(new Angry());
                    }
                })
                .AddTo(this);

            return true;
        }
    }
}