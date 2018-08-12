using SystemBase;
using Systems.People.States;

namespace Systems.People
{
    [GameSystem]
    public class PeopleSystem : GameSystem<PersonComponent>
    {
        public override void Register(PersonComponent comp)
        {
            var firstState = new Entering();
            comp.StateContext = new PersonStateContext(firstState, comp);
            firstState.Enter(comp.StateContext);
        }
    }
}