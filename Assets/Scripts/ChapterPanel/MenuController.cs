﻿using System;
using System.Collections.Generic;
using System.Linq;
using EditorMenu;
using Envast.Layouts;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class MenuController : MonoBehaviour
    {
        public static MenuController instance; 
        
        [SerializeField] private Button addExBtn;
        [SerializeField] private GameObject exerciseHolder;
        [SerializeField] private Text chapterName;
        [SerializeField] private Text levelName;
        [SerializeField] private Text subjName;
        [SerializeField] public GameObject mainContent;
        // [SerializeField] private int templateId;
        //[SerializeField] private int situationData;
        
        [NonSerialized]public  List<ExerciseBtn> currentExList;
        [NonSerialized]public  List<QuestionBtn> currentQstList;
        private RectTransform _rt;
        private ExerciseData Data;
        private QuestionData QstData;
        private bool _isInitialised;
        private bool _editing;
        public GameObject ExerciseHolder => exerciseHolder;

        private void Awake()
        {
            if (instance != null) return;
            instance = this;
            
            Initialize();
        }
        private void Initialize()
        {
            currentExList = new List<ExerciseBtn>();
            currentQstList=new List<QuestionBtn>();
            
            addExBtn.onClick.AddListener(AddExercise);
            mainContent.gameObject.SetActive(false);
            chapterName.text = PlayerPrefs.GetString("chapterName");
            levelName.text=PlayerPrefs.GetString("levelName");
            subjName.text = PlayerPrefs.GetString("subjectName");
        }
        
        private void AddExercise()
        {
            // Spawn and prepare exercise data
            Data = new ExerciseData();
            var newExBtn = PoolSystem.instance.Spawn<ExerciseBtn>(ObjectToPoolType.Exercise);
            newExBtn.Initialize();
            newExBtn.transform.SetParent(exerciseHolder.transform);
            newExBtn.transform.localScale = Vector3.one;
            Data.chapterId = PlayerPrefs.GetInt("chapterId");
            Data.questions = new List<QuestionData>();
            newExBtn.BindData(Data);
            currentExList.Add(newExBtn);
            GameDataManager.instance.Data.exercises.Add(Data);
            GameDataManager.instance.SaveToJson();
            UpdateExercisesHolderSize(1);
        }
        public void AddNewQst(ExerciseBtn inExerciseBtn)
        {
            // Spawn and prepare question data
            QstData = new QuestionData();
            var newQuestionButton = PoolSystem.instance.Spawn<QuestionBtn>(ObjectToPoolType.Question);
            inExerciseBtn.AddQuestionChild(newQuestionButton); // add qst & update qsts holder size
            newQuestionButton.UpdateName();
            newQuestionButton.Initialize(inExerciseBtn);
            newQuestionButton.BindData(QstData);
            inExerciseBtn.Data.questions.Add(QstData);
            currentQstList.Add(newQuestionButton);
            
            // update sizes & graphics
            UpdateExercisesHolderSize(1);
            inExerciseBtn.ExpandQuestions();
            GameDataManager.instance.SaveToJson();
        }

        private RectTransform RectTransform
        {
            get
            {
                if (_rt == null) _rt = GetComponent<RectTransform>();
                return _rt;
            }
        }
        public void UpdateExercisesHolderSize(int nb)
        {
            var exHolderSize=exerciseHolder.GetComponent<RectTransform>().sizeDelta;
            exerciseHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(exHolderSize.x, exHolderSize.y + (nb*100));
        }
    }
}
