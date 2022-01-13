using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TemplatesHandler : EntryPointSystemBase
{
    public static TemplatesHandler Instance;
    
    [System.Serializable]
    public struct TemplateCatalog
    {
        public TemplatesNames type;
        public GameObject prefab;
    }
    
    
    public List<TemplateData> templatesData;
    public List<TemplateCategory> categoriesData;
    [SerializeField]  private List<TemplateCatalog> templatesCatalog;
    
    private Dictionary<TemplateCategory, List<TemplateData>> _mapByCategory;
    
    public override void Begin()
    {
        if (Instance != null) return;
        Instance = this;
    }
}

 
