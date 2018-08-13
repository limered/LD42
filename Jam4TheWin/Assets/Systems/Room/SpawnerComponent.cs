using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Room
{
    public class SpawnerComponent : GameComponent
    {
        public GameObject[] Spawns;
        public FloatReactiveProperty Interval = new FloatReactiveProperty(5);
    }
}
