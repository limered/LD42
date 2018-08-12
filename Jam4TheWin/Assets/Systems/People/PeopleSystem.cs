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
        private GatheringSpotComponent[] _gatheringSpots;

        public override void Init()
        {
            base.Init();
            _gatheringSpots = GameObject.FindObjectsOfType<GatheringSpotComponent>();
        }

        public override void Register(PersonComponent comp)
        {
            var targetComp = comp.GetComponent<TargetMutator>();

            Observable.Interval(TimeSpan.FromSeconds(10))
                .StartWith(0)
                .Subscribe(_ => GoToNextLocation(comp, targetComp))
                .AddTo(comp);
        }

        private void GoToNextLocation(PersonComponent comp, TargetMutator targetComp)
        {
            var spots = GameObject.FindGameObjectsWithTag("gathering spot");
            if (spots.Length > 0) targetComp.Target = spots[UnityEngine.Random.Range(0, spots.Length)];
        }
    }
}