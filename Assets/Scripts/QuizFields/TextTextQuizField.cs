using System.Collections;
using System.Collections.Generic;
using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;

public class TextTextQuizField : QuizFieldBase
{
    [SerializeField]  private InputField text1;
    [SerializeField]   private InputField text2;
    [SerializeField] private Button buttonRemove;

    public override void Initialize()
    {
        text1.onEndEdit.AddListener(InputValue1);
        text2.onEndEdit.AddListener(InputValue2);
        buttonRemove.onClick.AddListener(Remove);
    }

    public override void BindData(QuizFieldData inData)
    {
        _data = inData;
        text1.text = _data.textA;
        text2.text = _data.textTwo;
    }

    private void Remove()
    {
        Destroy(gameObject);
    }


    private void InputValue1(string newValue) => _data.textA = newValue;
    private void InputValue2(string newValue) => _data.textA = newValue;
}