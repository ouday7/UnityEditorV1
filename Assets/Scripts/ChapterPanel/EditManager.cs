using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Envast.Components.GridLayout.Helpers;
using Unity.VisualScripting;
using UnityEngine;
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
        [SerializeField] private RectTransform templateHolder;
        [SerializeField] private Button openPanel;
        [SerializeField] private RectTransform panelPopUp;
        [SerializeField] private InputField mainQuestionText;
        [SerializeField] private InputField subQuestionText;
        [SerializeField] private InputField helpQuestionText;
        [SerializeField] private TemplateBtn templateBtn;
        [SerializeField] private TemplateCategory templateCategory;
        [SerializeField] private Button addQuizField;
        [SerializeField] private int tempTemplatId;
        private TemplateData x;

        public QuestionData currentQuestionData => currentQuestion;
        public RectTransform TemplateHolder => templateHolder;

        public override void Begin()
        {
            if (Instance != null) return;
            Instance = this;

            openPanel.onClick.AddListener(OpenPanel);
            QuestionBtn.OnClickQuestion += ClickQuestion;
            selectTemplateDialog.OnSubmitTemplate += OnTemplateSelected;
            addQuizField.onClick.AddListener(QuizFieldsMaxGenerate);
            mainQuestionText.onEndEdit.AddListener(UpdateMainQuestion);
            subQuestionText.onEndEdit.AddListener(UpdateSubQuestion);
            helpQuestionText.onEndEdit.AddListener(UpdateHelpQuestion);
        }

        private void OpenPanel()
        {
            panelPopUp.DOAnchorPos(new Vector2(0, 0), 0.35f);
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
                RemoveTemplateFromHierarchy();
                return;
            }

            var templateData =
                TemplatesHandler.Instance.templatesData.FirstOrDefault(
                    template => template.id == qstBtn.Data.templateId);
            OnTemplateSelected(templateData);
        }

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
            currentTemplate = inTemplate;
            currentQuestion.templateId = currentTemplate.id;
            selectTemplateBtn.templateIcon.sprite = currentTemplate.icon;
            selectTemplateBtn.templateNameTxt.text = currentTemplate.templateName.ToString();
            minFieldsTxt.text = "Min Fields : " + currentTemplate.minFields;
            GenerateTemplateFields();
            GameDataManager.instance.SaveToJson();

        }
        private void GenerateTemplateFields()
        {
            addQuizField.gameObject.SetActive(true);
            RemoveTemplateFromHierarchy();
            if (currentQuestion.quizFields == null)
            {
                currentQuestion.quizFields = new List<QuizFieldData>();
                for (var i = 0; i < currentTemplate.minFields; i++)
                {
                    var data = new QuizFieldData();
                    var quizFieldType = currentTemplate.GetQuizFieldType(i);
                    var quizField = QuizFieldsHandler.GetQuizField(quizFieldType);
                    quizField.transform.SetParent(templateHolder);
                    quizField.transform.localScale = Vector3.one;
                    quizField.Initialize();
                    quizField.BindData(data);
                    currentQuestion.quizFields.Add(data);
                    tempTemplatId = i;
                }
            }
            // else if (((selectTemplateDialog.submittedData.id != currentQuestion.templateId)))
            // {
            //     Test();
            // }
            else
            {
                Debug.Log($"//. This Question Has {currentQuestion.quizFields.Count} Fields");
                for (var i = 0; i < currentQuestion.quizFields.Count; i++)
                {
                    var data = currentQuestion.quizFields[i];
                    var quizFieldType = currentTemplate.GetQuizFieldType(i);
                    var quizField = QuizFieldsHandler.GetQuizField(quizFieldType);
                    quizField.transform.SetParent(templateHolder);
                    quizField.transform.localScale = Vector3.one;
                    quizField.Initialize();
                    quizField.BindData(data);
                    tempTemplatId = i;
                }
            }
        }
        private void RemoveTemplateFromHierarchy()
        {
            foreach (Transform child in templateHolder)
            {
                Destroy(child.gameObject);
            }
        }
        private void QuizFieldsMaxGenerate()
        {
            var x = templateHolder.GetChildren().Count;
            if (x >= currentTemplate.maxFields) return;
            Debug.Log(currentTemplate.GetQuizFieldType(tempTemplatId));
            var data = currentQuestion.quizFields[tempTemplatId];
            var quizFieldType = currentTemplate.GetQuizFieldType(tempTemplatId);
            var quizField = QuizFieldsHandler.GetQuizField(quizFieldType);
            quizField.transform.SetParent(templateHolder);
            //   quizField.transform.localScale=Vector3.one;
            // Instantiate(quizField, templateHolder);
            quizField.Initialize();
            quizField.BindData(data);
            currentQuestion.quizFields.Add(data);
            GameDataManager.instance.SaveToJson();
            tempTemplatId++;
        }
        private void Test()
        {
            currentQuestion.quizFields.Clear();
            for (var i = 0; i < currentTemplate.minFields; i++)
            {
                var data = new QuizFieldData();
                var quizFieldType = currentTemplate.GetQuizFieldType(i);
                var quizField = QuizFieldsHandler.GetQuizField(quizFieldType);
                quizField.transform.SetParent(templateHolder);
                quizField.transform.localScale = Vector3.one;
                quizField.Initialize();
                quizField.BindData(data);
                currentQuestion.quizFields.Add(data);
            }
        }
    }
}