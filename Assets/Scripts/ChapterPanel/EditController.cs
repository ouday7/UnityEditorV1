using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class EditController : MonoBehaviour
    {
        [SerializeField] private Button addExBtn;
        [SerializeField] public Transform exerciseHolder;
        [SerializeField] private ExerciseController exercise;

        public static EditController instance; 
        private void Awake()
        {
            if (instance != null) return;
            instance = this;
            
            exercise.Begin();
            addExBtn.onClick.AddListener(ExerciseController.instance.AddExercise);
        }
        
    }
}
