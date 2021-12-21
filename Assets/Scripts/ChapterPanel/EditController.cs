using System.Collections.Generic;
using EditorMenu;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class EditController : MonoBehaviour
    {
        public static EditController instance; 
        
        [SerializeField] private Button addExBtn;
        [SerializeField] public Transform exerciseHolder;
        [SerializeField] private Text chapterName;
        [SerializeField] private Text levelName;
        [SerializeField] private Text subjName;
        [SerializeField] public GameObject mainContent;
        [SerializeField] public InputField mainQst;
        [SerializeField] public InputField subQst;
        private InputField templateId;
        private InputField situationData;
        
        public  List<ExerciseBtn> currentExList;
        public  List<QuestionBtn> currentQstList;

        private ExerciseData _data;
        private ExerciseData Data => _data;
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
            var newExBtn = PoolSystem.instance.Spawn<ExerciseBtn>(ObjectToPoolType.Exercise);
            newExBtn.Initialize();
            newExBtn.transform.SetParent(exerciseHolder.transform);
            newExBtn.transform.localScale = Vector3.one;
            
            Data.chapterId = EditorButtonsManager.instance._selectedChapter.Data.id;
            Data.questions = new List<QuestionData>();
            
            newExBtn.BindData(Data);
            GameDataManager.instance.SaveToJson();
        }
        public void AddNewQst()
        {
             qstData = new QuestionData();
             var newQst = PoolSystem.instance.Spawn<QuestionBtn>(ObjectToPoolType.Question);

             newQst.UpdateName();
             newQst.Initialize(newQst);
            // newQst.transform.SetParent();
             currentQstList.Add(newQst);
            
             LoadData();
             
             newQst.BindData(qstData);
             GameDataManager.instance.SaveToJson();
        }
        private void LoadData()
        {
            qstData.mainQst =mainQst.text;
            qstData.subQst= subQst.text;
            qstData.templateId=int.Parse(templateId.text);
            qstData.situationData=situationData.text;
        }
        
    }
}
