using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;

namespace QuizFields
{
    public class TextTextQuizField : QuizFieldBase
    {
        [SerializeField] private InputField text1;
        [SerializeField] private InputField text2;
        [SerializeField] private Button buttonRemove;

        public override void Initialize()
        {
            text1.onEndEdit.AddListener(InputValue1);
            text2.onEndEdit.AddListener(InputValue2);
            buttonRemove.onClick.AddListener(()=>Remove(_data));
        }

        public override void BindData(QuizFieldData inData)
        {
            _data = inData;
            text1.text = _data.textA;
            text2.text = _data.textTwo;
        }

        private void Remove(QuizFieldData data)
        {
            if (EditManager.Instance.currentTemplate.minFields >=
                EditManager.Instance.QuizFieldsHolder.transform.childCount) return;
            EditManager.Instance.currentQuestionData.quizFields.Remove(data);
            transform.SetParent(null);
            Destroy(gameObject);
            EditManager.Instance.UpdateHolderSize();
            EditManager.Instance.UpdateMainHolderByOneItem(false);
        }


        private void InputValue1(string newValue)
        {
            Debug.Log($"test");
            _data.textA = newValue;
            GameDataManager.instance.SaveToJson();
        }

        private void InputValue2(string newValue)
        {
            Debug.Log($"test");
            _data.textTwo = newValue;
            GameDataManager.instance.SaveToJson();
        }
    
    }
}