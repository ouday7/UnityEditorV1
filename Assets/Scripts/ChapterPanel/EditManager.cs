﻿using System.Collections.Generic;
using System.Linq;
using ChapterPanel;
using Envast.Components.GridLayout.Helpers;
using UnityEngine;
using UnityEngine.UI;


public class EditManager : MonoBehaviour
{
    [SerializeField] private Text minFieldsTxt;
    [SerializeField] private SelectTemplateDialog selectTemplateDialog;
    [SerializeField] private TemplateCategory currentTemplateCategory;
    [SerializeField] public SelectTemplateButton selectTemplateBtn;
    [SerializeField] public TemplateData currentTemplate;
    [SerializeField] private QuestionData currentQuestion;
    [SerializeField] private RectTransform templateHolder;
    [SerializeField] private Button openPanel;
    [SerializeField] private GameObject panelPopUp;
    [SerializeField] private InputField mainQuestionText;
    [SerializeField] private InputField subQuestionText;
    [SerializeField] private InputField helpQuestionText;
    [SerializeField] private TemplateBtn templateBtn;
    [SerializeField] private TemplateCategory templateCategory;
    [SerializeField] private Button addQuizFiled;
    [SerializeField] private int tempTemplatId;
    
    public void Start()
    {
        openPanel.onClick.AddListener(OpenPanel);
        QuestionBtn.OnClickQuestion += ClickQuestion;
        selectTemplateDialog.OnSubmitTemplate += OnTemplateSelected;
        addQuizFiled.onClick.AddListener(QuizFieldsMaxGenerate);
        
    }

    private void OpenPanel()
    {
        panelPopUp.SetActive(true);
    }

    private void ClickQuestion(QuestionBtn qstBtn)
    {
        currentQuestion = qstBtn.Data;
        mainQuestionText.onEndEdit.RemoveAllListeners();
        subQuestionText.onEndEdit.RemoveAllListeners();
        helpQuestionText.onEndEdit.RemoveAllListeners();
        
        mainQuestionText.onEndEdit.AddListener(delegate {EditMainQuestion(mainQuestionText.text,qstBtn); });
        subQuestionText.onEndEdit.AddListener(delegate {EditSubQuestion(subQuestionText.text,qstBtn); });
        helpQuestionText.onEndEdit.AddListener(delegate{EditHelpQuestion(helpQuestionText.text,qstBtn); });
        
        mainQuestionText.text = qstBtn.Data.mainQst;
        subQuestionText.text = qstBtn.Data.subQst;
        helpQuestionText.text = qstBtn.Data.helpQst;
        if(qstBtn.Data.templateId == 0) return;
        var templateData =
            TemplatesHandler.Instance.templatesData.FirstOrDefault(template => template.id == qstBtn.Data.templateId);
        
        OnTemplateSelected(templateData);
    }

    private void EditMainQuestion(string arg0,QuestionBtn qstBtn)
    {
        qstBtn.Data.mainQst = arg0;
        GameDataManager.instance.SaveToJson();
    }
    private void EditSubQuestion(string arg0,QuestionBtn qstBtn)
    {
        qstBtn.Data.subQst = arg0;
        GameDataManager.instance.SaveToJson();
    }
    private void EditHelpQuestion(string arg0,QuestionBtn qstBtn)
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
        minFieldsTxt.text = "Min Fiddles : "+currentTemplate.minFields;
        GenerateTemplateFields();
    }

    private void GenerateTemplateFields()
    {
        RemoveDataTemplate();
        addQuizFiled.gameObject.SetActive(true);
        if (currentQuestion.quizFields == null)
        {
            Debug.Log("//. Initialize Questions List");
            currentQuestion.quizFields = new List<QuizFieldData>();
        }
        for (var i = 0; i < currentTemplate.minFields; i++)
        {
            Debug.Log(currentTemplate.GetQuizFieldType(i));
            var data = new QuizFieldData();
            var quizFieldType = currentTemplate.GetQuizFieldType(i);
            var quizFieldPrefab = QuizFieldsHandler.GetQuizField(quizFieldType);
            var quizField = Instantiate(quizFieldPrefab, templateHolder);
            quizField.Initialize();
            quizField.BindData(data); 
            currentQuestion.quizFields.Add(data);
            tempTemplatId = i;
        }
    }

    private void RemoveDataTemplate()
    {
        foreach (Transform child in templateHolder) {
            Destroy(child.gameObject);
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
        Instantiate(quizField, templateHolder);
    }

}