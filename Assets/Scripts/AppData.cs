using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[Serializable] public class Level
{
    public int id;
    public string name;
    public int order;
    public List<int> subjectsId;
}
[Serializable] public class Subject
{
    public int id;
    public string name;
    public int order;
}
[Serializable] public class Chapter
{
    public int id;
    public string name;
    public int order;
    public int subjectId;
    public int levelId;
}

[Serializable] public class JsonData
{
    public List<Level> levels;
    public List<Subject> subjects;
    public List<Chapter> chapters;
}