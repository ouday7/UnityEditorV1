using System.Collections;
using System.Collections.Generic;
using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTextSorting : QuizFieldBase
{
    [SerializeField] private Button deleteButton;
    [SerializeField] private InputField inputField;
    [SerializeField] private InputField sortingNumber;
    public override void Initialize()
    {
        deleteButton.onClick.AddListener(RemoveToggleField);
        inputField.onEndEdit.AddListener(InputValue);
        sortingNumber.onEndEdit.AddListener(InputSorting);
    }

    private void InputSorting(string arg0)
    {
        
    }

    private void RemoveToggleField()
    {
        Destroy(gameObject);
        EditManager.Instance.MaximiseMainContentHolder(EditManager.Instance.TemplateHolder.RectTransform
            .childCount-1);
    }

    public override void BindData(QuizFieldData inData)
    {
        _data = inData;
        inputField.text = _data.textA;
    }
    private void InputValue(string newValue) => _data.textA = newValue;
    
}
