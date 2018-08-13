using System;
using SystemBase;
using Systems.GameState;
using UniRx;
using UnityEngine;
using Utils;
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
                    if (!(IoC.Game.GameStateMachine.CurrentState.Value is Running)) return;

                    var spawn = comp.Spawns.RandomElement();
                    GameObject.Instantiate(spawn, comp.transform.position, Quaternion.identity);
                })
                .AddTo(comp);
        }
    }
}