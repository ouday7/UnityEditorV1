using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Envast.Components.GridLayout.Helpers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class EditManager : MonoBehaviour
    {
        public static EditManager instance; 
        
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
        [SerializeField] private Button addQuizFiled;
        [SerializeField] private int tempTemplatId;

        public void Begin()
        {
            if (instance != null) return;
                instance = this;
                
            openPanel.onClick.AddListener(OpenPanel);
            QuestionBtn.OnClickQuestion += ClickQuestion;
            selectTemplateDialog.OnSubmitTemplate += OnTemplateSelected;
            addQuizFiled.onClick.AddListener(QuizFieldsMaxGenerate);
        }
        private void OpenPanel()
        {
            panelPopUp.DOAnchorPos(new Vector2(0, 0), 0.35f);
            panelPopUp.GameObject().SetActive(true);
        }
        private void ClickQuestion(QuestionBtn qstBtn)
        {
            currentQuestion = qstBtn.Data;
            
            mainQuestionText.onEndEdit.RemoveAllListeners();
            subQuestionText.onEndEdit.RemoveAllListeners();
            helpQuestionText.onEndEdit.RemoveAllListeners();

            mainQuestionText.onEndEdit.AddListener(delegate { EditMainQuestion(mainQuestionText.text, qstBtn); });
            subQuestionText.onEndEdit.AddListener(delegate { EditSubQuestion(subQuestionText.text, qstBtn); });
            helpQuestionText.onEndEdit.AddListener(delegate { EditHelpQuestion(helpQuestionText.text, qstBtn); });
            mainQuestionText.text = qstBtn.Data.mainQst;
            subQuestionText.text = qstBtn.Data.subQst;
            helpQuestionText.text = qstBtn.Data.helpQst;
            if (qstBtn.Data.templateId == 0)
            {
                selectTemplateBtn.templateIcon.sprite = null;
                selectTemplateBtn.templateNameTxt.text = "None";
                minFieldsTxt.text = "Min Fiddles : " ;
                addQuizFiled.gameObject.SetActive(false);
                RemoveDataTemplate();
                return;
            }

            var templateData =
                TemplatesHandler.Instance.templatesData.FirstOrDefault(template => template.id == qstBtn.Data.templateId);
            OnTemplateSelected(templateData);
        }
        private void EditMainQuestion(string arg0, QuestionBtn qstBtn)
        {
            qstBtn.Data.mainQst = arg0;
            GameDataManager.instance.SaveToJson();
        }
        private void EditSubQuestion(string arg0, QuestionBtn qstBtn)
        {
            qstBtn.Data.subQst = arg0;
            GameDataManager.instance.SaveToJson();
        }
        private void EditHelpQuestion(string arg0, QuestionBtn qstBtn)
        {
            qstBtn.Data.helpQst = arg0;
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
        }
        private void GenerateTemplateFields()
        {
            RemoveDataTemplate();
            addQuizFiled.gameObject.SetActive(true);
            if (currentQuestion.quizFields == null)
            {
                currentQuestion.quizFields = new List<QuizFieldData>();
                for (var i = 0; i < currentTemplate.minFields; i++)
                {
                    var data = new QuizFieldData();
                    var quizFieldType = currentTemplate.GetQuizFieldType(i);
                    var quizField = QuizFieldsHandler.GetQuizField(quizFieldType);
                    quizField.transform.SetParent(templateHolder);
                    quizField.transform.localScale=Vector3.one;
                    quizField.Initialize();
                    quizField.BindData(data);
                    currentQuestion.quizFields.Add(data);
                    GameDataManager.instance.SaveToJson();
                    tempTemplatId = i;
                }
            }
            else
            {
                for (var i = 0; i <currentQuestion.quizFields.Count ; i++)
                {
                    var data = currentQuestion.quizFields[i];
                    var quizFieldType = currentTemplate.GetQuizFieldType(i);
                    var quizFieldPrefab = QuizFieldsHandler.GetQuizField(quizFieldType);
                    var quizField = Instantiate(quizFieldPrefab, templateHolder);
                    quizField.Initialize();
                    quizField.BindData(data);
                    GameDataManager.instance.SaveToJson();
                    tempTemplatId = i;
                }   
            }
        }
        private void RemoveDataTemplate()
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
            quizField.Initialize();
            quizField.BindData(data);
            currentQuestion.quizFields.Add(data);
            Instantiate(quizField, templateHolder);
            GameDataManager.instance.SaveToJson();
            tempTemplatId++;
        }
    }
}