using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]  public struct TemplateCatalog
{
    public TemplatesNames type;
    public GameObject prefab;
}

public class TemplatesHandler : MonoBehaviour
{
    public static TemplatesHandler Instance;
    public List<TemplateDataInformation> templatesData;
    public List<TemplateCategory> categoriesData;
    [SerializeField] [TableList] private List<TemplateCatalog> templatesCatalog;


    
    private List<TemplateCategoryBtn> _allCategories;
    private List<TemplateBtn> _allTemplates;
    private TemplateCategoryBtn _selectedCategory;
    private TemplateBtn _selectedTemplate;
    private TemplateBtn _submittedTemplate;
    private TemplateData _submittedData;
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void GetTemplate()
    {
    }
    public TemplateDataInformation GetTemplateData(int id)
    {
        return templatesData.FirstOrDefault(t => t.templateName == (TemplatesNames) id);
    }
    public TemplateDataInformation GetTemplateData(TemplatesNames tName)
    {
        return templatesData.FirstOrDefault(t => t.templateName == tName);
    }
    public TemplateCategory GetCategory(TemplatesCategories category)
    {
        return this.categoriesData.FirstOrDefault(c => c.category == category);
    }
}