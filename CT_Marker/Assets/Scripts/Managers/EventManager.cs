using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public event Action OnPersonSelected;
    public event Action OnPersonReachedTable;
    public event Action OnPersonStartedInteraction;
    public event Action OnPersonFinishedInteraction;
    public event Action OnPersonJoinedQueue;
    public event Action OnQueuePositionUpdated;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
