using SystemBase.StateMachineBase;
using Systems.People.States;

namespace Systems.People
{
    public class PeapoleStateContext : StateContextBase<IPersonState>
    {
        public PersonComponent Person { get; private set; }
        public PeapoleStateContext(IPersonState initialState, PersonComponent p) : base(initialState)
        {
            Person = p;
        }
    }
}
