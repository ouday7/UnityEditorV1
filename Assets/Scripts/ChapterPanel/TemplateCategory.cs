using UnityEngine;

public enum TemplatesCategories
{
    Sorting,
    Connecting,
    Correcting,
    Tables,
    Choosing,
    Installing,
    Calculation,
    CompleteFields
}

[CreateAssetMenu(menuName = "Categories", fileName = "CategoryData")]
public class TemplateCategory : ScriptableObject
{
    public int id;
    public Sprite icon;
    public TemplatesCategories category;
    
}