using System;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

namespace EditorMenu
{
    public abstract class EditorButtonBase : PoolableObject
    {
        protected event Action OnSelectAction;
        protected static event Action OnUpdateIndex;
    
        [SerializeField] protected Button btn;
        [SerializeField] protected RtlText text;
        [SerializeField] protected Image background;
        [SerializeField] protected Button editBtn;
    
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color unselectedColor;

        public bool isInitialized;

        private void OnDestroy()
        {
            OnSelectAction = null;
            OnUpdateIndex = null;
        }

        public virtual void Initialize()
        {
            if(isInitialized) return;
            btn.onClick.AddListener(OnClickMainButton);
            OnUpdateIndex += UpdateDataIndex;
            isInitialized = true;
        }

        protected virtual void UpdateDataIndex() { }

        private void OnClickMainButton()
        {
            Select();
            OnSelectAction?.Invoke();
        }
        public void Select()
        { 
            background.color = selectedColor;
            editBtn.gameObject.SetActive(true);
            btn.interactable = false;
        }
        public void Unselect()
        {
            background.color = unselectedColor;
            editBtn.gameObject.SetActive(false);
            btn.interactable = true;
        }

        protected void InvokeUpdateChaptersOrder() => OnUpdateIndex?.Invoke();
    }
}
