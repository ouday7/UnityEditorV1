using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;

namespace QuizFields
{
    public class TextQuizField : QuizFieldBase
    {
        [SerializeField] private InputField _inputField;
        [SerializeField] private Button buttonRemove;
        private int _sortingNumber;
        
        public override void Initialize()
        {
            _inputField.onEndEdit.AddListener(InputValue);
            buttonRemove.onClick.AddListener(()=>Remove(this._data));
        }

        public override void BindData(QuizFieldData inData)
        {
            _data = inData;
            _inputField.text = _data.textA;
            _sortingNumber = _data.id;
        }
        private void Remove(QuizFieldData data)
        {
            if (EditManager.Instance.currentTemplate.minFields >=
                EditManager.Instance.QuizFieldsHolder.transform.childCount) return;
            EditManager.Instance.currentQuestionData.quizFields.Remove(data);
            Destroy(gameObject);
            EditManager.Instance.MaximiseMainContentHolders(EditManager.Instance.currentQuestionData.quizFields.Count);
        }

        private void InputValue(string newValue)
        {
            _data.textA = newValue;
            GameDataManager.instance.SaveToJson();
        }
    }
}
