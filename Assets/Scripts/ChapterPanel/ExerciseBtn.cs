using Envast.Layouts;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class ExerciseBtn : PoolableObject
    {

        [SerializeField] private Text titleTxt;
        [SerializeField] private Button addQuestions;
        [SerializeField] private Button showElement;
        [SerializeField] private Text order;
        
        private ExerciseBtn _targetBtn;
        private Color _startColor;
        private Color _endColor;
        
        
        private bool isDraggable;
        private bool isDragging;
        
        private int _startIndex;
        private int _endIndex;
        
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
            var transform1 = transform;
            transform1.position = new Vector2(transform1.position.x, pos.y);
        }
        public void OnEndDrag()
        {
            if (this._targetBtn != null)
            {
                PermutationOrder();
                _startColor=Color.black;
            }
        
            else
            {
                transform.SetSiblingIndex(_startIndex);
            
            }
        
            
           
        }
        private void PermutationOrder()
        {
          
            
            
        }
        private void Remove()
        {
        
                PoolSystem.instance.DeSpawn(this.transform);
                _exerciseNbr--;
                ExerciseController.instance.currentExList.Remove(transform.GetComponent<ExerciseBtn>());
           
    
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Remove"))
            {
                _isRemove = true;
                Remove();
            };
            if (!other.gameObject.CompareTag("GroupQuestionMenuBtn")) return;
            this._targetBtn = other.GetComponent<ExerciseBtn>();
            Remove();

        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Remove"))
            {
                _isRemove = false;
                Remove();

            };
            if (!other.gameObject.CompareTag("GroupQuestionMenuBtn")) return;
            this._targetBtn = null;
        }
    }
    
    
 
}
