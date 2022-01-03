﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    [Serializable]
    public class LevelData
    {
        public int id;
        public string name;
        public int order;
        public List<int> subjectsId;
        public Dictionary<int, SubjectData> Subjects;
    }

    [Serializable]
    public class SubjectData
    {
        public int id;
        public string name;
        public int order;
        public List<ChapterData> chapters;
    }

    [Serializable]
    public class ChapterData
    {
        public int id;
        public string name;
        public int order;
        public int subjectId;
        public int levelId;
    }

    [Serializable]
    public class JsonData
    {
        public List<LevelData> levels;
        public List<SubjectData> subjects;
        public List<ChapterData> chapters;
        public List<ExerciseData> exercises;
    }

    [Serializable]
    public class QuizFieldData
    {
        public int id;
        public string type;
        public string textOne;
        public string textTwo;
        public Sprite imageOne;
        public Sprite imageTwo;
        public Button toggleOne;
        public Button toggleTwo;
    }

    [Serializable]
    public class QuestionData
    {
        public int questionId;
        public string mainQst;
        public string subQst;
        public string helpQst;
        public List<QuizFieldData> quizFields;
        public int templateId;
        public string situationData;
    }

    [Serializable]
    public class ExerciseData
    {
        public int chapterId;
        public int exerciseId;
        public List<QuestionData> questions;
    }

    [Serializable]
    public class TemplateData
    {
        public int templateId;
        public string templateName;
    }

    [Serializable]
    public class Categorydata
    {
        public int categoryId;
        public string categoryName;
        public List<TemplateData> template;
    }
}