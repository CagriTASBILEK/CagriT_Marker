using System.Collections.Generic;
using Controller;
using UnityEngine;

namespace Managers
{
    public class PersonManager : MonoBehaviour
    {
        [Header("References")]
        private Transform tableTransform;

        private ObjectPool<PersonController> personPool;
        [SerializeField]private List<PersonController> availablePeople;
        [SerializeField]private Queue<PersonController> queuedPeople;
        [SerializeField]private PersonController currentInteractingPerson;

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
        }
        private void SubscribeToEvents()
        {
            EventManager.Instance.OnPersonReachedTable += HandlePersonReachedTable;
            EventManager.Instance.OnPersonFinishedInteraction += HandlePersonFinishedInteraction;
        }
        private void UnsubscribeFromEvents()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.OnPersonReachedTable -= HandlePersonReachedTable;
                EventManager.Instance.OnPersonFinishedInteraction -= HandlePersonFinishedInteraction;
            }
        }
        private void HandlePersonReachedTable(PersonController person)
        {
            if (currentInteractingPerson == null)
            {
                StartPersonInteraction(person);
            }
            else
            {
                AddPersonToQueue(person);
            }
        }
        private void HandlePersonFinishedInteraction(PersonController person)
        {
            if (currentInteractingPerson == person)
            {
                FinishPersonInteraction(person);
            }
        }
        private void StartPersonInteraction(PersonController person)
        {
            currentInteractingPerson = person;
        }
        private void AddPersonToQueue(PersonController person)
        {
            queuedPeople.Enqueue(person);
        }
        private void FinishPersonInteraction(PersonController person)
        {
            currentInteractingPerson = null;
        
            if (queuedPeople.Count > 0)
            {
                ProcessNextPersonInQueue();
            }
        }
        private void ProcessNextPersonInQueue()
        {
            PersonController nextPerson = queuedPeople.Dequeue();
            currentInteractingPerson = nextPerson;
        }
        private void OnDisable()
        {
            if(isInitialized)
            {
                UnsubscribeFromEvents();
            }
        }
        private void OnDestroy()
        {
            if(tableTransform != null)
            {
                Destroy(tableTransform.gameObject);
            }
            personPool?.DestroyPool();
        }
        
    }
}