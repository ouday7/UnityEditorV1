using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EditorMenu
{
    public enum ObjectToPoolType
    {
        Level, Subject, Chapter, Toggle,Exercise,Question
    }
    public class PoolSystem : EntryPointSystemBase
    {
        [Serializable] private struct PoolObject
        {
            public ObjectToPoolType type;
            public PoolableObject objectReference;
            public int amount;
        }
        [SerializeField] private List<PoolObject> poolObjects;
        [SerializeField] private List<PoolableObject> currentPoolObjects;
    
        public static PoolSystem instance;
        
        public override void Begin()
        {
            if (instance != null) return;
            instance = this;
            InitPool();
        }

        public void Initialize()
        {
            if (instance != null) return;
            instance = this;
            InitPool();
        }
    
        private void InitPool()
        {
            foreach (var obj in poolObjects)
            {
                for (var i = 0; i <obj.amount; i++)
                {
                    var newObj = Instantiate(obj.objectReference, transform, true);
                    newObj.gameObject.SetActive(false);
                    currentPoolObjects.Add(newObj);
                }
            }
        }
        public T Spawn<T>(ObjectToPoolType type) where T : PoolableObject
        {
            var poolItem = currentPoolObjects.FirstOrDefault(t => t.Type == type);
            if (poolItem != null)
            {
                var obj = poolItem.GetComponent<T>();
                currentPoolObjects.Remove(poolItem);
                obj.gameObject.SetActive(true);
                obj.Spawn();
                return obj;
            }
            GenerateElement(type);
            return Spawn<T>(type);
        }
        public void DeSpawn(Transform objectToDeSpawn)
        {
            objectToDeSpawn.GetComponent<PoolableObject>().DeSpawn();
            objectToDeSpawn.SetParent(transform);
            objectToDeSpawn.gameObject.SetActive(false);
            currentPoolObjects.Add(objectToDeSpawn.GetComponent<PoolableObject>());
        }
    
        private void GenerateElement(ObjectToPoolType type) //create an element if the pool is empty
        {
            var poolItem = poolObjects.FirstOrDefault(poolObject => poolObject.type == type);
            var item = Instantiate(poolItem.objectReference, transform);
            item.gameObject.SetActive(false);
            currentPoolObjects.Add(item);
        }
    }
}