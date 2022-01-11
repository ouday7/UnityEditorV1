using System.Collections.Generic;
using System.Linq;
using Envast.Layouts;
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
        [SerializeField] private QuestionData currentQuestionData;
        [SerializeField] private CustomGridLayout quizfieldsHolder;
        [SerializeField] private Button openPanel;
        [SerializeField] private RectTransform panelPopUp;
        [SerializeField] private InputField mainQuestionText;
        [SerializeField] private InputField subQuestionText;
        [SerializeField] private InputField helpQuestionText;
        [SerializeField] private TemplateBtn templateBtn;
        [SerializeField] private TemplateCategory templateCategory;
        [SerializeField] private Button addQuizField;
        [SerializeField] private int tempTemplatId;
        [SerializeField] private CustomSizeFitter maincontentHolder;
        private QuestionBtn currentQuestion;
        private Vector2 defaultholderSize;
        private int HeightToAdd=200;

        public CustomGridLayout quizFieldsHolder => quizfieldsHolder;      
        public override void Begin()
        {
            if (instance != null) return;
                instance = this;
                
            openPanel.onClick.AddListener(OpenPanel);
            QuestionBtn.OnClickQuestion += ClickQuestion;
            selectTemplateDialog.OnSubmitTemplate += OnTemplateSelected;
            addQuizField.onClick.AddListener(QuizFieldsMaxGenerate);

            mainQuestionText.onEndEdit.AddListener(UpdateMainQuestion);
            subQuestionText.onEndEdit.AddListener(UpdateSubQuestion);
            helpQuestionText.onEndEdit.AddListener(UpdateHelpQuestion);
            defaultholderSize = new Vector2(1484, 920);
        }

        private void OpenPanel()
        {
            panelPopUp.GameObject().SetActive(true);
        }
        private void ClickQuestion(QuestionBtn qstBtn)
        {
            currentQuestionData = qstBtn.Data;
            currentQuestion = qstBtn;
            mainQuestionText.text = qstBtn.Data.mainQst;
            subQuestionText.text = qstBtn.Data.subQst;
            helpQuestionText.text = qstBtn.Data.helpQst;
            maincontentHolder.RectTransform.sizeDelta = defaultholderSize;
            RemoveTemplateFromHierarchy();
            if (qstBtn.Data.templateId == 0)
            {
                selectTemplateBtn.templateIcon.sprite = null;
                selectTemplateBtn.templateNameTxt.text = "None";
                minFieldsTxt.text = "Min Fiddles : " ;
                addQuizField.gameObject.SetActive(false);
                return;
            }

            var templateData =
                TemplatesHandler.Instance.templatesData.FirstOrDefault(template => template.id == qstBtn.Data.templateId);
            OnTemplateSelected(templateData);
        }
        private void UpdateMainQuestion(string inNewValue)
        {
            currentQuestionData.mainQst = inNewValue;
            GameDataManager.instance.SaveToJson();
        }
        private void UpdateSubQuestion(string inNewValue)
        {
            currentQuestionData.subQst = inNewValue;
            GameDataManager.instance.SaveToJson();
        }
        private void UpdateHelpQuestion(string inNewValue)
        {
            currentQuestionData.helpQst = inNewValue;
            GameDataManager.instance.SaveToJson();
        }
        private void OnTemplateSelected(TemplateData inTemplate)
        {
            currentTemplate = inTemplate; 
            currentQuestionData.templateId = currentTemplate.id;
            selectTemplateBtn.templateIcon.sprite = currentTemplate.icon;
            selectTemplateBtn.templateNameTxt.text = currentTemplate.templateName.ToString();
            minFieldsTxt.text = "Min Fields : " + currentTemplate.minFields;
            GenerateTemplateFields();
        }
        private void GenerateTemplateFields()
        {
            RemoveTemplateFromHierarchy();
            // RemoveDataFromJson();
            addQuizField.gameObject.SetActive(true);
           // RemoveTemplateFromHierarchy();
           
            if (currentQuestionData.quizFields.Count == 0)
            {
                currentQuestionData.quizFields = new List<QuizFieldData>();
                
                for (var i = 0; i < currentTemplate.minFields; i++)
                {
                    var data = new QuizFieldData();
                    var quizFieldType = currentTemplate.GetQuizFieldType(i);
                    var quizField = QuizFieldsHandler.GetQuizField(quizFieldType);
                    quizField.transform.SetParent(quizfieldsHolder.RectTransform);
                    quizField.transform.localScale=Vector3.one;
                    quizField.Initialize();
                    quizField.BindData(data);
                    currentQuestionData.quizFields.Add(data);
                    tempTemplatId = i;
                    quizfieldsHolder.UpdateLayout();
                }
                MaximiseMainContentHolder(currentQuestionData.quizFields.Count);
            }
            else
            {
              //  RemoveDataFromJson();
              //RemoveDataFromJson();
                Debug.Log($"//. This Question Has {currentQuestionData.quizFields.Count} Fields");
                for (var i = 0; i <currentQuestionData.quizFields.Count ; i++)
                {
                   // var templatedata = currentQuestion.templateId;
                   var data = currentQuestionData.quizFields[i];
                    var quizFieldType = currentTemplate.GetQuizFieldType(i);
                    var quizField = QuizFieldsHandler.GetQuizField(quizFieldType);
                    quizField.transform.SetParent(quizfieldsHolder.RectTransform);
                    // quizField.transform.localScale=Vector3.one;
                    quizField.Initialize();
                    quizField.BindData(data);
                    tempTemplatId = i;
                    quizfieldsHolder.UpdateLayout();
                }
                MaximiseMainContentHolder(currentQuestionData.quizFields.Count);
            }
            GameDataManager.instance.SaveToJson();

        }
        public void MaximiseMainContentHolder(int child)
        {
            if(child==0)
            {
                maincontentHolder.RectTransform.sizeDelta=defaultholderSize;
                quizfieldsHolder.UpdateLayout();
                return;
            }
            maincontentHolder.RectTransform.sizeDelta = new Vector2(defaultholderSize.x, defaultholderSize.y +(child* HeightToAdd));
            maincontentHolder.transform.position = new Vector2(maincontentHolder.transform.position.x, 800);
            quizfieldsHolder.UpdateLayout();
        }
        private void RemoveTemplateFromHierarchy()
        {
            foreach (Transform child in quizfieldsHolder.RectTransform)
                {
                    Destroy(child.gameObject);
                }
        }
        private void RemoveDataFromJson()
        {

            if (currentQuestionData.quizFields[0].id == selectTemplateDialog.selectedTemplate.Data.id )
            {
                Debug.Log(currentQuestionData.templateId +"template id ,current question ");
                
            }
            // for (int i = 0; i < currentQuestion.quizFields.Count; i++)
            // {
            //     var data = currentQuestion.quizFields[i];
            //     currentQuestion.quizFields.Remove(data);
            // }   
          
            
        }
        private void QuizFieldsMaxGenerate()
        {
            var x = quizfieldsHolder.RectTransform.childCount;
            if (x >= currentTemplate.maxFields) return;
            Debug.Log(currentTemplate.GetQuizFieldType(tempTemplatId));
            var data = currentQuestionData.quizFields[tempTemplatId];
            var quizFieldType = currentTemplate.GetQuizFieldType(tempTemplatId);
            var quizField = QuizFieldsHandler.GetQuizField(quizFieldType);
            quizField.transform.SetParent(quizfieldsHolder.RectTransform);
            MaximiseMainContentHolder(quizfieldsHolder.RectTransform.childCount);
            quizField.Initialize();
            quizField.BindData(data);
            currentQuestionData.quizFields.Add(data);
            GameDataManager.instance.SaveToJson();
            tempTemplatId++;
        }
    }
}