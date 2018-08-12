using SystemBase.StateMachineBase;
using Systems.Player.States;

namespace Systems.Player
{
    public class CatStateContext : StateContextBase<ICatState>
    {
        public CatComponent Cat;

        public CatStateContext(ICatState initialState, CatComponent cat) : base(initialState)
        {
            Cat = cat;
        }
    }
}