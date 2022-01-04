using System.Collections.Generic;
//using Envast.MenuEditor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Envast.Layouts
{
    [RequireComponent(typeof(RectTransform))]
    public class CustomSizeFitter : MonoBehaviour
    {
        public bool subscribeToEditorMenuEvent;
        public bool autoUpdate;
        public float extraSpacing;
        public float spacing;
        public List<CustomSizeFitter> allParent;

        private RectTransform _rt;
        private float _startSize;
    
        private RectTransform RectTransform
        {
            get
            {
                if (_rt == null) _rt = GetComponent<RectTransform>();
                return _rt;
            }
        }
        private void Start()
        {
         //   if (subscribeToEditorMenuEvent) EditorMenu.OnSelectQuestion += UpdateSize;
            _startSize = RectTransform.sizeDelta.y;
            if(autoUpdate) UpdateSize();
        }

        public void UpdateSize(float delay)
        {
            Invoke(nameof(UpdateSize), delay);
        }
       [Button("Update Size", ButtonSizes.Medium)]
        public void UpdateSize()
        {
           
            Debug.Log("Clicked !");
            RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, _startSize);
            var totalWidth = 0f;
            if (RectTransform.childCount > 0)
            {
                foreach (RectTransform child in RectTransform)
                {
                    if(!child.gameObject.activeSelf) continue;
                    totalWidth += child.rect.height + spacing;
                }
                totalWidth += extraSpacing;
                RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, totalWidth);
            }

            if (allParent == null || allParent.Count <= 0) return;
            for (var i = 0; i < allParent.Count; i++)
            {
                var parent = allParent[i];
                parent.UpdateSize();
            }
        }
    }
}