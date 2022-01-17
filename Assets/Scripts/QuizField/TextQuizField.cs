using System.Collections;
using System.Collections.Generic;
using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;

public class TextQuizField : QuizFieldBase
{

    [SerializeField]  private InputField text1;
    [SerializeField] private Button buttonRemove;

    public override void Initialize()
    {
        text1.onEndEdit.AddListener(InputValue1);
        buttonRemove.onClick.AddListener(Remove);
    }

    public override void BindData(QuizFieldData inData)
    {
        _data = inData;
        text1.text = _data.textA;
    }

    private void Remove()
    {
        Destroy(gameObject);
    }


    private void InputValue1(string newValue) => _data.textA = newValue;

}
