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
    public class Full : ICatState
    {
        private IDisposable _poopBoxDisposable;
        private IDisposable _enemyHitDisposable;

        public ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new List<Type> { typeof(Pooping) });
            }
        }

        public bool Enter<TState>(IStateContext<TState> context) where TState : IState
        {
            var ctx = (CatStateContext) context;
            ctx.Cat.Hunger.Value = CatComponent.MaxHunger;

            _poopBoxDisposable = ctx.Cat.OnTriggerEnterAsObservable()
                .Where(coll => !ctx.Cat.IsAngry)
                .Where(coll=>coll.GetComponent<LooComponent>())
                .Subscribe(CatStartsPooping(ctx));

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

        private Action<UnityEngine.Collider> CatStartsPooping(CatStateContext context)
        {
            return coll => context.GoToState(new Pooping());
        }

        public void Exit()
        {
            _poopBoxDisposable.Dispose();
            _enemyHitDisposable.Dispose();
        }
    }
}