using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class SelectTemplateDialog : EntryPointSystemBase
    {
        public event Action<TemplateData> OnSubmitTemplate;
        public TemplateData submittedData;
        public TemplateData x;
        public Button closeBtn;
        public RectTransform templateCategoryPopUp;

        [SerializeField] private RectTransform categoriesHolder;
        [SerializeField] private RectTransform templatesHolder;
        [SerializeField] private GameObject categoryPrefab;
        [SerializeField] private GameObject templatePrefab;

        private SelectTemplateButton _selectTemplateButton;
        private List<TemplateCategoryBtn> _allCategories;
        private List<TemplateButton> _allTemplates;
        private TemplateCategoryBtn _selectedCategory;
        public TemplateButton selectedTemplate;
        public TemplateButton _submittedTemplate;
        private TemplatesHandler _templatesHandler;


        public int z;
        private void OnDestroy()
        {
            TemplateCategoryBtn.onClick -= OnClickCategory;
            TemplateButton.onSelect -= OnSelectTemplate;
            TemplateButton.onSubmit -= OnTemplateSubmitted;
        }

        public override void Begin()
        {
            selectedTemplate = null;
            _submittedTemplate = null;
            _selectedCategory = null;
            _allCategories = new List<TemplateCategoryBtn>();
            _allTemplates = new List<TemplateButton>();
            TemplateCategoryBtn.onClick += OnClickCategory;
            TemplateButton.onSelect += OnSelectTemplate;
            TemplateButton.onSubmit += OnTemplateSubmitted;
            Generate();
            ClosePanelPopUp();
            closeBtn.onClick.AddListener(ClosePanelPopUp);
        }

        private void OnTemplateSubmitted(TemplateButton button, TemplateData data)
        {
            _submittedTemplate = button;
            submittedData = data;
            x = data;
            Debug.Log("<color=red> template name selected :</color>" + data.templateName);
            if (submittedData == null)
            {
                Debug.Log("<color=black> template is NULL !");
                return;
            }
            
            ClosePanelPopUp();
            OnSubmitTemplate?.Invoke(data);
        }

        private void OnSelectTemplate(TemplateButton button)
        {
            if (selectedTemplate != null) selectedTemplate.Unselect();
            selectedTemplate = button;
            selectedTemplate.Select();
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
            foreach (var categoryData in TemplatesHandler.Instance.Categories)
            {
                var category = Instantiate(categoryPrefab, categoriesHolder).gameObject
                    .GetComponent<TemplateCategoryBtn>();
                this._allCategories.Add(category);
                category.Initialize(categoryData);
            }

            foreach (var templateData in TemplatesHandler.Instance.Templates)
            {
                var template = Instantiate(templatePrefab, templatesHolder).gameObject.GetComponent<TemplateButton>();
                this._allTemplates.Add(template);
                template.Initialize(templateData);
                template.SetVisibility(false);
            }
        }

        private void UpdateSelectedCategory(TemplatesCategories category)
        {
            foreach (var templateBtn in _allTemplates) templateBtn.SetVisibility(templateBtn.Data.category == category);
        }

        private void ClosePanelPopUp()
        {
            templateCategoryPopUp.DOAnchorPos(new Vector2(0, 0), 0.35f);
            templateCategoryPopUp.gameObject.SetActive(false);
        }
    }
}