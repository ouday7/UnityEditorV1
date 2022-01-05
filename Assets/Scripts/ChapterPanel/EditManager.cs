using ChapterPanel;
using Envast.Components.GridLayout.Helpers;
using UnityEngine;
using UnityEngine.UI;
public class EditManager : MonoBehaviour
{
    [SerializeField] private Text minFieldsTxt;
    [SerializeField] private QuestionFields questionFields;
    [SerializeField] private SelectTemplateDialog selectTemplateDialog;
    [SerializeField] private TemplateCategory currentTemplateCategory;
    [SerializeField] public SelectTemplateButton selectTemplateBtn;
    [SerializeField] public TemplateData currentTemplate;
    [SerializeField] private QuestionData currentQuestion;
    [SerializeField] private RectTransform templateHolder;
    [SerializeField] private Button openPanel;
    [SerializeField] private GameObject panelPopUp;
    [SerializeField] private Button addQuizFiled;
    [SerializeField] private int tempTemplatId;

    public void Start()
    {
        openPanel.onClick.AddListener(OpenPanel);
        selectTemplateDialog.OnSubmitTemplate += OnTemplateSelected;
        addQuizFiled.onClick.AddListener(QuizFieldsMaxGenerate);
        
    }
    public void OnQuestionSelected(QuestionData inQuestion)
    {
        currentQuestion = inQuestion;
        //update fields properly (question, template, sub question)
        
        
        
        //review 
        // inQuestion.mainQst = questionFields.mainQuestionText.text;
        // inQuestion.helpQst = questionFields.helpQuestionText.text;
        // inQuestion.subQst = questionFields.subQuestionText.text;
        // inQuestion.templateId = questionFields.templateBtn.Data.id;
        //
        // Debug.Log("ddd"+inQuestion.mainQst+"" +inQuestion.helpQst+""+ inQuestion.subQst+""+inQuestion.templateId );
    }
    public void OnTemplateSelected(TemplateData inTemplate)
    {

        //update template button
        currentTemplate = inTemplate;
        currentQuestion.templateId = currentTemplate.id;
        selectTemplateBtn.templateIcon.sprite = currentTemplate.icon;
        selectTemplateBtn.templateNameTxt.text = currentTemplate.templateName.ToString();
        minFieldsTxt.text = "Min Fiddles : "+currentTemplate.minFields;
        GenerateTemplateFields();
        
        
        Debug.Log(currentTemplate.templateName);
    }
    private void GenerateTemplateFields()
    {
        RemoveDataTemplate();
        addQuizFiled.gameObject.SetActive(true);
        for (var i = 0; i < currentTemplate.minFields; i++)
        {
            Debug.Log(currentTemplate.GetQuizFieldType(i));
            var data = new QuizFieldData();
            var quizFieldType = currentTemplate.GetQuizFieldType(i);
            var quizField = QuizFieldsHandler.GetQuizField(quizFieldType);
            quizField.Initialize();
            quizField.BindData(data);
            currentQuestion.quizFields.Add(data);
            Instantiate(quizField,templateHolder);
            tempTemplatId = i;
        }
        
    }
    private void QuizFieldsMaxGenerate()
    {
        if (templateHolder.GetChildren().Count >= currentTemplate.maxFields) return;
        Debug.Log(currentTemplate.GetQuizFieldType(tempTemplatId));
        var data = new QuizFieldData();
        var quizFieldType = currentTemplate.GetQuizFieldType(tempTemplatId);
        var quizField = QuizFieldsHandler.GetQuizField(quizFieldType);
        quizField.Initialize();
        quizField.BindData(data);
        currentQuestion.quizFields.Add(data);
        Instantiate(quizField,templateHolder);

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