using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TemplatesHandler : MonoBehaviour
{
    public static TemplatesHandler Instance;
    
    public List<TemplateData> templatesData;
    public List<TemplateCategory> categoriesData;
    private Dictionary<TemplateCategory, List<TemplateData>> _mapByCategory;
    public void Begin()
    {
        if (Instance != null) return;
        Instance = this;
    }
}

 
