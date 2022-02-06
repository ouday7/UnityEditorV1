using System.Collections;
using System.Collections.Generic;
using Envast.Layouts;
using ModeManager;
using Sirenix.Utilities;
using Templates;
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
        [SerializeField] private CustomGridLayout mainGridLayout ;
        private Vector2 defaultSize;
        private IEnumerator coroutine;
        private static int nbr = 1;
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

        private void OnDestroy()
        {
            QuestionBtn.OnClickQuestion -= ClickQuestion;
        }

        private void OpenPanel()
        {
            panelPopUp.gameObject.SetActive(true);
        }

        private void ClickQuestion(QuestionBtn qstBtn)
        {
            qstBtn.Select();
            currentQuestion = qstBtn.Data;
            mainQuestionText.text = currentQuestion.mainQst;
            subQuestionText.text = currentQuestion.subQst;
            helpQuestionText.text = currentQuestion.helpQst;

            if (currentQuestion.templateId== 0)
            {
                currentQuestion = qstBtn.Data;
                selectTemplateBtn.templateIcon.sprite = null;
                selectTemplateBtn.templateNameTxt.text = "None";
                minFieldsTxt.text = "Min Fiddles : ";
                addQuizField.gameObject.SetActive(false);
                var childCount = quizFieldsHolder.RectTransform.childCount;
                RemoveTemplateFromHierarchy(childCount);
                Invoke(nameof(ReturnMainContentToDefaultSize),.01f);
                return;
            }
            
            var templateData = TemplatesHandler.GetTemplateDataById(qstBtn.Data.templateId);
            OnTemplateSelected(templateData);
        }
        
        private void ReturnMainContentToDefaultSize()
        {
            mainHolder.RectTransform.sizeDelta = new Vector2(defaultSize.x, defaultSize.y);
        }

        public void MaximiseMainContentHolder()
        {
            var nbChild = quizFieldsHolder.transform.childCount;
            if (nbChild == 0)
            {
                mainContentHolder.RectTransform.sizeDelta = new Vector2(defaultSize.x, defaultSize.y);
                Invoke(nameof(UpdateHolderSize),.01f);
                return;
            }

            mainContentHolder.RectTransform.sizeDelta = new Vector2(defaultSize.x, defaultSize.y + nbChild * 105);
            Invoke(nameof(UpdateHolderSize),.01f);
        }
        public void UpdateHolderSize() =>quizFieldsHolder.UpdateLayout();
        
        public void UpdateGridLayout() => mainGridLayout.UpdateLayout();

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

            Invoke(nameof(MaximiseMainContentHolder),.05f);
            
            GameDataManager.instance.SaveToJson();
            Invoke(nameof(UpdateGridLayout),.05f);
        }

        private void GenerateQuestionFields()
        {
            nbr = 1;
            for (var i = 0; i < currentQuestion.quizFields.Count; i++)
            {
                var quizFieldType = currentTemplate.GetQuizFieldType(i);
                QuizFieldsHandler.Instance.GetQuizField(quizFieldType,GetFields);
            }
        }
        private void GetFields(QuizFieldBase quizField)
        {
            var data = currentQuestion.quizFields[nbr-1];
            quizField.transform.SetParent(quizFieldsHolder.RectTransform);
            quizField.transform.localScale = Vector3.one;
            quizField.Initialize();
            data.id = nbr;
            quizField.BindData(data);
            tempTemplatId = nbr;
            nbr++;
            MaximiseMainContentHolder();
            UpdateHolderSize();
            GameDataManager.instance.SaveToJson();
        }

        private void GenerateDefaultFields()
        {
            nbr = 1;
            Debug.Log("//. Generate Default Fields");
            currentQuestion.quizFields = new List<QuizFieldData>();
            for (var i = 0; i < currentTemplate.minFields; i++)
            {
                var quizFieldType = currentTemplate.GetQuizFieldType(i);
                QuizFieldsHandler.Instance.GetQuizField(quizFieldType,OnGetQuizfields);
            }
            Invoke(nameof(UpdateGridLayout),.05f);
        }
        private void OnGetQuizfields(QuizFieldBase quizField)
        {
            var data = new QuizFieldData();
            quizField.transform.SetParent(quizFieldsHolder.RectTransform); 
            quizField.transform.localScale = Vector3.one;
            data.id = nbr;
            quizField.Initialize();
            quizField.BindData(data);
            currentQuestion.quizFields.Add(data);
            Debug.Log($"saved data : "+data);
            tempTemplatId = nbr;
            nbr++;
            MaximiseMainContentHolder();
            UpdateHolderSize();
            mainGridLayout.UpdateLayout();
            GameDataManager.instance.SaveToJson();
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
            if (nbQuizFields >= currentTemplate.maxFields) return;

            var quizFieldType = currentTemplate.GetQuizFieldType(tempTemplatId);
            QuizFieldsHandler.Instance.GetQuizField(quizFieldType, OnGetQuizField);
            quizFieldsHolder.UpdateLayout();
        }

        private void OnGetQuizField(QuizFieldBase quizField)
        {
            var data = new QuizFieldData
            {
                id = quizFieldsHolder.RectTransform.childCount + 1
            };
            quizField.transform.SetParent(quizFieldsHolder.RectTransform);
            quizField.Initialize();
            quizField.BindData(data);
            currentQuestion.quizFields.Add(data);
            GameDataManager.instance.SaveToJson();
            tempTemplatId++;
            UpdateMainHolderByOneItem(true);
            UpdateMainContentPosition(true);
            Invoke(nameof(UpdateGridLayout),.05f);
        }

        public void UpdateMainHolderByOneItem(bool add)
        {
            var nb = -1;
            if (add) nb=1;
            var sizeDelta = mainGridLayout.RectTransform.sizeDelta;
            sizeDelta = new Vector2(sizeDelta.x, sizeDelta.y +(nb* 120));
            mainGridLayout.RectTransform.sizeDelta = sizeDelta;
            mainGridLayout.UpdateLayout();
            quizFieldsHolder.UpdateLayout();
        } 

        public void UpdateMainContentPosition(bool Down)
        {
            var nb = -1;
            if (Down)  nb= 1;
            var position = mainHolder.RectTransform.position;
            position = new Vector2(position.x,
                position.y +(nb* 150));
            mainHolder.RectTransform.position = position;
        }
    }
}