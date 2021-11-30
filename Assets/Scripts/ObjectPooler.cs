using System;
using System.Collections.Generic;
using UnityEngine;
public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;
    public List<PrePooledObjects> prePooledObjects;
    private Dictionary<string, Queue<GameObject>> dict = null;
    [Serializable]
    public struct PrePooledObjects
    {
        public GameObject gameObject;
        public int count;
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            dict = new Dictionary<string, Queue<GameObject>>();
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        List<GameObject> pooledObjects = new List<GameObject>();
        foreach(PrePooledObjects prePoolObj in prePooledObjects)
        {
            for(int i = 0; i<prePoolObj.count; i++)
            {
                pooledObjects.Add(GetPooledObject(prePoolObj.gameObject));
            }
        }
        foreach(GameObject go in pooledObjects)
        {
            go.transform.SetParent(transform);
            go.SetActive(false);
        }
    }
    public GameObject GetPooledObject(GameObject go)
    {
        if (!dict.ContainsKey(go.name))
        {
            dict.Add(go.name, new Queue<GameObject>());
        }

        if (dict[go.name].Count > 0)
        {
            return dict[go.name].Dequeue();
        }
        var newGo = Instantiate(go);
        var po = newGo.GetComponent<PoolableObject>();
        if( po == null)
        {
            po = newGo.AddComponent<PoolableObject>();
        }
        po.prefab = go;
        return newGo;
    }
    public void ReleasePooledObject(PoolableObject po)
    {
        if (!dict.ContainsKey(po.prefab.name))
        {
            dict.Add(po.prefab.name, new Queue<GameObject>());
        }
        dict[po.prefab.name].Enqueue(po.gameObject);
    }
}