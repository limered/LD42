using SystemBase;
using UniRx;
using UnityEngine;

namespace Systems.Control
{
    public class PlayerInteractionComponent : GameComponent
    {
        public GameObject MouseCollisionPlane;

        public Vector2ReactiveProperty MousePosition = new Vector2ReactiveProperty();
        public BoolReactiveProperty MousePressed = new BoolReactiveProperty();
    }
}