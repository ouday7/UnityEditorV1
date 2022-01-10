using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TemplatesHandler : EntryPointSystemBase
{
    public static TemplatesHandler Instance;
    
    public List<TemplateData> templatesData;
    public List<TemplateCategory> categoriesData;
    private Dictionary<TemplateCategory, List<TemplateData>> _mapByCategory;
    public override void Begin()
    {
        if (Instance != null) return;
        Instance = this;
    }
}

 
