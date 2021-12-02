using System.Collections.Generic;

[System.Serializable] public class Level
{
    public int id;
    public string name;
    public int order;
    public List<int> subjectsId;
}
[System.Serializable] public class Subject
{
    public int id;
    public string name;
    public int order;
}
[System.Serializable] public class Chapter
{
    public int id;
    public string name;
    public int order;
    public int subjectId;
    public int levelId;
}

public class JsonData
{
    public List<Level> levels;
    public List<Subject> subjects;
    public List<Chapter> chapters;
    
}