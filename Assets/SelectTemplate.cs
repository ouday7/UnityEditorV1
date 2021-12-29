using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectTemplate : MonoBehaviour
    {
        [SerializeField] private RectTransform categoriesHolder;
        [SerializeField] private RectTransform templatesHolder;
        [SerializeField] private GameObject categoryPrefab;
        [SerializeField] private GameObject templatePrefab;
        [SerializeField] private List<TemplateCategoryBtn> allCategories;
        [SerializeField] private List<TemplateBtn> allTemplates;
        private TemplateCategoryBtn _selectedCategory;
        private TemplateBtn _selectedTemplate;
        private TemplateBtn _submittedTemplate;
        private TemplateData _submittedData;

        public Button closeBtn;
        public GameObject templateCategoriePopUp;


        public void Start()
        {
            
          Generate();
          closeBtn.onClick.AddListener(ClosePanelPopUp);


        }
        private void Generate()
        { 
            Debug.Log(TemplatesHandler.Instance.categoriesData.Count);
          //Debug.Log(TemplatesHandler.Instance.name);
            foreach (var categoryData in TemplatesHandler.Instance.categoriesData)
            {
               
                Debug.Log("test");
                var category = Instantiate(categoryPrefab, categoriesHolder).gameObject.GetComponent<TemplateCategoryBtn>();
                this.allCategories.Add(category);
                category.Initialize(categoryData);
            }
        
            //- Generate Templates
            foreach (var templateData in TemplatesHandler.Instance.templatesData)
            {
                Debug.Log(TemplatesHandler.Instance.templatesData.Count+"template Data count ! ");
                var template = Instantiate(templatePrefab, templatesHolder).gameObject.GetComponent<TemplateBtn>();
               this.allTemplates.Add(template);
                template.Initialize(templateData);
             //   template.SetVisibility(false);
            }
        }
        private void ClosePanelPopUp()
        {
            templateCategoriePopUp.SetActive(false);
            
        }
        
     
    }
  


