using SystemBase.StateMachineBase;

namespace Systems.GameState
{
    public class BasicGameStateContext : StateContextBase<IGameState>
    {
        public BasicGameStateContext(IGameState initialState) : base(initialState)
        {
        }
    }
}
