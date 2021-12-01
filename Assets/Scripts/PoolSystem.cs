using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum PoolType
{
    Level, Subject, Chapter, ToggleSelect
}

public class PoolSystem : MonoBehaviour
{
    [Serializable] private struct PoolObjectData
    {
        public PoolType type;
        public PoolObjectBase objectReference;
        public int preloadedAmount;
    }
    
    public static PoolSystem Instance;
    
    [SerializeField] private List<PoolObjectData> poolObjects;
    [SerializeField] private List<PoolObjectBase> currentPool;

    public void Initialize()
    {
        if(Instance != null) return;
        Instance = this;
        PopulatePool();
    }

    private void PopulatePool()
    {
        for (var i = 0; i < poolObjects.Count; i++)
        {
            for (var j = 0; j < poolObjects[i].preloadedAmount; j++)
            {
                GenerateElement(poolObjects[i].type);
            }
        }
    }

    public T Spawn<T>(PoolType inType) where T : Component
    {
        var poolItem = currentPool.FirstOrDefault(pI => pI.Type == inType);
        if (poolItem != null)
        {
            var obj = poolItem.GetComponent<T>();
            if (obj == null)
            {
                Debug.Log($"//. Object is Null {inType}");
            }
            currentPool.Remove(poolItem);
            obj.gameObject.SetActive(true);
            return obj;
        }
        GenerateElement(inType);
        return Spawn<T>(inType);
        //- Map example
        /*
        if (poolMap.ContainsKey(inType))
        {
            var item = poolMap[inType].Dequeue();
            if (item != null) return item.GetComponent<T>();
            GenerateElement(inType);
            return Spawn<T>(inType);
        }
        else
        {
            throw new Exception($"Pool doesn't contain Element with key: {inType}");
        }
        */
    }

    public Transform Spawn(PoolType inType)
    {
        var poolItem = currentPool.FirstOrDefault(pI => pI.Type == inType);
        if (poolItem != null) return poolItem.Transform;
        GenerateElement(inType);
        return Spawn(inType);
    }

    public void DeSpawn(PoolObjectBase objectToDeSpawn)
    {
        objectToDeSpawn.Transform.SetParent(transform);
        objectToDeSpawn.gameObject.SetActive(false);
        currentPool.Add(objectToDeSpawn);
    }

    private void GenerateElement(PoolType inType) //create element if pool is empty
    {
        var poolItemData = poolObjects.FirstOrDefault(poolObject => poolObject.type == inType);
        var item = Instantiate(poolItemData.objectReference, transform);
        item.gameObject.SetActive(false);
        currentPool.Add(item);
    }

    private List<ChaptersBtn> spawnedChapters = new List<ChaptersBtn>();
}