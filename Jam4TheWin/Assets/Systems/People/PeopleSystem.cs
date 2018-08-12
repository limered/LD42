using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UniRx.Triggers;
using UniRx;
using UnityEngine;
using Utils.Plugins;
using Utils.Math;
using Systems.Movement;
using System;

namespace Systems.People
{
    [GameSystem]
    public class PeopleSystem : GameSystem<PersonComponent>
    {
        public override void Register(PersonComponent comp)
        {
            var moveComp = comp.GetComponent<TargetedMovementComponent>();

            Observable.Interval(TimeSpan.FromSeconds(10))
                .StartWith(0)
                .Subscribe(_ => GoToNextLocation(comp, moveComp))
                .AddTo(comp);
        }

        private void GoToNextLocation(PersonComponent comp, TargetedMovementComponent moveComp)
        {
            var spots = GameObject.FindGameObjectsWithTag("gathering spot");
            if (spots.Length > 0) moveComp.Target = spots[UnityEngine.Random.Range(0, spots.Length)];
        }
    }
}