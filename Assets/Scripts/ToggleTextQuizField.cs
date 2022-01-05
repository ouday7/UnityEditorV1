using System;
using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTextQuizField : QuizFieldBase
{
    [SerializeField] private Toggle myToggle;
    [SerializeField] private InputField inputFiled;
    [SerializeField] private Button buttonRemove;


    public void Start()
    {
        buttonRemove.onClick.AddListener(Remove);
        myToggle.onValueChanged.AddListener(delegate { ToggleValue(myToggle); });
        inputFiled.onEndEdit.AddListener(delegate { InputValue(inputFiled); });
    }

    public override void Initialize()
    {   
        buttonRemove.onClick.AddListener(Remove);
        myToggle.onValueChanged.AddListener(delegate { ToggleValue(myToggle); });
        inputFiled.onEndEdit.AddListener(delegate { InputValue(inputFiled); });

    }

    public override void BindData(QuizFieldData inData)
    {
        inData.textOne = inputFiled.text;
        inData.toggleOne = myToggle.isOn;
        Debug.Log(inData.textOne);
        Debug.Log(inData.toggleOne);
        
    }


    private void Remove()
    {
        Destroy(gameObject);
    }
    private bool ToggleValue(Toggle tog)
    {

        return (tog.isOn);
    }
    private string InputValue(InputField inputField)
    {
        return (inputField.text);
    }
}