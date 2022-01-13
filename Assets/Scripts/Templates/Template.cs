using System.Collections;
using System.Collections.Generic;
using ChapterPanel;
using UnityEngine;

public abstract class Template : MonoBehaviour
{
    
    public abstract void Initialize(QuestionData qts);
    public abstract void SetData();
}
