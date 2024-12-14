using Controller;

public abstract class BasePersonState
{
    protected PersonController person;

    public BasePersonState(PersonController person)
    {
        this.person = person;
    }

    public virtual void EnterState() { }
    public virtual void UpdateState() { }
    public virtual void ExitState() { }
}