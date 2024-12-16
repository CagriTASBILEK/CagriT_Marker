using System.Collections.Generic;
using Controller;
using Managers;
using UnityEngine;

namespace State
{
    /// <summary>
    /// Represents the state when a person is moving to a target position, handling queue position checks and arrival events
    /// </summary>
    public class MovingState : BasePersonState
    {
        private Vector3 targetPosition;
        private bool isMovingToRightSide;
        private float checkInterval = 0.1f;
        private float nextCheckTime;
        private float minDistanceToCheck = 1f;


        /// <summary>
        /// Initializes moving state with target position and right side movement flag
        /// </summary>
        /// <param name="person">Person controller reference</param>
        /// <param name="targetPosition">Target position to move to</param>
        /// <param name="isMovingToRightSide">Flag indicating if moving to right side after interaction</param>
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
        /// <summary>
        /// Checks and updates queue position if person is ahead of the person in front
        /// </summary>
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
}