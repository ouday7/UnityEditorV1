using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class SelectTemplateDialog : MonoBehaviour
    {
        public static SelectTemplateDialog instance;
        public event Action<TemplateData> OnSubmitTemplate;
        public TemplateData submittedData;
        public Button closeBtn;
        public RectTransform templateCategoryPopUp;

        [SerializeField] private RectTransform categoriesHolder;
        [SerializeField] private RectTransform templatesHolder;
        [SerializeField] private GameObject categoryPrefab;
        [SerializeField] private GameObject templatePrefab;

        private SelectTemplateButton _selectTemplateButton;
        private List<TemplateCategoryBtn> _allCategories;
        private List<TemplateBtn> _allTemplates;
        private TemplateCategoryBtn _selectedCategory;
        public TemplateBtn selectedTemplate;
        private TemplateBtn _submittedTemplate;
        private TemplatesHandler _templatesHandler;

        private void OnDestroy()
        {
            TemplateCategoryBtn.onClick -= OnClickCategory;
            TemplateBtn.onSelect -= OnSelectTemplate;
            TemplateBtn.onSubmit -= OnTemplateSubmitted;
        }

        public void Begin()
        {
            if(instance!=null) return;
            instance = this;
            
            this.selectedTemplate = null;
            this._submittedTemplate = null;
            this._selectedCategory = null;
            this._allCategories = new List<TemplateCategoryBtn>();
            this._allTemplates = new List<TemplateBtn>();
            TemplateCategoryBtn.onClick += OnClickCategory;
            TemplateBtn.onSelect += OnSelectTemplate;
            TemplateBtn.onSubmit += OnTemplateSubmitted;
            Generate();
            ClosePanelPopUp();
            closeBtn.onClick.AddListener(ClosePanelPopUp);
        }

        private void OnTemplateSubmitted(TemplateBtn btn, TemplateData data)
        {
            _submittedTemplate = btn;
            submittedData = data;
            Debug.Log("<color=red> template name selected :</color>" + data.templateName);
            if (submittedData == null)
            {
                Debug.Log("NULL !");
                return;
            }

            ClosePanelPopUp();
            OnSubmitTemplate?.Invoke(data);
        }

        private void OnSelectTemplate(TemplateBtn btn)
        {
            if (selectedTemplate != null) selectedTemplate.Unselect();
            selectedTemplate = btn;
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