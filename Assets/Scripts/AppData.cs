using System;
using System.Collections.Generic;

[Serializable] public class LevelData
{
    public int id;
    public string name;
    public int order;
    public List<int> subjectsId;
    public Dictionary<int, SubjectData> subjects;
}

[Serializable] public class SubjectData
{
    public int id;
    public string name;
    public int order;
    public List<ChapterData> chapters;
}

[Serializable] public class ChapterData
{
    public int id;
    public string name;
    public int order;
    public int subjectId;
    public int levelId;
}

[Serializable] public class JsonData
{
    public List<LevelData> levels;
    public List<SubjectData> subjects;
    public List<ChapterData> chapters;
}

[Serializable]
public class QuizFieldData
{
    public int id;
    public string type;
}

[Serializable] public class QuestionData
{
    public string mainQst;
    public string subQst;
    public List<QuizFieldData> quizFields;
    public int templateId;
    public string situationData;
}
[Serializable] public class ExerciseData
{
    public int chapterId;
    public List<QuestionData> questions;
}

[Serializable] public class TemplateData
{
    public int templateId;
    public string templateName;
}

    
