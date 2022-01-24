using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using EditorMenu;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class MenuController : EntryPointSystemBase
    {
        //todo separate Add Question => change size from Remove Question and Generate Questions 
        public static MenuController instance; //todo: remove instance
        
        [SerializeField] private Button addExBtn;
        [SerializeField] private CustomGridLayout exerciseHolder;
        [SerializeField] private Text chapterName;
        [SerializeField] private Text levelName;
        [SerializeField] private Text subjName;
        [SerializeField] public RectTransform mainContent;
        // [SerializeField] private int templateId;
        //[SerializeField] private int situationData;
        
        [NonSerialized]public  List<ExerciseBtn> currentExList;
        [NonSerialized]public  List<QuestionBtn> currentQstList;
        private ExerciseData Data;
        private QuestionData QstData;
        private bool _isInitialised;
        private bool _editing;
        public CustomGridLayout ExerciseHolder => exerciseHolder;

        public override void Begin()
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
            mainContent.DOAnchorPos(new Vector2(0.5f, 1250), 0.35f);
            chapterName.text = GameDataManager.instance.GetSelectedChapter().name;
            levelName.text = GameDataManager.instance.GetSelectedLevel().name;
            subjName.text = GameDataManager.instance.GetSelectedSubject().name;
        }
        
        private void AddExercise()
        {
            Data = new ExerciseData();
            var newExBtn = PoolSystem.instance.Spawn<ExerciseBtn>(ObjectToPoolType.Exercise);
            newExBtn.transform.SetParent(exerciseHolder.transform);
            newExBtn.Initialize();
            Data.chapterId = GameDataManager.instance.GetSelectedChapter().id;
            Data.questions = new List<QuestionData>();
            newExBtn.BindData(Data);
            currentExList.Add(newExBtn);
            GameDataManager.instance.Data.exercises.Add(Data);
            GameDataManager.instance.SaveToJson();
            UpdateExercisesHolderSize();
            
            UpdateLayout();
        }
        public void AddNewQst(ExerciseBtn inExerciseBtn)
        {
            QstData = new QuestionData();
            var newQuestionButton = PoolSystem.instance.Spawn<QuestionBtn>(ObjectToPoolType.Question);
            inExerciseBtn.AddQuestionChild(newQuestionButton); 
            newQuestionButton.UpdateName();
            newQuestionButton.Initialize(inExerciseBtn);
            newQuestionButton.BindData(QstData);
            inExerciseBtn.Data.questions.Add(QstData);
            currentQstList.Add(newQuestionButton);
            
            UpdateExercisesHolderSize();
            inExerciseBtn.ExpandQuestions();
            GameDataManager.instance.SaveToJson();
            
            UpdateLayout();
        }
        
        public void UpdateExercisesHolderSize()
        {
            UpdateHolderSize();
        }

        private void UpdateHolderSize()
        {
            var nb = exerciseHolder.transform.childCount;
            var totalElementsHeight = currentExList.Sum(exerciseBtn => exerciseBtn.GetHeight());
            var exHolderSize = exerciseHolder.RectTransform.sizeDelta;
            var newHeight = totalElementsHeight + (nb - 1) * exerciseHolder.spacing.y + exerciseHolder.padding.y * 2;
            exerciseHolder.RectTransform.sizeDelta = new Vector2(exHolderSize.x, newHeight);
            DelayedUpdateLayout();
        }
        public void UpdateLayout() => exerciseHolder.UpdateLayout();

        private void DelayedUpdateLayout()
        {
            Invoke(nameof(UpdateLayout), .01f);
        }

        public void RemoveQuestion(QuestionBtn questionBtn)
        {
            currentQstList.Remove(questionBtn);
            PoolSystem.instance.DeSpawn(questionBtn.Transform);
        }
    }
}
