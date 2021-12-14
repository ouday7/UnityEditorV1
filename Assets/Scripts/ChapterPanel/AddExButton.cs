using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class AddExButton : MonoBehaviour
    {
        [SerializeField] private Button addExbtn;
        [SerializeField] private Transform exerciseHolder;

        private ExerciseData _data;
        public ExerciseData Data => _data;
        
        private List<Transform> _currentExercisesList;
        

        public void Start()
        {
            addExbtn.onClick.AddListener( AddExercise);
        }

        private void AddExercise()
        {
            var newExBtn=PoolSystem.instance.Spawn<ExerciseBtn>(ObjectToPoolType.Exercise);
            newExBtn.Initialize();
            newExBtn.transform.localScale = Vector3.one;
            newExBtn.transform.SetParent(exerciseHolder.transform);
        }
        
    }
}
