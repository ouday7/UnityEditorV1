using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "DataInFormation ", fileName = "Data")]

public class Data : ScriptableObject
{

[Serializable] public class LevelData :ScriptableObject
{
    public int id;
    public string name;
    public int order;
    public List<int> subjectsId;
    public Dictionary<int, SubjectData> subjects;
}

[Serializable] public class SubjectData :ScriptableObject
{
    public int id;
    public string name;
    public int order;
    public List<ChapterData> chapters;
}

[Serializable] public class ChapterData :ScriptableObject
{
    public int id;
    public string name;
    public int order;
    public int subjectId;
    public int levelId;
}

[Serializable] public class JsonData :ScriptableObject
{
    public List<LevelData> levels;
    public List<SubjectData> subjects;
    public List<ChapterData> chapters;
    public List<ExerciseData> exercises;
}

[Serializable]
public class QuizFieldData :ScriptableObject{
    public int id;
    public string type;
    public string textOne;
    public string textTwo;
    public Sprite imageOne;
    public Sprite imageTwo;
    public Button toggleOne;
    public Button toggleTwo;
}

[Serializable] public class QuestionData :ScriptableObject
{
    public int questionId;
    public string mainQst;
    public string subQst;
    public string helpQst;
    public List<QuizFieldData> quizFields;
    public int templateId;
    public string situationData;
}
[Serializable] public class ExerciseData :ScriptableObject
{
    public int chapterId;
    public int exerciseId;
    public List<QuestionData> questions;
}

[Serializable] public class TemplateData :ScriptableObject
{
    public int templateId;
    public string templateName;
}
}
