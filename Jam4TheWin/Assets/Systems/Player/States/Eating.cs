using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using Systems.Interactables;
using Systems.People;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Systems.Player.States
{
    public class Eating : ICatState
    {
        private IDisposable _catUpdateDisposable;
        private IDisposable _catTriggerExitDisposable;
        private IDisposable _enemyHitDisposable;

        public ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new List<Type> {typeof(Full), typeof(Hungry)});
            }
        }

        public bool Enter<TState>(IStateContext<TState> context) where TState : IState
        {
            var ctx = (CatStateContext) context;
            _catTriggerExitDisposable = ctx.Cat.OnTriggerExitAsObservable()
                .Where(IsFood())
                .Subscribe(CatStopsEating(ctx));

            _catUpdateDisposable = ctx.Cat.OnTriggerStayAsObservable()
                .Where(coll => !ctx.Cat.IsAngry)
                .Where(IsFood())
                .Subscribe(CatFeeding(ctx));

            _enemyHitDisposable = ctx.Cat.InnerSpaceCollider
                .OnTriggerStayAsObservable()
                .Where(coll => !ctx.Cat.IsAngry)
                .Where(coll => coll.GetComponent<PersonComponent>())
                .Subscribe(GetHit(ctx.Cat));

            return true;
        }

        private Action<Collider> GetHit(CatComponent cat)
        {
            return coll => { MessageBroker.Default.Publish(new CatGetsHitMessage {Cat = cat, Collider = coll}); };
        }

        private static Func<Collider, bool> IsFood()
        {
            return coll => coll.GetComponent<FoodComponent>();
        }

        private Action<Collider> CatFeeding(CatStateContext ctx)
        {
            return coll =>
            {
                var food = coll.GetComponent<FoodComponent>();
                ctx.Cat.Hunger.Value -= food.HungerReduceFactor * Time.deltaTime;
                if (ctx.Cat.Hunger.Value < 0)
                {
                    ctx.GoToState(new Full());
                }
            };
        }

        private Action<Collider> CatStopsEating(CatStateContext context)
        {
            return coll => context.GoToState(new Hungry());
        }

        public void Exit()
        {
            _catUpdateDisposable.Dispose();
            _catTriggerExitDisposable.Dispose();
            _enemyHitDisposable.Dispose();
        }
    }
}