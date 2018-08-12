using SystemBase;
using Systems.Player;
using UniRx;

namespace Systems.Interactables
{
    public class InteractionSystem : GameSystem<LooComponent, FoodComponent>
    {
        public override void Register(FoodComponent component) { }

        public override void Register(LooComponent component) { }
    }
}
