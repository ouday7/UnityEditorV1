using System.Collections;
using System.Collections.Generic;
using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

public abstract class TemplateBase : MonoBehaviour
{
     public Text mainQuestionTxt;
     public Text subQuestionTxt;
     public Button resultBtn;
    
    public abstract void Initialize(QuestionData question);
    public abstract void SetData();
    public abstract bool GetResult();
    public abstract void ResetTemplate();
    public abstract void OnDestroy();
}
