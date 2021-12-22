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
        
        private bool _isDragging=false;
        private bool _removable;
        private bool _changePos;
        
        
        private int _startIndex;
        private int _endIndex;
        
        private bool _isInitialized;
        private static int _exerciseNbr=1;
        private const string _exName = "  تمرين  ";
        private Vector2 startPos;
     
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
            startPos = transform.position;
        }
        public void OnDrag(Vector3 pos)
        {
            _isDragging=true;
            var transform1 = transform;
            transform1.position = new Vector2(transform1.position.x, pos.y);
        }
        public void OnEndDrag()
        {
            if (_removable)
            {
                Remove();
            }
            else if (_changePos)
            {
                var x = _targetBtn.transform.position;
                _targetBtn.transform.position = startPos;
                transform.position = x;
            }
            else transform.position = startPos;
            _isDragging = false;
        }
        
        private void Remove()
        {
            PoolSystem.instance.DeSpawn(this.transform);
            _exerciseNbr--;
            MenuController.instance.currentExList.Remove(transform.GetComponent<ExerciseBtn>());
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Remove")&& _isDragging)
            {
                transform.GetComponent<Image>().color=Color.red;
                _removable = true;
            }

            if (!other.gameObject.CompareTag("GroupQuestionMenuBtn")) return;
            
            this.transform.GetComponent<Image>().color=Color.yellow;
            other.transform.GetComponent<Image>().color=Color.white;
            _changePos = true;
            _targetBtn = other.GetComponent<ExerciseBtn>();

        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Remove") )
            {
               transform.GetComponent<Image>().color=Color.white;
               _removable = false;
            }

            if (!other.gameObject.CompareTag("GroupQuestionMenuBtn")) return;
            transform.GetComponent<Image>().color=Color.white;
            _changePos = false;
        }
    }
}
