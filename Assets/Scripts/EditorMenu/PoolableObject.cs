using UnityEngine;

public class PoolableObject : MonoBehaviour
{
   [SerializeField] protected ObjectToPoolType type;
       public ObjectToPoolType Type => type;
       
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