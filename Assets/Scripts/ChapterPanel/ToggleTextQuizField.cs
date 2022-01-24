using ChapterPanel;
using QuizFields;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleTextQuizField : QuizFieldBase
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
        if (_lastSelected != null)
        {
            _lastSelected.isOn = false;
            ToggleValue(newValue);
            _lastSelected = myToggle;
            return;
        }

        ToggleValue(newValue);
        _lastSelected = myToggle;
    }

    public override void BindData(QuizFieldData inData)
    {
        _data = inData;
        inputFiled.text = _data.textA;
        myToggle.isOn = _data.toggleA;
    }

    private void Remove(QuizFieldData obj)
    {
        Destroy(gameObject);
        EditManager.Instance.currentQuestionData.quizFields.Remove(obj);
        EditManager.Instance.MaximiseMainContentHolder
        (EditManager.Instance.QuizFieldsHolder.RectTransform.childCount-1);
    }

    private void ToggleValue(bool newValue)
    {
        _data.toggleA = newValue;
    }
    private void InputValue(string newValue) => _data.textA = newValue;
    
}