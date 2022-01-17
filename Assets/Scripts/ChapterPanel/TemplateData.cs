using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public enum TemplatesNames
{
    TrueOrFalse,
    SelectPhrases,
    SortingPhrase,
    SelectPictures,
    SortingPictures,
    ConnectImageToImage,
    ConnectTextToText,
    ConnectImageToText,
    CompleteParagraphByWriting,
    CompleteParagraphBySentence,
    CorrectMistake,
    InstallOnPicture,
    WriteOnPicture,
    SelectNumber,
    SelectSentence,
    InstallPhrase,
    InstallInputPhrase,
    CorrectWrongWithWord,
    Writing
}

public enum FieldTypes
{
    Text,
    Image,
    Toggle,
    TextText,
    ImageText,
    ImageImage,
    ToggleText,
    ImageToggle
}

[CreateAssetMenu(fileName = "TemplateData", menuName = "ScriptableObjects/TemplateDataInformation", order = 1)]
public class TemplateData : ScriptableObject
{
    public static int nextId;
    
    [ReadOnly] public int id;
    public TemplatesNames templateName;
    public TemplatesCategories category;
    public Sprite icon;
    public int maxFields;
    public int minFields;
    public List<FieldTypes> templateFields;

    public FieldTypes GetQuizFieldType(int i)
    {
        if (templateFields.IsNullOrEmpty())
            throw new Exception(
                $"Quiz Fields is Empty ({templateName})");
        if (i >= templateFields.Count) return templateFields[templateFields.Count - 1];
        return templateFields[i];
    }

    [Button] private void GenerateId()
    {
        nextId++;
        id = nextId;
    }
}