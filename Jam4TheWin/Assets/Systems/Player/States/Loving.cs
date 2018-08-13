using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SystemBase.StateMachineBase;
using Systems.People;
using Systems.Sound;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.DotNet;

namespace Systems.Player.States
{
    public class Loving : ICatState
    {
        private IDisposable _loveStayDisposable;
        private IDisposable _lovingStopsDisposable;

        public ReadOnlyCollection<Type> ValidNextStates
        {
            get
            {
                return new ReadOnlyCollection<Type>(new List<Type> { typeof(NeedsLove) });
            }
        }

        public bool Enter<TState>(IStateContext<TState> context) where TState : IState
        {
            var ctx = (CatStateContext)context;

            ctx.Cat.StinkCollider.SetActive(false);

            ctx.Cat.LoveTimer.Value = CatComponent.MaxLoveTimer;
            _loveStayDisposable = ctx.Cat.OnTriggerStayAsObservable()
                .Where(coll => coll.GetComponent<PersonComponent>())
                .Subscribe(CatIsLoving(ctx));

            _lovingStopsDisposable = ctx.Cat.OnTriggerExitAsObservable()
                .Where(coll => coll.GetComponent<PersonComponent>())
                .Subscribe(CatStopsLoving(ctx));

            return true;
        }

        private Action<Collider> CatStopsLoving(CatStateContext ctx)
        {
            return coll => { ctx.GoToState(new NeedsLove()); };
        }

        private Action<Collider> CatIsLoving(CatStateContext ctx)
        {
            return coll =>
            {
                ctx.Cat.LoveTimer.Value -= Time.deltaTime;
                if (ctx.Cat.LoveTimer.Value < 0)
                {
                    MessageBroker.Default.Publish(new MessageCatMadeLoveToPerson { Cat = ctx.Cat, Person = coll });
                    ctx.GoToState(new NeedsLove());
                }
            };
        }

        public void Exit()
        {
            _loveStayDisposable.Dispose();
            _lovingStopsDisposable.Dispose();
        }
    }
}