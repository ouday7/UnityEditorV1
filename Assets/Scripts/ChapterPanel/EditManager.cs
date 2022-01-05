using ChapterPanel;
using Envast.Layouts;
using UnityEngine;
using UnityEngine.UI;


public class EditManager : MonoBehaviour
{
    [SerializeField] private Text minFieldsTxt;
    [SerializeField] private QuestionFields questionFields;
    [SerializeField] private SelectTemplateDialog selectTemplateDialog;
    [SerializeField] private CustomSizeFitter holder;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private TemplateCategory currentTemplateCategory;
    [SerializeField] public SelectTemplateButton selectTemplateBtn;
    [SerializeField] public TemplateData currentTemplate;
    [SerializeField] private QuestionData currentQuestion;
    [SerializeField] private RectTransform templateHolder;
    [SerializeField] private Button openPanel;
    [SerializeField] private GameObject panelPopUp;
    [SerializeField] private InputField mainQuestion;
    [SerializeField] private InputField subQuestion;
    [SerializeField] private InputField helpQuestion;


    public void Start()
    {
        openPanel.onClick.AddListener(OpenPanel);
        QuestionBtn.OnClickQuestion += ClickQuestion;
    }
    private void ClickQuestion(QuestionBtn qstBtn)
    {
        mainQuestion.text = qstBtn.Data.mainQst;
        subQuestion.text = qstBtn.Data.subQst;
        helpQuestion.text = qstBtn.Data.subQst;
    }
   
    public void OnQuestionSelected(QuestionData inQuestion)
    {
        currentQuestion = inQuestion;
    }
    private void OnTemplateSelected(TemplateData inTemplate)
    {
        currentTemplate = inTemplate;
        currentQuestion.templateId = currentTemplate.id;
        selectTemplateBtn.templateIcon.sprite = currentTemplate.icon;
        selectTemplateBtn.templateNameTxt.text = currentTemplate.templateName.ToString();
        minFieldsTxt.text = currentTemplate.minFields.ToString();
        GenerateTemplateFields();
    }
    private void GenerateTemplateFields()
    {
        for (var i = 0; i < currentTemplate.minFields; i++)
        {
            var data = new QuizFieldData();
            var quizFieldType = currentTemplate.GetQuizFieldType(i);
            var quizField = QuizFieldsHandler.GetQuizField(quizFieldType);
            quizField.Initialize();
            quizField.BindData(data);
            currentQuestion.quizFields.Add(data);
        }
    }
    private void SpawnData()
    {
        /*
         RemoveDataTemplate();
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
        // ClosePanelPopUp();
    }
    private void AddQuizFiled()
    {
        // /*todo: Move to EditManager
        // if (_selectedTemplate.Data.maxFields > templateHolder.GetChildren().Count)
        // {
        //     Instantiate(TemplatesHandler.Instance.templatesCatalog[_selectedTemplate.Data.id].prefab,
        //         templateHolder);
        //     
        // }
    }
    private void RemoveDataTemplate()
    {
        foreach (Transform child in templateHolder) {
            Destroy(child.gameObject);
        }
    }
    private void OpenPanel()
    {
        panelPopUp.SetActive(true);
    }
}