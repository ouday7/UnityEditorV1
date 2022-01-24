using System.Collections.Generic;
using ChapterPanel;
using Templates;
using UnityEngine;

public class ConnectTextToText : TemplateBase
{
    [SerializeField] private AnswerDrag dragObject;
    [SerializeField] private QuestionConnectTextToText question;
    [SerializeField] private GameObject questionHolder;
    [SerializeField] private GameObject answerHolder;
    private List<AnswerDrag> _dragObjectList;
    private List<QuestionConnectTextToText> _questionBtnList = new List<QuestionConnectTextToText>();
    private bool _result;
    private QuestionData _questionData;


    public override void Initialize()
    {
        
        //check event here 
    }

    public override void BindData(QuestionData inQuestionData)
    {
        _questionData = inQuestionData;
        GenerateAnswer();
        GenerateQuestion();

    }


    private void GenerateAnswer()
    {
        foreach (var t in _questionData.quizFields)
        {
            var newAnswerBtn= Instantiate(dragObject, answerHolder.transform);
            newAnswerBtn.text.text = t.textTwo;
        }
    }
    private void GenerateQuestion()
    {
        foreach (var t in _questionData.quizFields)
        {
            var newQuestionBtn = Instantiate(question, questionHolder.transform);
            newQuestionBtn.btnRtlText.text = t.textA;
            
        }
    }
    public override bool GetResult()
    {
        return _result;
    }
    public override void ResetTemplate()
    {
        
    }
    public override void OnDestroy()
    {
        ResetTemplate();
    }
}