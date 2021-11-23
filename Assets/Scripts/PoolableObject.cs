using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    [SerializeField]
    public GameObject prefab = null;

    private void OnDisable()
    {
        if(prefab != null)
            ObjectPooler.Instance.ReleasePooledObject(this);
    }
}