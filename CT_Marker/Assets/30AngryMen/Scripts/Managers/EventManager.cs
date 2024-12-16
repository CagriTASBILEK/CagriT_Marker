using System;
using Controller;
using UnityEngine;

namespace Managers
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; }
        public event Action<PersonController> OnPersonReachedTable;
        public event Action<PersonController> OnPersonFinishedInteraction;
        public event Action<PersonController> OnCheckQueuePosition;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PersonReachedTable(PersonController person)
        {
            OnPersonReachedTable?.Invoke(person);
        }

        public void PersonFinishedInteraction(PersonController person)
        {
            OnPersonFinishedInteraction?.Invoke(person);
        }

        public void CheckQueuePosition(PersonController person)
        {
            OnCheckQueuePosition?.Invoke(person);
        }

        public void ClearAllEvents()
        {
            OnPersonReachedTable = null;
            OnPersonFinishedInteraction = null;
            OnCheckQueuePosition = null;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}