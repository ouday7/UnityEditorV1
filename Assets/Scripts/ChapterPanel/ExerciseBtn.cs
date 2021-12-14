using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class ExerciseBtn : PoolableObject
    {
        [SerializeField] private Transform btnPrefab;
        [SerializeField] private Text btnName;
        
        private ExerciseData _data;
        public ExerciseData Data => _data;
        
        private bool _isInitialized;
        private static int _exercicseNbr=1;
        private const string _exName = "  تمرين  ";
        

        public  void Initialize()
        {
            if(_isInitialized) return;
            this.btnName.text = _exName + _exercicseNbr;
            _exercicseNbr++;
            _isInitialized = true;
        }
    }
}
