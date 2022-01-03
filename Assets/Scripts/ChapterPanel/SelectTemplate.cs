using System.Collections.Generic;
using Envast.Components.GridLayout.Helpers;
using UnityEngine;
using UnityEngine.UI;
namespace ChapterPanel
{
    public class SelectTemplate : MonoBehaviour
    {
        public Text maxfiled;
        [SerializeField] private RectTransform categoriesHolder;
        [SerializeField] private RectTransform templatesHolder;
        [SerializeField] private GameObject categoryPrefab;
        [SerializeField] private GameObject templatePrefab;
        [SerializeField] private RectTransform templateHolder;

        public Text selectTemplateButton;
        public Image selectTemplateImage;
        [SerializeField] private Button addQuizFiled;

        private SelectTemplateButton _selectTemplateButton;
        private List<TemplateCategoryBtn> _allCategories;
        private List<TemplateBtn> _allTemplates;
        private TemplateCategoryBtn _selectedCategory;
        private TemplateBtn _selectedTemplate;
        private TemplateBtn _submittedTemplate;
        public TemplateDataInformation _submittedData;
        private TemplatesHandler _templatesHandler;
        public Button closeBtn;
        public GameObject templateCategoryPopUp;
        public void Start()
        {
            this._selectedTemplate = null;
            this._submittedTemplate = null;
            this._selectedCategory = null;
            this._allCategories = new List<TemplateCategoryBtn>();
            this._allTemplates = new List<TemplateBtn>();
            TemplateCategoryBtn.onClick += OnClickCategory;
            TemplateBtn.onSelect += OnSelectTemplate;
            TemplateBtn.onSubmit += OnSubmitTemplate;
            Generate();
            ClosePanelPopUp();
            closeBtn.onClick.AddListener(ClosePanelPopUp);
            addQuizFiled.gameObject.SetActive(false);
            addQuizFiled.onClick.AddListener(AddQuizFiled);

        }
        private void OnSubmitTemplate(TemplateBtn btn, TemplateDataInformation data)
        {
            _submittedTemplate = btn;
            _submittedData = data;

            Debug.Log("<color=red> template name selected :</color>" + data.templateName);

            if (_submittedData == null)
            {
                Debug.Log("NULL !");
                return;
            }

            selectTemplateImage.sprite = _submittedData.icon;
            selectTemplateButton.text = data.templateName.ToString();
            maxfiled.text = "Min Fields : " + _submittedData.minFields;
            addQuizFiled.gameObject.SetActive(true);
            SpawnData();
        }
        private void SpawnData()
        {
            for (var i = 0; i < TemplatesHandler.Instance.templatesCatalog.Count; i++)
            {
                if (_selectedTemplate.Data.templateName.ToString() ==
                    TemplatesHandler.Instance.templatesCatalog[i].type.ToString())
                {
                    Debug.Log(TemplatesHandler.Instance.templatesCatalog[i].prefab);
                    for (var j = 0; j < _selectedTemplate.Data.minFields; j++)
                    {
                        Instantiate(TemplatesHandler.Instance.templatesCatalog[i].prefab, templateHolder);
                    }
                }
            }

            ClosePanelPopUp();
        }
        private void AddQuizFiled()
        {
            if (_selectedTemplate.Data.maxFields > templateHolder.GetChildren().Count)
            {
                Instantiate(TemplatesHandler.Instance.templatesCatalog[_selectedTemplate.Data.id].prefab, templateHolder);
            }
        }
        private void OnSelectTemplate(TemplateBtn btn)
        {
            if (_selectedTemplate != null) _selectedTemplate.Unselect();
            _selectedTemplate = btn;
            _selectedTemplate.Select();
        }
        private void OnClickCategory(TemplateCategoryBtn btn, TemplateCategory category)
        {
            if (_selectedCategory != null) _selectedCategory.Unselect();
            _selectedCategory = btn;
            _selectedCategory.Select();
            UpdateSelectedCategory(category.category);
        }
        private void Generate()
        {
            foreach (var categoryData in TemplatesHandler.Instance.categoriesData)
            {
                var category = Instantiate(categoryPrefab, categoriesHolder).gameObject
                    .GetComponent<TemplateCategoryBtn>();
                this._allCategories.Add(category);
                category.Initialize(categoryData);
            }

            foreach (var templateData in TemplatesHandler.Instance.templatesData)
            {
                var template = Instantiate(templatePrefab, templatesHolder).gameObject.GetComponent<TemplateBtn>();
                this._allTemplates.Add(template);
                template.Initialize(templateData);
                template.SetVisibility(false);
            }
        }
        private void ClosePanelPopUp()
        {
            templateCategoryPopUp.SetActive(false);
        }
        private void UpdateSelectedCategory(TemplatesCategories category)
        {
            foreach (var tmp in _allTemplates)
                tmp.SetVisibility(tmp.Data.category == category);
        }
    }
}