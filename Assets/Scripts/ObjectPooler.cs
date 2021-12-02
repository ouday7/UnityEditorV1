using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public enum ObjectToPoolType
{
    Level, Subject, Chapter, Toggle
}
public class ObjectPooler : MonoBehaviour
{
    [Serializable] private struct PoolObject
    {
        public ObjectToPoolType type;
        public PoolableObject objectReference;
        public int _amount;
    }
    [SerializeField] private List<PoolObject> poolObjects;
    [SerializeField] private List<PoolableObject> currentPoolObjects;
    
 
    public static ObjectPooler Instance;

    public void Initialize()
    {
        if (Instance != null) return;
        Instance = this;
    }
    public void Begin()
    {
        InitPool();
    }
    private void InitPool()
    {
        foreach (var obj in poolObjects)
        {
            for (var i = 0; i <obj._amount; i++)
            {
                var newObj = Instantiate(obj.objectReference, transform, true);
                newObj.gameObject.SetActive(false);
                currentPoolObjects.Add(newObj);
            }
        }
    }
    public T Spawn<T>(ObjectToPoolType type) where T:Component
    {
        Debug.Log($"Spawn");

        var poolItem = currentPoolObjects.FirstOrDefault(t => t.Type == type);
        if (poolItem != null)
        {
            var obj = poolItem.GetComponent<T>();
            currentPoolObjects.Remove(poolItem);
            obj.gameObject.SetActive(true);
            return obj;
        }
        GenerateElement(type);
        return Spawn<T>(type);
    }
    public void DeSpawn(Transform objectToDeSpawn)
    {
        Debug.Log($"Despawn");
        objectToDeSpawn.SetParent(transform);
        objectToDeSpawn.gameObject.SetActive(false);
        currentPoolObjects.Add(objectToDeSpawn.GetComponent<PoolableObject>());
    }
    
    private void GenerateElement(ObjectToPoolType type)
        //create an element if the pool is empty
    {
        var poolItem = poolObjects.FirstOrDefault(poolObject => poolObject.type == type);
        var item = Instantiate(poolItem.objectReference, transform);
        item.gameObject.SetActive(false);
        currentPoolObjects.Add(item);
    }
}