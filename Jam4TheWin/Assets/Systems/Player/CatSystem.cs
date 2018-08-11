using System;
using SystemBase;
using Systems.Player.Movement;

namespace Systems.Player
{
    [GameSystem]
    public class CatSystem : GameSystem<CatComponent, PlayerInteractionComponent>
    {
        private CatComponent _cat;

        public override void Register(CatComponent component)
        {
            _cat = component;
        }

        public override void Register(PlayerInteractionComponent component)
        {
            
        }
    }
}
