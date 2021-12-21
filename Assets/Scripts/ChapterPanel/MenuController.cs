﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class MenuController : MonoBehaviour
    {
        public static MenuController instance; 
        
        [SerializeField] private Button addExBtn;
        [SerializeField] public Transform exerciseHolder;
        [SerializeField] private Text chapterName;
        [SerializeField] private Text levelName;
        [SerializeField] private Text subjName;
        [SerializeField] public GameObject mainContent;

        public  List<ExerciseBtn> currentExList;
        public  List<QuestionBtn> currentQstList;

        private ExerciseData Data { get; set; }

        public QuestionData qstData;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            
            Begin();
        }
        private void Begin()
        {
            addExBtn.onClick.AddListener(AddExercise);
            mainContent.gameObject.SetActive(false);
            chapterName.text = PlayerPrefs.GetString("chapterName");
            levelName.text=PlayerPrefs.GetString("levelName");
            subjName.text = PlayerPrefs.GetString("subjectName");
        }

        private void AddExercise()
        {
            Data = new ExerciseData();
            var newExBtn = PoolSystem.instance.Spawn<ExerciseBtn>(ObjectToPoolType.Exercise);
            newExBtn.Initialize();
            newExBtn.transform.SetParent(exerciseHolder.transform);
            newExBtn.transform.localScale = Vector3.one;
            newExBtn.BindData(Data);
        }
        public void AddNewQst()
        {
             var newQst = PoolSystem.instance.Spawn<QuestionBtn>(ObjectToPoolType.Question);
             newQst.UpdateName();
             newQst.transform.SetParent(exerciseHolder.transform);
             currentQstList.Add(newQst);
             newQst.BindData(qstData);
             
             // onclick qstBtn or On Delete it
             newQst.Initialize(newQst);
        }
    }
}
