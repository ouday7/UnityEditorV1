using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTextQuizField : QuizFieldBase
{
    [SerializeField] private Toggle myToggle;
    [SerializeField] private InputField inputFiled;
    [SerializeField] private Button buttonRemove;
   

    public override void Initialize()
    {
        buttonRemove.onClick.AddListener(Remove);
        myToggle.onValueChanged.AddListener(ToggleValue);
        inputFiled.onEndEdit.AddListener(InputValue);
        
    }

    public override void BindData(QuizFieldData inData)
    {
        _data = inData;
        inputFiled.text = _data.textA;
        myToggle.isOn = _data.toggleA;
     
    }

    private void Remove()
    {
        // if (EditManager.Instance.TemplateHolder.childCount==EditManager.Instance.currentTemplate.minFields)
        // {
        //     buttonRemove.interactable = false;
        // }
        
            Destroy(gameObject);
        
    }

    private void ToggleValue(bool newValue) => _data.toggleA = newValue;
    private void InputValue(string newValue) => _data.textA = newValue;
}