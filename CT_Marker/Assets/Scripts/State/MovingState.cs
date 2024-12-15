using System.Collections.Generic;
using Controller;
using UnityEngine;

public class MovingState : BasePersonState
{
    private Vector3 targetPosition;
    private bool isMovingToRightSide;
    private float checkInterval = 0.1f;
    private float nextCheckTime;
    private float minDistanceToCheck = 1f;


    public MovingState(PersonController person, Vector3 targetPosition, bool isMovingToRightSide = false) 
        : base(person)
    {
        this.targetPosition = targetPosition;
        this.isMovingToRightSide = isMovingToRightSide;
        nextCheckTime = Time.time + checkInterval;
    }

    public override void EnterState()
    {
        person.StartMoving(targetPosition);
    }

    public override void UpdateState()
    {
        if (!isMovingToRightSide)
        {
            float distanceToTarget = Vector3.Distance(person.transform.position, targetPosition);
            if (distanceToTarget > minDistanceToCheck && Time.time >= nextCheckTime)
            {
                CheckQueuePosition();
                nextCheckTime = Time.time + checkInterval;
            }

            if (person.HasReachedDestination())
            {
                EventManager.Instance.PersonReachedTable(person);
            }
        }
    }
    private void CheckQueuePosition()
    {
        var personManager = GameManager.Instance.personManager;
        var queueList = new List<PersonController>(personManager.queuedPeople);
        int currentIndex = queueList.IndexOf(person);

        if (currentIndex > 0)
        {
            PersonController personInFront = queueList[currentIndex - 1];

            if (person.transform.position.z > personInFront.transform.position.z)
            {
                queueList[currentIndex] = personInFront;
                queueList[currentIndex - 1] = person;
                
                personManager.queuedPeople = new Queue<PersonController>(queueList);
                
                personManager.UpdateQueuePositions();
                
                EventManager.Instance.CheckQueuePosition(person);
            }
        }
    }


    public override void ExitState()
    {
        person.StopMoving();
    }
}