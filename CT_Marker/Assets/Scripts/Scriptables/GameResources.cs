using UnityEngine;

[CreateAssetMenu(fileName = "GameResources", menuName = "Settings/Game Resources")]
public class GameResources : ScriptableObject
{
    [Header("Prefabs")]
    public GameObject tablePrefab;

    [Header("Person Settings")]
    public float minSpeed = 2f;
    public float maxSpeed = 10f;
    public float rotationSpeed = 5f;
    public float stoppingDistance = 0.1f;
    public int totalPeople = 30;

    [Header("Spawn Settings")]
    public float spawnRadius = 30f;
    public float rightSideOffset = 30f;
    public float rightSideSpawnRadius = 5f;

    [Header("Queue Settings")]
    public float queueStartOffset = 2f;
    public float queueSpacing = 2f;

    [Header("Interaction Settings")]
    public float interactionDuration = 5f;
}