using EditorMenu;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class ExerciseBtn : PoolableObject
    {
        [SerializeField] private Button showQstsBtn;
        [SerializeField] private Button addQstBtn;
        [SerializeField] private GameObject exScrollRect;
        [SerializeField] private Text titleTxt;
        [SerializeField] private DraggBtn dragBtn;

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
        public  void Initialize()
        {
            if(_isInitialized) return;
            this.titleTxt.text = _exName + _exerciseNbr;
            _exerciseNbr++;
            _isInitialized = true;
            dragBtn.Initialize(this);
            
            addQstBtn.onClick.AddListener(() =>
            {
                MenuController.instance.AddNewQst(addQstBtn);
            });
            showQstsBtn.onClick.AddListener(ShowQsts);
        }

        public void BindData(ExerciseData newExerciseData)
        {
            _data = newExerciseData;
            _data.chapterId = newExerciseData.chapterId;
            _data.questions = newExerciseData.questions;
        }
        
        private void ShowQsts()
        {
            if (exScrollRect.activeInHierarchy)
            {
                exScrollRect.SetActive(false);
                return;
            }
            exScrollRect.SetActive(true);
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
            MenuController.instance.currentExList.Remove(transform.GetComponent<ExerciseBtn>());
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Remove"))
            {
                transform.GetComponent<Image>().color=Color.red;
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
