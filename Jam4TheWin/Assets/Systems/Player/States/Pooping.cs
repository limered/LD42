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
    public class Pooping : ICatState
    {
        private IDisposable _poopintDisposable;
        private IDisposable _poopingStopDisposable;
        private IDisposable _enemyHitDisposable;

        public ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new List<Type>{typeof(Full), typeof(NeedsLove) });
            }
        }

        public bool Enter<TState>(IStateContext<TState> context) where TState : IState
        {
            var ctx = (CatStateContext) context;

            ctx.Cat.PoopingTimer.Value = CatComponent.MaxPoopingTime;
            _poopintDisposable = ctx.Cat.OnTriggerStayAsObservable()
                .Where(coll => !ctx.Cat.IsAngry)
                .Where(coll => coll.GetComponent<LooComponent>())
                .Subscribe(CatPoops(ctx));

            _poopingStopDisposable = ctx.Cat.OnTriggerExitAsObservable()
                .Where(coll => coll.GetComponent<LooComponent>())
                .Subscribe(CatStopsPooping(ctx));

            _enemyHitDisposable = ctx.Cat.InnerSpaceCollider
                .OnTriggerStayAsObservable()
                .Where(coll => !ctx.Cat.IsAngry)
                .Where(coll => coll.GetComponent<PersonComponent>())
                .Subscribe(GetHit(ctx.Cat));

            return true;
        }

        private Action<Collider> GetHit(CatComponent cat)
        {
            return coll => MessageBroker.Default.Publish(new CatGetsHitMessage { Cat = cat, Collider = coll });
        }

        private Action<Collider> CatStopsPooping(CatStateContext context)
        {
            return coll => { context.GoToState(new Full()); };
        }

        private Action<Collider> CatPoops(CatStateContext context)
        {
            return coll =>
            {
                context.Cat.PoopingTimer.Value -= Time.deltaTime;
                if (context.Cat.PoopingTimer.Value < 0)
                {
                    context.GoToState(new NeedsLove());
                }
            };
        }

        public void Exit()
        {
            _poopintDisposable.Dispose();
            _poopingStopDisposable.Dispose();
            _enemyHitDisposable.Dispose();
        }
    }
}