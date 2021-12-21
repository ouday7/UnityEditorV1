using EditorMenu;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class EditController : MonoBehaviour
    {
        [SerializeField] private Button addExBtn;
        [SerializeField] public Transform exerciseHolder;
        [SerializeField] private ExerciseController exercise;
        [SerializeField] private Text chapterName;
        [SerializeField] private Text levelName;
        [SerializeField] private Text subjName;
        private EditorButtonsManager chapterBtn;
        public static EditController instance; 
        
        private void Awake()
        {
            if (instance != null) return;
            instance = this;
            
            exercise.Begin();
            addExBtn.onClick.AddListener(ExerciseController.instance.AddExercise);
        }
        private void Start()
        {
            chapterName.text = PlayerPrefs.GetString("chapterName");
            levelName.text=PlayerPrefs.GetString("levelName");
            subjName.text = PlayerPrefs.GetString("subjectName");
        }
    }
}
