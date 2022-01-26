using System;
using DG.Tweening;
using EditorMenu;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

namespace MainEditor
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
        [SerializeField] private Color labelSelectedColor;
        [SerializeField] private Color labelUnselectedColor;
    
        [SerializeField] private float duration = .2f;
        [SerializeField] private Ease ease = Ease.Linear;
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
            AnimateButton();
            OnSelectAction?.Invoke();
        }
        public void Select()
        {
            background.color = selectedColor;
            text.color = labelSelectedColor;
            editBtn.gameObject.SetActive(true);
            btn.interactable = false;
        }
        public void Unselect()
        {
            background.color = unselectedColor;
            text.color = labelUnselectedColor;
            editBtn.gameObject.SetActive(false);
            btn.interactable = true;
        }

        protected void InvokeUpdateChaptersOrder() => OnUpdateIndex?.Invoke();

        private void AnimateButton()
        {
            Transform.DOScale(0.8f, duration).SetEase(ease).OnComplete( () =>
            {
                Transform.DOScale(1f, duration).SetEase(ease);
            });
        }
    }
}
