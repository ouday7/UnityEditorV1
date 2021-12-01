using UnityEngine;
public abstract class PoolObjectBase : MonoBehaviour
{
    [SerializeField] protected PoolType type;
    public PoolType Type => type;
    
    private Transform _t;
    public Transform Transform
    {
        get
        {
            if (_t == null) _t = transform;
            return _t;
        }
    }
}