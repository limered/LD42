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
    public class Hungry : ICatState
    {
        private IDisposable _catEatingDisposable;
        private IDisposable _enemyHitDisposable;

        public ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new List<Type> {typeof(Eating)});
            }
        }

        public bool Enter<TState>(IStateContext<TState> context) where TState : IState
        {
            var ctx = (CatStateContext)context;

            ctx.Cat.StinkCollider.SetActive(false);
            ctx.Cat.InnerSpaceCollider.SetActive(true);

            _catEatingDisposable = ctx.Cat.OnTriggerEnterAsObservable()
                .Where(IsFood())
                .Where(coll=>!ctx.Cat.IsAngry)
                .Subscribe(CatStartsEating(ctx));

            _enemyHitDisposable = ctx.Cat.InnerSpaceCollider
                .OnTriggerStayAsObservable()
                .Where(coll => !ctx.Cat.IsAngry)
                .Where(coll => coll.GetComponent<PersonComponent>())
                .Subscribe(GetHit(ctx.Cat));

            return true;
        }

        private Action<Collider> GetHit(CatComponent cat)
        {
            return coll => MessageBroker.Default.Publish(new CatGetsHitMessage{Cat = cat, Collider = coll});
        }

        private static Func<Collider, bool> IsFood()
        {
            return coll => coll.GetComponent<FoodComponent>();
        }

        private Action<Collider> CatStartsEating(CatStateContext context)
        {
            return coll => context.GoToState(new Eating());
        }

        public void Exit()
        {
            _catEatingDisposable.Dispose();
            _enemyHitDisposable.Dispose();
        }
    }
}
