using System.Collections.Generic;

[System.Serializable] public class Level
{
    public int id;
    public string name;
    public int order;
}
[System.Serializable] public class Subject
{
    public int id;
    public string name;
    public int order;
    public int levelId;
}
[System.Serializable] public class Chapter
{
    public int id;
    public string name;
    public int order;
    public int subjectId;
    public int levelId;
}
[System.Serializable] public class LevelsData
{
    public Level[] levels;
}
[System.Serializable] public class ChaptersData
{
    public Chapter[] chapters;
}
[System.Serializable] public class SubjectsData
{
    public Subject[] subjects;
}

public class JsonData
{
    public List<Level> levels;
    public List<Subject> subjects;
    public List<Chapter> chapters;
}