using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;


[System.Serializable] public struct TemplateCatalog
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


    [SerializeField] private RectTransform categoriesHolder;
    [SerializeField] private RectTransform templatesHolder;
    [SerializeField] private GameObject categoryPrefab;
    [SerializeField] private GameObject templatePrefab;
    
    private List<TemplateCategoryBtn> _allCategories;
    private List<TemplateBtn> _allTemplates;
    private TemplateCategoryBtn _selectedCategory;
    private TemplateBtn _selectedTemplate;
    private TemplateBtn _submittedTemplate;
    private TemplateData _submittedData;

    
    
    
    public void Start()
    {
        Generate();
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


    private void Generate()
    {
        foreach (var categoryData in categoriesData)
        {
            Debug.Log(categoriesData.Count);
          //gameObject.SetActive();
          var category = Instantiate(categoryPrefab, categoriesHolder).gameObject.GetComponent<TemplateCategoryBtn>();
            category.Initialize(categoryData);
        }

        foreach (var templateData in templatesData)
        {
            Debug.Log( "count template data " + templatesData.Count );
            var template = Instantiate(templatePrefab, templatesHolder).gameObject.GetComponent<TemplateBtn>();
            template.Initialize(templateData);
        }
    }


  
}