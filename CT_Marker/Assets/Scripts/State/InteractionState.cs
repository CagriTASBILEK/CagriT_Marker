using Controller;
using UnityEngine;

public class InteractingState : BasePersonState
{
    private float interactionTimer;

    public InteractingState(PersonController person) : base(person)
    {
        interactionTimer = 0f;
    }

    public override void EnterState()
    {
        EventManager.Instance.PersonStartedInteraction(person);
    }

    public override void UpdateState()
    {
        interactionTimer += Time.deltaTime;
        
        if (interactionTimer >= GameManager.Instance.Resources.interactionDuration)
        {
            EventManager.Instance.PersonFinishedInteraction(person);
        }
    }
    public override void ExitState() 
    {
        interactionTimer = 0f;
    }
}