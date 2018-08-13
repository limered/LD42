using System;
using System.Linq;
using SystemBase;
using UniRx;
using UnityEngine;
using Utils.DotNet;

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
            .Subscribe(_ =>
                {
                    var spawn = comp.Spawns.RandomElement();
                    GameObject.Instantiate(spawn, comp.transform.position, Quaternion.identity);
                })
            .AddTo(comp);
        }
    }
}