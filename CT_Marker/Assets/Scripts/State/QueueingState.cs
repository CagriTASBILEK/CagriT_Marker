using Controller;

public class QueueingState : BasePersonState
{
    public QueueingState(PersonController person) : base(person)
    {
        EventManager.Instance.PersonJoinedQueue(person);
    }
}
