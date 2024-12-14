using UnityEngine;
using System.Collections.Generic;

public class PersonManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform tableTransform;
    
    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    
    public void Initialize()
    {
        if (tableTransform == null)
        {
            Debug.LogError("Table Transform reference is missing in PersonManager!");
            return;
        }
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

    private void HandlePersonReachedTable()
    {
    }

    private void HandlePersonFinishedInteraction()
    {
    }
    
}