using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameResources gameResources;
    [SerializeField] private PersonManager personManager;
    
    private float lastSelectionTime;
    private const float SELECTION_DELAY = 1.5f;

    public GameResources Resources => gameResources;
    
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

    private void Start()
    {
        personManager.Initialize();
        
        if (personManager == null || gameResources == null)
        {
            return;
        }
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time - lastSelectionTime >= SELECTION_DELAY)
            {
                if (personManager.CanSelectNewPerson())
                {
                    personManager.SelectRandomPerson();
                    lastSelectionTime = Time.time;
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}