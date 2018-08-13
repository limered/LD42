using System;
using SystemBase;
using Systems.People.States;
using Systems.Score;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Systems.People
{
    [GameSystem]
    public class PeopleSystem : GameSystem<PersonComponent, DespawnerComponent>
    {
        public override void Register(PersonComponent comp)
        {
            var firstState = new Entering();
            comp.StateContext = new PersonStateContext(firstState, comp);
            firstState.Enter(comp.StateContext);
        }

        public override void Register(DespawnerComponent component)
        {
            component.OnTriggerEnterAsObservable()
                .Where(coll => coll.GetComponent<PersonComponent>())
                .Where(coll => coll.GetComponent<PersonComponent>().StateContext.CurrentState.Value.GetType().Name == typeof(Happy).Name)
                .Subscribe(LovedPersonLeft())
                .AddTo(component);

            component.OnTriggerEnterAsObservable()
                .Where(coll => coll.GetComponent<PersonComponent>())
                .Where(coll => coll.GetComponent<PersonComponent>().StateContext.CurrentState.Value.GetType().Name == typeof(Angry).Name)
                .Subscribe(AngryPersonLeft())
                .AddTo(component);
        }

        private static Action<Collider> LovedPersonLeft()
        {
            return coll =>
            {
                MessageBroker.Default.Publish(new MessagePersonLoved());
                Object.Destroy(coll.gameObject);
            };
        }

        private static Action<Collider> AngryPersonLeft()
        {
            return coll =>
            {
                MessageBroker.Default.Publish(new MessagePersonLeftAngry());
                Object.Destroy(coll.gameObject);
            };
        }
    }
}