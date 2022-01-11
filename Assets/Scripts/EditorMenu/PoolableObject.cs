using UnityEngine;

namespace EditorMenu
{
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
        //todo: for debugging -> remove later
        public virtual void Spawn()
        {
            
        }

        public virtual void DeSpawn()
        {
            
        }
    }
}