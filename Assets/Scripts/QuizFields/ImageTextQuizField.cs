using System.Collections;
using System.Collections.Generic;
using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;

public class ImageTextQuizField : QuizFieldBase
{
    [SerializeField] private InputField text1;
    [SerializeField] private Button _image;
    [SerializeField] private Button buttonRemove;

    public override void Initialize()
    {
        text1.onEndEdit.AddListener(InputValue1);
        //_image.onClick.AddListener(SetImage);
        buttonRemove.onClick.AddListener(Remove);
    }

    public override void BindData(QuizFieldData inData)
    {
        _data = inData;
        text1.text = _data.textA;
        _image.image.sprite = _data.imageOne;
    }

    private void Remove()
    {
        Destroy(gameObject);
    }

    private void InputValue1(string newValue)
    {
        _data.textA = newValue;
        GameDataManager.instance.SaveToJson();
    } 
    private void SetImage(Image newImage)
    {
        _data.imageOne = newImage.sprite;
        GameDataManager.instance.SaveToJson();
    }
}