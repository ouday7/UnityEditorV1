using System.Collections.Generic;
using UnityEngine;


public enum TemplatesNames
{
    TrueFalse,
}

public enum FieldTypes
{
    Text, Image, Toggle, TextText, ImageText, ImageImage, ToggleText, ImageToggle
}

[CreateAssetMenu(fileName = "TemplateData", menuName = "ScriptableObjects/TemplateData", order = 1)]
public class TemplateDataSO : ScriptableObject
{
    public string id;
    public TemplatesNames templateName;
    public string displayName;
    public Sprite icon;
    public int maxFields;
    public int minFields;
    public List<FieldTypes> templateFields;


    public void GetData(string name , string id , string desc , Sprite spr , GameObject obj , int max  , int min)
    {
        
        
        
    }
    
    



}
