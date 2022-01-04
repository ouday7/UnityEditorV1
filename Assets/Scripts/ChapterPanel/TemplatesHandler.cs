using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TemplatesHandler : MonoBehaviour
{
    public static TemplatesHandler Instance;
    
    public List<TemplateData> templatesData;
    public List<TemplateCategory> categoriesData;
    private Dictionary<TemplateCategory, List<TemplateData>> _mapByCategory;
    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }
    public TemplateData GetTemplateData(int id)
    {
        return templatesData.FirstOrDefault(t => t.templateName == (TemplatesNames) id);
        
    }
    public TemplateData GetTemplateData(TemplatesNames tName)
    {
        return templatesData.FirstOrDefault(t => t.templateName == tName);
    }

    public TemplateCategory GetCategory(TemplatesCategories category)
    {
        return this.categoriesData.FirstOrDefault(c => c.category == category);
    }
}

 
