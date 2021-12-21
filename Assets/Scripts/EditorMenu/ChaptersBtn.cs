using System;
using UnityEngine;
using UnityEngine.UI;

namespace EditorMenu
{
    public class ChaptersBtn : EditorButtonBase
    {
        public static event Action<ChaptersBtn> OnSelectChaptersButton;

        private ChapterData _data;
        private EditorButtonsManager _parent;
        [SerializeField] public Button configBtn;

        public ChapterData Data => _data;

        private void OnDestroy() => OnSelectChaptersButton = null;

        public override void Initialize()
        {
            if (_isInitialized) return;
            base.Initialize();

            editBtn.onClick.AddListener(() => UIManager.instance.ChapterEdit(this));
            OnSelectAction += OnSelectChapterButton;
        }

        public void BindData(ChapterData chapterData, EditorButtonsManager parent)
        {
            _parent = parent;
            _data = chapterData;
            text.text = _data.name;
            Transform.SetSiblingIndex(chapterData.order - 1);
        }

        public void UpdateName(string newName)
        {
            if (newName.Length == 0 || newName == _data.name) return;
            _data.name = newName;
            text.text = _data.name;
        }

        public void UpdateOrder(string newOrder)
        {
            if (newOrder.Length == 0) return;
            if (!int.TryParse(newOrder, out var newOrderValue)) return;
            _data.order = newOrderValue;
            Transform.SetSiblingIndex(newOrderValue);
            InvokeUpdateChaptersOrder();
        }

        protected override void UpdateDataIndex()
        {
            if (!gameObject.activeInHierarchy) return;
            _data.order = Transform.GetSiblingIndex() + 1;
        }

        private void OnSelectChapterButton() => OnSelectChaptersButton?.Invoke(this);

        public SubjectData GetSelectedSubjectData() => _parent.SelectedSubject;
    }
}