using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Movement
{
    public interface ITest {}

    public class MovementComponent : GameComponent
    {
        public MovementMutator[] MovementMutators; 
        public bool CanMove;

        public Vector3ReactiveProperty Direction = new Vector3ReactiveProperty(Vector3.zero);
        public FloatReactiveProperty Speed = new FloatReactiveProperty(0);
    }
}