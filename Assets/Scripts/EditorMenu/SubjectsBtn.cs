using System;
using ChapterPanel;

namespace EditorMenu
{
    public class SubjectsBtn : EditorButtonBase
    {
        public static event Action<SubjectsBtn> OnSelectSubjectButton;
    
        private SubjectData _data;
        private bool _isInitialized;
        public SubjectData Data => _data;
    
        public override void Initialize()
        {
            if(_isInitialized) return;
            base.Initialize();
            editBtn.onClick.AddListener(() => UIManager.instance.SubjectEdit(this));
            OnSelectAction += SubjectButtonSelected;
        }

        public void BindData(SubjectData subjectData)
        {
            _data = subjectData;
            text.text = _data.name;
            transform.SetSiblingIndex(subjectData.order-1);
        }
    
        public void SubjectButtonSelected()
        {
            OnSelectSubjectButton?.Invoke(this);
        }
    
        public void UpdateData(string newName, string newOrderText)
        {
            if (newName.Length == 0)
            {
                newName = _data.name;
            }
            if (newOrderText.Length == 0)
            {
                newOrderText = _data.order.ToString();
            }
            _data.name = newName;
            if (int.TryParse(newOrderText, out var newOrder)) _data.order = newOrder;
            BindData(_data);
            GameDataManager.Instance.SaveToJson();
        }
    }
}
