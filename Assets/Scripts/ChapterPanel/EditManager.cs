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
        
        
      
        public override void Begin()
        {
            if (instance != null) return;
                instance = this;
                
            openPanel.onClick.AddListener(OpenPanel);
            QuestionBtn.OnClickQuestion += ClickQuestion;
            selectTemplateDialog.OnSubmitTemplate += OnTemplateSelected;
            addQuizFiled.onClick.AddListener(QuizFieldsMaxGenerate);

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
                minFieldsTxt.text = "Min Fiddles : " ;
                addQuizFiled.gameObject.SetActive(false);
                RemoveTemplateFromHierarchy();
                return;
            }

            var templateData =
                TemplatesHandler.Instance.templatesData.FirstOrDefault(template => template.id == qstBtn.Data.templateId);
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
        }
        private void GenerateTemplateFields()
        {
            // RemoveDataFromJson();
            addQuizFiled.gameObject.SetActive(true);
           // RemoveTemplateFromHierarchy();

            if (currentQuestion.quizFields == null)
            {
                currentQuestion.quizFields = new List<QuizFieldData>();
                for (var i = 0; i < currentTemplate.minFields; i++)
                {
                    var data = new QuizFieldData();
                    var quizFieldType = currentTemplate.GetQuizFieldType(i);
                    var quizField = QuizFieldsHandler.GetQuizField(quizFieldType);
                    quizField.transform.SetParent(templateHolder);
                   // quizField.transform.localScale=Vector3.one;
                    quizField.Initialize();
                    quizField.BindData(data);
                    currentQuestion.quizFields.Add(data);
                    tempTemplatId = i;
                }
            }
            else
            {
              //  RemoveDataFromJson();
              RemoveDataFromJson();
                Debug.Log($"//. This Question Has {currentQuestion.quizFields.Count} Fields");
                for (var i = 0; i <currentQuestion.quizFields.Count ; i++)
                {
                   // var templatedata = currentQuestion.templateId;
                    
                    
                    var data = currentQuestion.quizFields[i];
                    var quizFieldType = currentTemplate.GetQuizFieldType(i);
                    var quizField = QuizFieldsHandler.GetQuizField(quizFieldType);
                    quizField.transform.SetParent(templateHolder);
                   // quizField.transform.localScale=Vector3.one;
                    quizField.Initialize();
                    quizField.BindData(data);
                    tempTemplatId = i;
                }
            }
            GameDataManager.instance.SaveToJson();

        }
        private void RemoveTemplateFromHierarchy()
        {
            //todo clear data
                foreach (Transform child in templateHolder)
                {
                    Debug.Log("//. Destroy QF_GameObject");
                    Destroy(child.gameObject);
                
                }

        }

        private void RemoveDataFromJson()
        {

            if (currentQuestion.quizFields[0].id == selectTemplateDialog.selectedTemplate.Data.id )
            {
                Debug.Log(currentQuestion.templateId +"template id ,current question ");
                
            }
            // for (int i = 0; i < currentQuestion.quizFields.Count; i++)
            // {
            //     var data = currentQuestion.quizFields[i];
            //     currentQuestion.quizFields.Remove(data);
            // }   
          
            
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
    }
}