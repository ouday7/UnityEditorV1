using System.Collections;
using System.Collections.Generic;
using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;

public class TrueFalseTemplateBase : TemplateBase
{
    [SerializeField] private QuestionData questionData;
    [SerializeField] private int templateID;

    
   
    public override void Initialize(QuestionData qts)
    {
        
        
    }
    public override void SetData()
    {
        
    }

    public override bool GetResult()
    {
        throw new System.NotImplementedException();
    }

    public override void ResetTemplate()
    {
        throw new System.NotImplementedException();
    }

    public override void OnDestroy()
    {
        throw new System.NotImplementedException();
    }
}
