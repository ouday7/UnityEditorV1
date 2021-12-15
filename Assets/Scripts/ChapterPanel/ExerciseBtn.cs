using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class ExerciseBtn : PoolableObject
    {
        [SerializeField] private Text btnName;

        private bool _isInitialized;
        private static int _exerciseNbr=1;
        private const string _exName = "  تمرين  ";
        
        public  void Initialize()
        {
            if(_isInitialized) return;
            this.btnName.text = _exName + _exerciseNbr;
            _exerciseNbr++;
            _isInitialized = true;
        }
    }
}
