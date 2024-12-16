using Controller;
using State;

public class IdleState : BasePersonState
{
    public IdleState(PersonController person) : base(person) { }

    public override void EnterState()
    {
        person.StopMoving();
    }
}