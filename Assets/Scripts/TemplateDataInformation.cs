using System;
using System.Collections.Generic;
using UnityEngine;
public enum TemplatesNames
{
    SortingPhrases, 
    SelectPhrases, 
    TrueOrFalse, 
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
public class TemplateDataInformation : ScriptableObject
{
    public int id;
    public TemplatesNames templateName;
    public TemplatesCategories category;
    public Sprite icon;
    public int maxFields;
    public int minFields;
    public List<FieldTypes> templateFields;
    public void GenerateId()
    {
        this.id = (int) templateName;
    }
    public void GetName()
    {
    }
    
}
  
    
   

