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
        public static PoolSystem instance;
        [Serializable] private struct PoolObject
        {
            public ObjectToPoolType type;
            public PoolableObject objectReference;
            public int amount;
        }
        [SerializeField] private List<PoolObject> poolObjects;
        [SerializeField] private List<PoolableObject> currentPoolObjects;
        
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
                return obj;
            }
            GenerateExtraElement(type);
            return Spawn<T>(type);
        }
        public void DeSpawn(Transform objectToDeSpawn)
        {
            objectToDeSpawn.SetParent(transform);
            objectToDeSpawn.gameObject.SetActive(false);
            currentPoolObjects.Add(objectToDeSpawn.GetComponent<PoolableObject>());
        }
    
        //create an extra element if the pool is empty
        private void GenerateExtraElement(ObjectToPoolType type) 
        {
            var poolItem = poolObjects.FirstOrDefault(poolObject => poolObject.type == type);
            var item = Instantiate(poolItem.objectReference, transform);
            item.gameObject.SetActive(false);
            currentPoolObjects.Add(item);
        }
    }
}