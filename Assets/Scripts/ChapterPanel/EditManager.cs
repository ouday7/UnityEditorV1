using System.Collections;
using System.Collections.Generic;
using Envast.Layouts;
using ModeManager;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class EditManager : EntryPointSystemBase
    {
        public static EditManager Instance;
        [SerializeField] private Text minFieldsTxt;
        [SerializeField] private SelectTemplateDialog selectTemplateDialog;
        [SerializeField] private TemplateCategory currentTemplateCategory;
        [SerializeField] public SelectTemplateButton selectTemplateBtn;
        [SerializeField] public TemplateData currentTemplate;
        [SerializeField] private QuestionData currentQuestion;
        [SerializeField] private CustomGridLayout quizFieldsHolder;
        [SerializeField] private CustomSizeFitter mainContentHolder;
        [SerializeField] private Button openPanel;
        [SerializeField] private RectTransform panelPopUp;
        [SerializeField] private InputField mainQuestionText;
        [SerializeField] private InputField subQuestionText;
        [SerializeField] private InputField helpQuestionText;
        [FormerlySerializedAs("templateBtn")] [SerializeField] private TemplateButton templateButton;
        [SerializeField] private TemplateCategory templateCategory;
        [SerializeField] private Button addQuizField;
        [SerializeField] private int tempTemplatId;
        private Vector2 defaultSize;
        private IEnumerator coroutine;
        public QuestionData currentQuestionData => currentQuestion;
        public CustomGridLayout QuizFieldsHolder => quizFieldsHolder;
        public CustomSizeFitter mainHolder => mainContentHolder;

        public override void Begin()
        {
            if (Instance != null) return;
            Instance = this;

            openPanel.onClick.AddListener(OpenPanel);
            QuestionBtn.OnClickQuestion += ClickQuestion;
            selectTemplateDialog.OnSubmitTemplate += OnTemplateSelected;
            addQuizField.onClick.AddListener(AddQuizFields);
            mainQuestionText.onEndEdit.AddListener(UpdateMainQuestion);
            subQuestionText.onEndEdit.AddListener(UpdateSubQuestion);
            helpQuestionText.onEndEdit.AddListener(UpdateHelpQuestion);
            defaultSize = new Vector2(1484, 920f);
        }

        private void OpenPanel()
        {
            panelPopUp.GameObject().SetActive(true);
        }

        private void ClickQuestion(QuestionBtn qstBtn)
        {
            currentQuestion = qstBtn.Data;

            mainQuestionText.text = qstBtn.Data.mainQst;
            subQuestionText.text = qstBtn.Data.subQst;
            helpQuestionText.text = qstBtn.Data.helpQst;

            if (qstBtn.Data.templateId == 0)
            {
                selectTemplateBtn.templateIcon.sprite = null;
                selectTemplateBtn.templateNameTxt.text = "None";
                minFieldsTxt.text = "Min Fiddles : ";
                addQuizField.gameObject.SetActive(false);
                var childCount = quizFieldsHolder.RectTransform.childCount;
                RemoveTemplateFromHierarchy(childCount);
                Invoke(nameof(MaximiseMainContentHolder),.01f);
                return;
            }

            var templateData = TemplatesHandler.GetTemplateDataById(qstBtn.Data.templateId);
            OnTemplateSelected(templateData);
        }

        public void MaximiseMainContentHolder()
        {
            var nbChild = currentQuestionData.quizFields.Count;
            if (nbChild == 0)
            {
                mainContentHolder.RectTransform.sizeDelta = new Vector2(defaultSize.x, defaultSize.y);
                Invoke(nameof(UpdateHodlerSize),.01f);
                return;
            }
            mainContentHolder.RectTransform.sizeDelta = new Vector2(defaultSize.x, defaultSize.y + nbChild * 210);
            Invoke(nameof(UpdateHodlerSize),.01f);
        }

        private void UpdateHodlerSize()=> quizFieldsHolder.UpdateLayout();
        
        private void UpdateMainQuestion(string inNewValue)
        {
            currentQuestion.mainQst = inNewValue;
            GameDataManager.instance.SaveToJson();
        }

        private void UpdateSubQuestion(string inNewValue)
        {
            currentQuestion.subQst = inNewValue;
            GameDataManager.instance.SaveToJson();
        }

        private void UpdateHelpQuestion(string inNewValue)
        {
            currentQuestion.helpQst = inNewValue;
            GameDataManager.instance.SaveToJson();
        }

        private void OnTemplateSelected(TemplateData inTemplate)
        {
            ClearPreviousQuizFields(inTemplate);
            currentTemplate = inTemplate;
            currentQuestion.templateId = currentTemplate.id;
            EditorModeManager.currentQuestionData = currentQuestion;
            selectTemplateBtn.SetTemplate(inTemplate);
            minFieldsTxt.text = "Min Fields : " + currentTemplate.minFields;
            GenerateTemplateFields();
            GameDataManager.instance.SaveToJson();
        }

        private void ClearPreviousQuizFields(TemplateData inTemplate)
        {
            if(currentQuestion.templateId == 0) return;
            if(currentQuestion.templateId == inTemplate.id) return;
            Debug.Log("//. Previous Template Different then current template Clear Data");
            currentQuestion.quizFields = new List<QuizFieldData>();
            var childCount = quizFieldsHolder.RectTransform.childCount;
            while (childCount > 0)
            {
                Destroy(quizFieldsHolder.RectTransform.GetChild(0).gameObject);
                childCount--;
            }
        }

        private void GenerateTemplateFields()
        {
            addQuizField.gameObject.SetActive(true);
            RemoveTemplateFromHierarchy(quizFieldsHolder.RectTransform.childCount);

            if (currentQuestion.quizFields.IsNullOrEmpty())
                GenerateDefaultFields();
            else
                GenerateQuestionFields();

            Invoke(nameof(MaximiseMainContentHolder),.01f);
            GameDataManager.instance.SaveToJson();
        }

        private void GenerateQuestionFields()
        {
            for (var i = 0; i < currentQuestion.quizFields.Count; i++)
            {
                var data = currentQuestion.quizFields[i];
                var quizFieldType = currentTemplate.GetQuizFieldType(i);
                var quizField = QuizFieldsHandler.GetQuizField(quizFieldType);
                quizField.transform.SetParent(quizFieldsHolder.RectTransform);
                quizField.transform.localScale = Vector3.one;
                quizField.Initialize();
                data.id = i + 1;
                quizField.BindData(data);
                tempTemplatId = i;
                GameDataManager.instance.SaveToJson();
            }
        }

        private void GenerateDefaultFields()
        {
            Debug.Log("//. Generate Default Fields");
            currentQuestion.quizFields = new List<QuizFieldData>();
            for (var i = 0; i < currentTemplate.minFields; i++)
            {
                var data = new QuizFieldData();
                var quizFieldType = currentTemplate.GetQuizFieldType(i);
                var quizField = QuizFieldsHandler.GetQuizField(quizFieldType);
                quizField.transform.SetParent(quizFieldsHolder.RectTransform); 
                quizField.transform.localScale = Vector3.one;
                data.id = i+1;
                quizField.Initialize();
                quizField.BindData(data);
                currentQuestion.quizFields.Add(data);
                tempTemplatId = i;
            }
        }

        private void Test()
        {
            currentQuestion.quizFields.Clear();
            RemoveTemplateFromHierarchy(quizFieldsHolder.RectTransform.childCount);
            for (var i = 0; i < currentTemplate.minFields; i++)
            {
                var data = new QuizFieldData();
                var quizFieldType = currentTemplate.GetQuizFieldType(i);
                var quizField = QuizFieldsHandler.GetQuizField(quizFieldType);
                quizField.transform.SetParent(quizFieldsHolder.RectTransform);
                quizField.transform.localScale = Vector3.one;
                quizField.Initialize();
                quizField.BindData(data);
                currentQuestion.quizFields.Add(data);
                tempTemplatId = i;
                MaximiseMainContentHolder();
            }
        }

        private void RemoveTemplateFromHierarchy(int nbChild)
        {
            if (nbChild == 0) return;
            
            foreach (Transform child in quizFieldsHolder.RectTransform)
            {
                Destroy(child.gameObject);
            }
        }

        private void AddQuizFields()
        {
            var nbQuizFields = quizFieldsHolder.RectTransform.childCount;
            if (nbQuizFields >= currentTemplate.maxFields)
                return;
            
            var data = new QuizFieldData
            {
                id = nbQuizFields + 1
            };
            var quizFieldType = currentTemplate.GetQuizFieldType(tempTemplatId);
            var quizField = QuizFieldsHandler.GetQuizField(quizFieldType);
            quizField.transform.SetParent(quizFieldsHolder.RectTransform);
            quizField.Initialize();
            quizField.BindData(data);
            currentQuestion.quizFields.Add(data);
            GameDataManager.instance.SaveToJson();
            tempTemplatId++;
            MaximiseMainContentHolder();
        }
    }
}