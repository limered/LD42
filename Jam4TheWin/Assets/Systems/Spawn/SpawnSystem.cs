using System.Collections;
using System.Collections.Generic;
using SystemBase;
using UniRx.Triggers;
using UniRx;
using UnityEngine;
using Utils.Plugins;
using System.Linq;
using System;

namespace Systems.Room
{
    [GameSystem]
    public class SpawnSystem : GameSystem<SpawnerComponent>
    {
        public override void Register(SpawnerComponent comp)
        {
            comp.Interval
            .SelectMany(x => Observable.Interval(TimeSpan.FromSeconds(comp.Interval.Value)).TakeUntil(comp.Interval.Skip(1).Take(1)))
            .Where(_ => comp.Spawns != null)
            .Subscribe(_ => GameObject.Instantiate(comp.Spawns, comp.transform.position, Quaternion.identity))
            .AddTo(comp);
        }
    }
}