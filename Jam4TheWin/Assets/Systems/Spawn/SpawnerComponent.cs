using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemBase;
using UniRx;

namespace Systems.Room
{
    public class SpawnerComponent : GameComponent
    {
        public GameObject Spawns;
        public FloatReactiveProperty Interval = new FloatReactiveProperty(5);
    }
}
