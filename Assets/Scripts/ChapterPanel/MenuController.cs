using System;
using System.Collections.Generic;
using System.Linq;
using Components.GridLayout;
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
        [SerializeField] private InputField mainQuestion;
        [SerializeField] private InputField subQuestion;
        // [SerializeField] private int templateId;
        //[SerializeField] private int situationData;
        [SerializeField] private Button saveBtn;
        
        [NonSerialized]public  List<ExerciseBtn> currentExList;
        [NonSerialized]public  List<QuestionBtn> currentQstList;
        private RectTransform _rt;
        private ExerciseData Data;
        private QuestionData QstData;
        private bool _isInitialised;
        public GameObject ExerciseHolder => exerciseHolder;

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
            currentExList = new List<ExerciseBtn>();
            currentQstList=new List<QuestionBtn>();
            QuestionBtn.OnClickQuestion += ClickQuestion;
            addExBtn.onClick.AddListener(AddExercise);
            mainContent.gameObject.SetActive(false);
            chapterName.text = PlayerPrefs.GetString("chapterName");
            levelName.text=PlayerPrefs.GetString("levelName");
            subjName.text = PlayerPrefs.GetString("subjectName");
        }

        private void ClickQuestion(QuestionBtn qstBtn)
        {
            mainQuestion.text = "";
            subQuestion.text="";
            
            if (qstBtn.Data.mainQst.Length > 0)
            {
                mainQuestion.text = qstBtn.Data.mainQst;
                subQuestion.text = qstBtn.Data.subQst;
                
            }
            saveBtn.onClick.AddListener(() =>
            {
                qstBtn.Data.mainQst = mainQuestion.text;
                qstBtn.Data.subQst = subQuestion.text;
                GameDataManager.instance.SaveToJson();
            });
            
        }
        private void AddExercise()
        {
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
            QstData = new QuestionData();
            var newQuestionButton = PoolSystem.instance.Spawn<QuestionBtn>(ObjectToPoolType.Question);
            newQuestionButton.UpdateName();
            inExerciseBtn.AddQuestionChild(newQuestionButton);
            newQuestionButton.Initialize();
            newQuestionButton.BindData(QstData);
            inExerciseBtn.Data.questions.Add(QstData);
            currentQstList.Add(newQuestionButton);
            inExerciseBtn.MaximiseExerciseSize(inExerciseBtn.QstHolder);
            inExerciseBtn.transform.Find("Panel").gameObject.SetActive(true);
            GameDataManager.instance.SaveToJson();
            UpdateExercisesHolderSize(1);
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
