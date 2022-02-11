﻿using System.Collections.Generic;
using ChapterPanel;
using Templates;
using UnityEngine;

public class ConnectTextToText : TemplateBase
{
    [SerializeField] private AnswerDrag dragObject;
    [SerializeField] private QuestionConnectTextToText question;
    [SerializeField] private CustomGridLayout questionHolder;
    [SerializeField] private CustomGridLayout answerHolder;
    private List<AnswerDrag> _dragObjectList;
    private List<QuestionConnectTextToText> _questionBtnList = new List<QuestionConnectTextToText>();
    private bool _result;
    private QuestionData _questionData;
    public QuizFieldData data;

    public override void Initialize()
    {
    }
    
    public override void BindData(QuestionData inQuestionData)
    {
        _questionData = inQuestionData;
        GenerateAnswer(_questionData);
        GenerateQuestion(_questionData);
    }
    
    private void GenerateAnswer(QuestionData questionData)
    {
        foreach (var t in questionData.quizFields)
        {
            var newAnswerBtn= Instantiate(dragObject, answerHolder.transform);
            newAnswerBtn.Initialise(t);
            newAnswerBtn.BindData();
        }
        answerHolder.UpdateLayout();
        
    }
    private void GenerateQuestion(QuestionData questionData)
    {
        foreach (var t in questionData.quizFields)
        {
            var newQuestionBtn = Instantiate(question, questionHolder.transform);
            newQuestionBtn.Initialise(t);
            newQuestionBtn.BindData();
        }
        questionHolder.UpdateLayout();
    }

    public override bool GetResult()
    {
        for (var i = 0; i < questionHolder.RectTransform.childCount; i++)
        {
            for (var j = 0; j < answerHolder.RectTransform.childCount; j++)
            {
                var question = questionHolder.RectTransform.GetChild(i);
                var answer = answerHolder.RectTransform.GetChild(j);


                if (!question.position.Equals(answer.position)) continue;
                if (!question.GetComponent<QuestionConnectTextToText>().data
                    .Equals(answer.GetComponent<AnswerDrag>().data))
                    return false;

            }
        }

        return true;
    }
    public override void ResetTemplate()
    {
        
    }
    public override void OnDestroy()
    {
        ResetTemplate();
    }
}