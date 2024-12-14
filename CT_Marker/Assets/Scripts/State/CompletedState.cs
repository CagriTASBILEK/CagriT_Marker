using Controller;

public class CompletedState : BasePersonState
{
    public CompletedState(PersonController person) : base(person) { }

    public override void EnterState()
    {
        person.StopMoving();
    }
}
