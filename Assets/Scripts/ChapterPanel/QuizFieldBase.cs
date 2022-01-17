using ChapterPanel;
using UnityEngine;

public abstract class QuizFieldBase : MonoBehaviour

{
    protected QuizFieldData _data;
    public abstract void Initialize();
    public abstract void BindData(QuizFieldData inData);
}