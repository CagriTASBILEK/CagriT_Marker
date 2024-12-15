using Controller;
using UnityEngine;

[CreateAssetMenu(fileName = "GameResources", menuName = "Settings/Game Resources")]
public class GameResources : ScriptableObject
{
    [Header("Prefabs")]
    public PersonController personPrefab;
    public GameObject tablePrefab;

    [Header("Person Settings")]
    public float minSpeed = 2f;
    public float maxSpeed = 10f;
    public int totalPeople = 30;

    [Header("Spawn Settings")]
    public float spawnRadius = 30f;
    public float rightSideOffset = 30f;
    public float rightSideSpawnRadius = 5f;

    [Header("Queue Settings")]
    public float queueSpacing = 2f;

    [Header("Interaction Settings")]
    public float interactionDuration = 5f;
}