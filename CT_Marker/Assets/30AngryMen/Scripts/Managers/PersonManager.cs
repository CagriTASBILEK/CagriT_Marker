using System.Collections.Generic;
using Controller;
using State;
using Unity.AI.Navigation;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Manages person spawning, queuing, and interactions with the table using object pooling
    /// </summary>
    public class PersonManager : MonoBehaviour
    {
        [Header("References")] private Transform tableTransform;

        private ObjectPool<PersonController> personPool;
        [SerializeField] private List<PersonController> availablePeople;
        public Queue<PersonController> queuedPeople;
        [SerializeField] private PersonController currentInteractingPerson;

        private GameResources Resources => GameManager.Instance.Resources;
        private bool isInitialized;

        public void Initialize()
        {
            if (isInitialized) return;

            InitializeCollections();
            InitializePool();
            SubscribeToEvents();
            SpawnTable();
            SpawnPeople();

            isInitialized = true;
        }

        private void InitializeCollections()
        {
            availablePeople = new List<PersonController>();
            queuedPeople = new Queue<PersonController>();
        }

        private void InitializePool()
        {
            personPool = new ObjectPool<PersonController>(Resources.personPrefab, transform);
        }

        private void SpawnTable()
        {
            GameObject table = Instantiate(Resources.tablePrefab, Vector3.zero, Quaternion.identity);
            tableTransform = table.transform;
            table.GetComponentInChildren<NavMeshSurface>().BuildNavMesh();
        }

        private void SpawnPeople()
        {
            for (int i = 0; i < Resources.totalPeople; i++)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                PersonController person = personPool.Get(spawnPosition, Quaternion.identity);

                if (person != null)
                {
                    float randomSpeed = Random.Range(Resources.minSpeed, Resources.maxSpeed);
                    person.Initialize(randomSpeed);
                    availablePeople.Add(person);
                }
            }
        }

        private Vector3 GetRandomSpawnPosition()
        {
            Vector2 randomCirclePoint = Random.insideUnitCircle * 4;
            return new Vector3(randomCirclePoint.x, 0, -Resources.spawnRadius + randomCirclePoint.y);
        }

        public void SelectRandomPerson()
        {
            if (availablePeople.Count == 0) return;

            int randomIndex = Random.Range(0, availablePeople.Count);
            PersonController selectedPerson = availablePeople[randomIndex];
            availablePeople.RemoveAt(randomIndex);

            queuedPeople.Enqueue(selectedPerson);

            if (currentInteractingPerson == null)
            {
                ProcessNextPersonInQueue();
            }
            else
            {
                UpdateQueuePositions();
            }
        }

        private void SubscribeToEvents()
        {
            EventManager.Instance.OnPersonReachedTable += HandlePersonReachedTable;
            EventManager.Instance.OnPersonFinishedInteraction += HandlePersonFinishedInteraction;
            EventManager.Instance.OnCheckQueuePosition += HandleQueuePositionCheck;
        }

        private void UnsubscribeFromEvents()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.OnPersonReachedTable -= HandlePersonReachedTable;
                EventManager.Instance.OnPersonFinishedInteraction -= HandlePersonFinishedInteraction;
                EventManager.Instance.OnCheckQueuePosition -= HandleQueuePositionCheck;
            }
        }

        /// <summary>
        /// Handles the event when a person reaches the table
        /// </summary>
        private void HandlePersonReachedTable(PersonController person)
        {
            if (person == currentInteractingPerson)
            {
                person.SetState(new InteractingState(person));
            }
        }

        private void HandlePersonFinishedInteraction(PersonController person)
        {
            if (currentInteractingPerson == person)
            {
                FinishPersonInteraction(person);
            }
        }

        private void FinishPersonInteraction(PersonController person)
        {
            if (currentInteractingPerson == person)
            {
                Vector3 rightSidePosition = GetRightSidePosition();
                person.SetState(new MovingState(person, rightSidePosition, true));
                currentInteractingPerson = null;

                if (queuedPeople.Count > 0)
                {
                    ProcessNextPersonInQueue();
                }
            }
        }

        private Vector3 GetRightSidePosition()
        {
            Vector2 randomOffset = Random.insideUnitCircle * Resources.rightSideSpawnRadius;
            return new Vector3(Resources.rightSideOffset, 0, randomOffset.y);
        }

        /// <summary>
        /// Processes the next person in queue to interact with the table
        /// </summary>
        private void ProcessNextPersonInQueue()
        {
            if (queuedPeople.Count == 0) return;

            PersonController nextPerson = queuedPeople.Dequeue();
            currentInteractingPerson = nextPerson;
            nextPerson.SetState(new MovingState(nextPerson, tableTransform.position));

            UpdateQueuePositions();
        }

        /// <summary>
        /// Updates the positions of all people in the queue
        /// </summary>
        public void UpdateQueuePositions()
        {
            List<PersonController> queueList = new List<PersonController>(queuedPeople);

            for (int i = 0; i < queueList.Count; i++)
            {
                PersonController person = queueList[i];
                Vector3 targetPosition = GetQueuePosition(i);
                person.SetState(new MovingState(person, targetPosition, false));
            }
        }

        /// <summary>
        /// Gets the position in queue based on index
        /// </summary>
        /// <returns>Queue position</returns>
        private Vector3 GetQueuePosition(int index)
        {
            return (tableTransform.position + Vector3.back) + Vector3.back * Resources.queueSpacing * (index + 1);
        }

        private void HandleQueuePositionCheck(PersonController person)
        {
            if (currentInteractingPerson == null && queuedPeople.Contains(person))
            {
                if (queuedPeople.Peek() == person)
                {
                    ProcessNextPersonInQueue();
                    return;
                }
            }

            UpdateQueuePositions();
        }


        public bool CanSelectNewPerson()
        {
            return availablePeople.Count > 0;
        }

        private void OnDisable()
        {
            if (isInitialized)
            {
                UnsubscribeFromEvents();
            }
        }

        private void OnDestroy()
        {
            if (tableTransform != null)
            {
                Destroy(tableTransform.gameObject);
            }

            personPool?.DestroyPool();
        }
    }
}