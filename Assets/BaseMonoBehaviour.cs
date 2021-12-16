using Sirenix.OdinInspector;
using UnityEngine;

namespace Envast
{
    public abstract class BaseMonoBehaviour: MonoBehaviour
    {
        public virtual void OnDestroy() => ReleaseReferences();

        
        [BoxGroup("References")]
        [Button("Release References", ButtonSizes.Medium)]
        [GUIColor("@Color.Lerp(Color.red, Color.magenta, Mathf.Sin((float)EditorApplication.timeSinceStartup))")]
        public abstract void ReleaseReferences();
    }
}