using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;

namespace QuizFields
{
    public class ToggleTextMultipleQuizField : QuizFieldBase
    {
        //[SerializeField] private ToggleField manyChoicesToggle;
        [SerializeField] private Toggle myToggle;
        [SerializeField] private InputField inputFiled;
        [SerializeField] private Button buttonRemove;

        private static Toggle _lastSelected;

        public override void Initialize()
        {
            buttonRemove.onClick.AddListener(()=>Remove(this._data));
            myToggle.onValueChanged.AddListener(OnClickToggle);
            inputFiled.onEndEdit.AddListener(InputValue);
        }

        private void OnClickToggle(bool newValue)
        {
            ToggleValue(newValue);
        }

        public override void BindData(QuizFieldData inData)
        {
            _data = inData;
            inputFiled.text = _data.textA;
            myToggle.isOn = _data.toggleA;
        }

        private void Remove(QuizFieldData obj)
        {
            if (EditManager.Instance.currentTemplate.minFields >=
                EditManager.Instance.QuizFieldsHolder.transform.childCount) return;
            Destroy(gameObject);
            EditManager.Instance.currentQuestionData.quizFields.Remove(obj);
            EditManager.Instance.MaximiseMainContentHolders(EditManager.Instance.currentQuestionData.quizFields.Count);
        }

        private void ToggleValue(bool newValue)
        {
            _data.toggleA = newValue;
        }
        private void InputValue(string newValue)
        {
            _data.textA = newValue;
        } 
    
    }
}