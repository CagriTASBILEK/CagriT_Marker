using Controller;
using UnityEngine;

public class MovingState : BasePersonState
{
    private Vector3 targetPosition;
    private bool isMovingToRightSide;

    public MovingState(PersonController person, Vector3 targetPosition, bool isMovingToRightSide = false) : base(person)
    {
        this.targetPosition = targetPosition;
        this.isMovingToRightSide = isMovingToRightSide;
    }

    public override void EnterState()
    {
        person.StartMoving(targetPosition);
    }

    public override void UpdateState()
    {
        if (person.HasReachedDestination())
        {
            person.StopMoving();
            if (isMovingToRightSide)
            {
                EventManager.Instance.PersonReachedRightSide(person);
            }
            else
            {
                EventManager.Instance.PersonReachedTable(person);
            }
        }
    }

    public override void ExitState()
    {
        person.StopMoving();
    }
}