using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class SelectTemplateDialog : MonoBehaviour
    {
        public static event Action<EditManager> OnselectTemplate;
        
        
        public Text maxFields;
        public Text selectTemplateButton;
        public Image selectTemplateImage;
        public TemplateData submittedData;
        public Button closeBtn;
        public GameObject templateCategoryPopUp;

        [SerializeField] private RectTransform categoriesHolder;
        [SerializeField] private RectTransform templatesHolder;
        [SerializeField] private GameObject categoryPrefab;
        [SerializeField] private GameObject templatePrefab;

        [SerializeField] private RectTransform templateHolder;
        [SerializeField] private Button addQuizFiled;

        private SelectTemplateButton _selectTemplateButton;
        private List<TemplateCategoryBtn> _allCategories;
        private List<TemplateBtn> _allTemplates;
        private TemplateCategoryBtn _selectedCategory;
        public TemplateBtn selectedTemplate;
        private TemplateBtn _submittedTemplate;
        private TemplatesHandler _templatesHandler;

        // #region EditManager
        //
        // private QuestionData _currentQuestion;
        // private TemplateData _currentTemplate;
        //
        // public void OnQuestionSelected(QuestionData inQuestion)
        // {
        //     _currentQuestion = inQuestion;
        //     //update fields properly (question, template, sub question)
        // }
        //
        // private void OnTemplateSelected(TemplateData inTemplate)
        // {
        //     //update template button
        //     _currentTemplate = inTemplate;
        //     _currentQuestion.templateId = _currentTemplate.id;
        //     GenerateTemplateFields();
        // }
        //
        // private void GenerateTemplateFields()
        // {
        //     for (var i = 0; i < _currentTemplate.minFields; i++)
        //     {
        //         var data = new QuizFieldData();
        //         var quizFieldType = _currentTemplate.GetQuizFieldType(i);
        //         var quizField = QuizFieldsHandler.GetQuizField(quizFieldType);
        //         quizField.Initialize();
        //         quizField.BindData(data);
        //         _currentQuestion.quizFields.Add(data);
        //     }
        // }
        //
        // #endregion


        public void Start()
        {
            this.selectedTemplate = null;
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

        private void OnSubmitTemplate(TemplateBtn btn, TemplateData data)
        {
            _submittedTemplate = btn;
            submittedData = data;

            Debug.Log("<color=red> template name selected :</color>" + data.templateName);

            if (submittedData == null)
            {
                Debug.Log("NULL !");
                return;
            }

            // selectTemplateImage.sprite = _submittedData.icon;
            // selectTemplateButton.text = data.templateName.ToString();
            // maxFields.text = "Min Fields : " + _submittedData.minFields;

           
            addQuizFiled.gameObject.SetActive(true);
     
            
            
            SpawnData();
        }

        private void SpawnData()
        {
            RemoveDataTemplate();
            /*todo :move to EditManager
            for (var i = 0; i < TemplatesHandler.Instance.templatesCatalog.Count; i++)
            {
                if (_selectedTemplate.Data.templateName ==
                    TemplatesHandler.Instance.templatesCatalog[i].type)
                {
                    Debug.Log(TemplatesHandler.Instance.templatesCatalog[i].prefab);
                    for (var j = 0; j < _selectedTemplate.Data.minFields; j++)
                    {
                        Instantiate(TemplatesHandler.Instance.templatesCatalog[i].prefab, templateHolder);
                    }
                }
            }
            */
            ClosePanelPopUp();
        }
        private void AddQuizFiled()
        {
            /*todo: Move to EditManager
            if (_selectedTemplate.Data.maxFields > templateHolder.GetChildren().Count)
            {
                Instantiate(TemplatesHandler.Instance.templatesCatalog[_selectedTemplate.Data.id].prefab,
                    templateHolder);
            }
            */
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

        private void ClosePanelPopUp()
        {
            templateCategoryPopUp.SetActive(false);
        }

        private void UpdateSelectedCategory(TemplatesCategories category)
        {
            foreach (var templateBtn in _allTemplates) templateBtn.SetVisibility(templateBtn.Data.category == category);
        }

        private void RemoveDataTemplate()
        {
            foreach (Transform child in templateHolder)
            {
                Destroy(child.gameObject);
            }
        }
    }
}