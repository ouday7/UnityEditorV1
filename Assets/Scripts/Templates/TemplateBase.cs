using System;
using System.Collections;
using System.Collections.Generic;
using ChapterPanel;
using UnityEngine;

public abstract class TemplateBase : MonoBehaviour
{
    
    public abstract void Initialize(QuestionData qts,Action action);
    public abstract void SetData();
    public abstract bool GetResult();
    public abstract void ResetTemplate();
    public abstract void OnDestroy();
}
