using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component, IPoolable
{
    private readonly T prefab;
    private readonly Transform container;
    private readonly Stack<T> availableObjects;
    private readonly HashSet<T> inUseObjects;
    
    private readonly int maxPoolSize = 30; 
    private bool isInitialized;

    public ObjectPool(T prefab, Transform container)
    {
        this.prefab = prefab;
        this.container = container;
        
        availableObjects = new Stack<T>(maxPoolSize);
        inUseObjects = new HashSet<T>();
        
        Initialize();
    }

    private void Initialize()
    {
        if (isInitialized) return;
        
        for (int i = 0; i < maxPoolSize; i++)
        {
            CreateNewObject();
        }
        isInitialized = true;
    }

    private T CreateNewObject()
    {
        T obj = GameObject.Instantiate(prefab, container);
        obj.gameObject.SetActive(false);
        availableObjects.Push(obj);
        return obj;
    }

    public T Get(Vector3 position, Quaternion rotation)
    {
        if (availableObjects.Count == 0) return null;

        T obj = availableObjects.Pop();
        Transform objTransform = obj.transform;
        objTransform.SetPositionAndRotation(position, rotation);
        obj.gameObject.SetActive(true);
        obj.OnSpawn();
        inUseObjects.Add(obj);
        return obj;
    }

    public void Return(T obj)
    {
        if (obj != null && inUseObjects.Remove(obj))
        {
            obj.OnDespawn();
            obj.gameObject.SetActive(false);
            availableObjects.Push(obj);
        }
    }

    public void DestroyPool()
    {
        foreach (T obj in availableObjects)
        {
            if (obj != null) GameObject.Destroy(obj.gameObject);
        }
        foreach (T obj in inUseObjects)
        {
            if (obj != null) GameObject.Destroy(obj.gameObject);
        }

        availableObjects.Clear();
        inUseObjects.Clear();
        isInitialized = false;
    }
}