using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

namespace ChapterPanel
{
    public class ExerciseBtn : PoolableObject
    {

        [SerializeField] private Text titleTxt;
        [SerializeField] private Button addQuestions;
        private ExerciseBtn _targetBtn;

        
        
        
        private bool _isOpen;
       
        private Color _startColor;
        private int _startIndex;
        private bool _isRemove;

        private bool _isInitialized;
        
        private static int _exerciseNbr=1;
        private const string _exName = "  تمرين  ";
        private ExerciseData _data; 
        public ExerciseData Data => _data;
        
        [SerializeField] private DraggBtn dragBtn;

        
        public  void Initialize()
        {
            if(_isInitialized) return;
            this.titleTxt.text = _exName + _exerciseNbr;
            _exerciseNbr++;
            _isInitialized = true;
            dragBtn.Initialize(this);

        }
        
        public void OnStartDrag()
        {
            
            _startIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
    
        }
        public void OnDrag(Vector3 pos)
        {
            transform.position = new Vector2(transform.position.x, pos.y);
          
        }
        public void OnEndDrag()
        {
            if (this._targetBtn != null)
            {
                Permutation();
            }
        
            else
            {
                transform.SetSiblingIndex(_startIndex);
            
            }
        
            _isRemove = false;
           
        }
        
        private void Permutation()
        {
           
            
        }
    }
}
