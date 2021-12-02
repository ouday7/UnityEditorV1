using UnityEngine;

public class PoolableObject : MonoBehaviour
{
   [SerializeField] protected ObjectToPoolType type;
       public ObjectToPoolType Type => type;
       
       public Transform t;
       public Transform Transform
       {
           get
           {
               if (t == null) t = transform;
               return t;
           }
       }
}