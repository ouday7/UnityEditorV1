using System.Collections.Generic;
using UnityEngine;


namespace Envast.EditorDialogs.Select_Template
{
    public class SelectTemplate : MonoBehaviour
    {
     // to do move script spawn here 
     // set modifications from categorie & templates
     
    
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
            
        //  Generate();

        }
        
 

        // private void Generate()
        // {
        //     
        //     //- Generate Categories
        //     foreach (var categoryData in TemplatesHandler.Instance.categoriesData)
        //     {
        //        
        //         Debug.Log("test");
        //         var category = Instantiate(categoryPrefab, categoriesHolder).gameObject.GetComponent<TemplateCategoryBtn>();
        //         this._allCategories.Add(category);
        //         category.Initialize(categoryData);
        //     }
        //
        //     //- Generate Templates
        //     foreach (var templateData in TemplatesHandler.Instance.templatesData)
        //     {
        //         var template = Instantiate(templatePrefab, templatesHolder).gameObject.GetComponent<TemplateBtn>();
        //         this._allTemplates.Add(template);
        //         template.Initialize(templateData);
        //         template.SetVisibility(false);
        //     }
        // }

    }
}
