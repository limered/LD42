using SystemBase;
using UniRx;

namespace Systems.Control
{
    public class MouseControlComponent : GameComponent
    {
        public Vector3ReactiveProperty MousePosition = new Vector3ReactiveProperty();
        public BoolReactiveProperty MousePressed = new BoolReactiveProperty();
    }
}