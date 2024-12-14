using UnityEngine;
using System;
using Controller;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    public event Action<PersonController> OnPersonSelected;
    public event Action<PersonController> OnPersonReachedTable;
    public event Action<PersonController> OnPersonStartedInteraction;
    public event Action<PersonController> OnPersonFinishedInteraction;
    public event Action<PersonController> OnPersonJoinedQueue;
    public event Action<PersonController> OnQueuePositionUpdated;
    
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
    public void PersonSelected(PersonController person)
    {
        OnPersonSelected?.Invoke(person);
    }
    public void PersonReachedTable(PersonController person)
    {
        OnPersonReachedTable?.Invoke(person);
    }
    public void PersonStartedInteraction(PersonController person)
    {
        OnPersonStartedInteraction?.Invoke(person);
    }
    public void PersonFinishedInteraction(PersonController person)
    {
        OnPersonFinishedInteraction?.Invoke(person);
    }
    public void PersonJoinedQueue(PersonController person)
    {
        OnPersonJoinedQueue?.Invoke(person);
    }
    public void QueuePositionUpdated(PersonController person)
    {
        OnQueuePositionUpdated?.Invoke(person);
    }
    public void ClearAllEvents()
    {
        OnPersonSelected = null;
        OnPersonReachedTable = null;
        OnPersonStartedInteraction = null;
        OnPersonFinishedInteraction = null;
        OnPersonJoinedQueue = null;
        OnQueuePositionUpdated = null;
    }
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}