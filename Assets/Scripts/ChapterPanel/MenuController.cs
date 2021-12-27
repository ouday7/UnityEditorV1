using System.Collections.Generic;
using EditorMenu;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class MenuController : MonoBehaviour
    {
        public static MenuController Instance;
        [SerializeField] private Button addExBtn;
        [SerializeField] private Transform exerciseHolder;
        [SerializeField] private Text chapterName;
        [SerializeField] private Text levelName;
        [SerializeField] private Text subjName;
        [SerializeField] public GameObject mainContent;

        public  List<ExerciseBtn> currentExList;
        public  List<QuestionBtn> currentQstList;
        
        public JsonData allData;
        private ExerciseData Data { get; set; }
        
        public ExerciseBtn currentExbtn;
        public QuestionData qstData;

        private static int exId;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            exId = 1;
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
            
            Data.chapterId = EditorButtonsManager.Instance.selectedChapter.Data.id;
            Data.exerciseId = exId;
            exId++;
            Data.questions = new List<QuestionData>();

            newExBtn.BindData(newExBtn._data);
            
            GameDataManager.Instance.SaveToJson();

            Debug.Log(Data.questions);
            allData.exercises.Add(Data); //no assgin
            // Data.Add(new ExerciseData());
            allData.exercises.Add(Data);
          
            // Debug.Log(newExBtn._data.questions);
            // GameDataManager.instance.SaveToJson();
            
            
         Debug.Log($"Taille : +{allData.exercises.Count}");   
        }
        public void AddNewQst(Button addqstBtn)
        {
             var newQst = PoolSystem.instance.Spawn<QuestionBtn>(ObjectToPoolType.Question);
             newQst.UpdateName();
             newQst.transform.SetParent(exerciseHolder);
            // newQst.transform.SetParent(this.transform);
             currentExbtn = addqstBtn.transform.parent.GetComponent<ExerciseBtn>();
             currentQstList.Add(newQst);
             newQst.BindData(qstData);
             
             // onclick qstBtn or On Delete it
             newQst.Initialize(newQst);
        }
    }
}
