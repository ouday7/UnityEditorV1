using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
        public static ObjectPooler Instance; 
        [SerializeField] private List<EditorButtonBase> pooledObjects;
        [SerializeField] private EditorButtonBase objectToPool;
        [SerializeField] private int amountToPool;
        [SerializeField] private Transform pooling;

        private int _lastIndex=-1;
        private void Awake()
        {
            if(Instance==null) Instance = this;
            pooledObjects = new List<EditorButtonBase>();
            
            for (var i = 0; i < amountToPool; i++)
            {
                var tmp = Instantiate(objectToPool,pooling);
                tmp.gameObject.SetActive(false);
                pooledObjects.Add(tmp);
                _lastIndex++;
            }
        }
        public EditorButtonBase GetPooledObject()
        {
            if (pooledObjects.Count <= 0) return Instantiate(objectToPool, transform);
            var objectToGet = pooledObjects[_lastIndex];
            pooledObjects.RemoveAt(_lastIndex);
            _lastIndex--;
            return objectToGet;

        }
        public void ReturnObject(EditorButtonBase objectToDespawn)
        {
            objectToDespawn.SetParent(transform);
            objectToDespawn.gameObject.SetActive(false);
            pooledObjects.Add(objectToDespawn);
            _lastIndex++;
        }
}
