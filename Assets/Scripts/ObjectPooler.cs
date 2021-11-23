using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;
    [SerializeField] private List<EditorButtonBase> pooledObjects;
    [SerializeField] private EditorButtonBase objectToPool;
    [SerializeField] private int amountToPool;

    private Transform _t;

    private int _lastIndex = -1;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;

        _t = transform;

        pooledObjects = new List<EditorButtonBase>();
        for (var i = 0; i < amountToPool; i++)
        {
            var tmp = Instantiate(objectToPool, _t);
            tmp.gameObject.SetActive(false);
            pooledObjects.Add(tmp);
            _lastIndex++;
        }
    }

    public EditorButtonBase GetPooledObject()
    {
        if (pooledObjects.Count <= 0) return Instantiate(objectToPool, this.transform);
        var objectToGet = pooledObjects[_lastIndex];
        pooledObjects.RemoveAt(_lastIndex);
        _lastIndex--;
        return objectToGet;
    }

    public void ReturnObject(EditorButtonBase objectToDeSpawn)
    {
        objectToDeSpawn.transform.SetParent(this.transform);
        objectToDeSpawn.gameObject.SetActive(false);
        pooledObjects.Add(objectToDeSpawn);
        _lastIndex++;
    }
}