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
        
        if (personManager == null)
        {
            return;
        }
        if (gameResources == null)
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
            personManager.SelectRandomPerson();
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