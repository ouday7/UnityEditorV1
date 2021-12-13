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