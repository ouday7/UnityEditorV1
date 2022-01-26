using EditorMenu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace ChapterPanel
{
    public class ExerciseBtn : PoolableObject
    {
        [SerializeField] private Button showQstsBtn;
        [SerializeField] private Button addQstBtn;
        [SerializeField] private Text titleTxt;
        [SerializeField] private  DragComponent dragComponent;
        [SerializeField] private CustomGridLayout qstHolder;
        [SerializeField] private RectTransform header;
        
        private ExerciseBtn _targetBtn;
        private Color _startColor;
        private Color _endColor;
        private bool _isDragging;
        private bool _removable;
        private bool _changePos;
        private RectTransform _rt;
        private float _startSize;
        private int _endIndex;
        private const string ExName = "  تمرين  ";
        private Vector2 _startPos;
        private ExerciseData _data;
        private bool _updateSize;
        private int qstHeight = 85;
        private int holderWeight = 400;
        private bool _isInitialised;
        
        public ExerciseData Data => _data;
        private CustomGridLayout QstHolder => qstHolder;

        private RectTransform RectTransform
        {
            get
            {
                if (_rt == null) _rt = GetComponent<RectTransform>();
                return _rt;
            }
        }
        
        public void Initialize()
        {
            if (_isInitialised) return;
            InitializeDrag();
            _isInitialised = true;
            addQstBtn.onClick.AddListener(AddQuestion);
            showQstsBtn.onClick.AddListener(DisplayQuestions);
            _startSize = RectTransform.sizeDelta.y;
        }

        private void AddQuestion() => MenuController.instance.AddNewQst(this);

        private void InitializeDrag()
        {
            dragComponent.OnBeginDragCallback += OnStartDrag;
            dragComponent.OnDragCallback += OnDrag;
            dragComponent.OnEndDragCallback += OnEndDrag;
        }

        public void BindData(ExerciseData newExerciseData)
        {
            titleTxt.text = ExName + (transform.GetSiblingIndex()+1);
            _data = newExerciseData;
            Data.chapterId = newExerciseData.chapterId;
            Data.questions = newExerciseData.questions;
        }
        private void DisplayQuestions()
        {
            var nbChild = qstHolder.transform.childCount;
            if (qstHolder.gameObject.activeInHierarchy)
            {
                Hide();
                return;
            }

            Show(nbChild);
        }

        private void Show(int nbChild)
        {
            if (nbChild == 0) return;

            UpdateQuestionHolderSize();
            MenuController.instance.UpdateExercisesHolderSize();
            qstHolder.gameObject.SetActive(true);
        }

        private void Hide()
        {
            qstHolder.gameObject.SetActive(false);
            RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x,
                _startSize);
            MenuController.instance.UpdateExercisesHolderSize();
        }

        private void Remove()
        {
            var toDeleteBtn = Transform.GetComponent<ExerciseBtn>();
            var nbQuestions = qstHolder.transform.childCount;
            Debug.Log(" nb Qts = "+nbQuestions);
            while (nbQuestions > 0)
            {
                Destroy(qstHolder.transform.GetChild(0).gameObject);
                nbQuestions--;
            }
            PoolSystem.instance.DeSpawn(Transform);
            MenuController.instance.UpdateExercisesHolderSize();
            UpdateQuestionHolderSize();
            UpdateExerciseSize();
            MenuController.instance.currentExList.Remove(toDeleteBtn);
            GameDataManager.instance.Data.exercises.Remove(toDeleteBtn.Data);
            GameDataManager.instance.SaveToJson();
            RenameExercises();
            MenuController.instance.UpdateLayout();
        }

        private void RenameExercises()
        {
            var exercicseNumber = 1;
            foreach (Transform ex in MenuController.instance.ExerciseHolder.transform)
            {
                ex.GetComponent<ExerciseBtn>().titleTxt.text = ExName+exercicseNumber;
                exercicseNumber++;
            }
        }

        private void RenameQuestions()
        {
            var qstNumber = 1;
            foreach (Transform qst in qstHolder.transform)
            {
                qst.GetComponent<QuestionBtn>().btnName.text = QuestionBtn._qstName+qstNumber;
                qstNumber++;
            }
        }

        public void AddQuestionChild(QuestionBtn newQuestionButton, bool updateSize = true)
        {
            newQuestionButton.Transform.SetParent(qstHolder.RectTransform);
            newQuestionButton.Transform.localScale = Vector3.one;
            if(!updateSize) return;
            UpdateQuestionHolderSize();
            qstHolder.UpdateLayout();
        }

        private void UpdateQuestionHolderSize()
        {
            var nbChild = qstHolder.RectTransform.childCount;
            var newSize = new Vector2(holderWeight,  nbChild * qstHeight);
            qstHolder.RectTransform.sizeDelta = newSize;
            qstHolder.UpdateLayout();
            UpdateExerciseSize();
        }

        private void UpdateExerciseSize()
        {
            var newHeight = qstHolder.RectTransform.sizeDelta.y +
                            header.sizeDelta.y;
            RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, newHeight);
        }

        public void ExpandQuestions()
        {
            UpdateExerciseSize();
            qstHolder.gameObject.SetActive(true);
        }

        public void DeleteQuestion(QuestionBtn questionBtn)
        {
            Data.questions.Remove(questionBtn.Data);
            MenuController.instance.RemoveQuestion(questionBtn);
            RenameQuestions();
            MenuController.instance.UpdateExercisesHolderSize();
            UpdateQuestionHolderSize();
            UpdateExerciseSize();
            MenuController.instance.UpdateLayout();
            GameDataManager.instance.SaveToJson();
        }
        
        //drag logic

        private void OnStartDrag(PointerEventData eventData)
        {
            _startPos = transform.position;
            Hide();
        }

        private void OnDrag(PointerEventData eventData)
        {
            _isDragging=true;
            var transform1 = transform;
            transform1.position = new Vector2(transform1.position.x, eventData.position.y);
        }

        private void OnEndDrag(PointerEventData eventData)
        {
            if (_removable)
            {
                Remove();
            }
            else if (_changePos)
            {
                var x = _targetBtn.transform.GetSiblingIndex();
                _targetBtn.transform.SetSiblingIndex(transform.transform.GetSiblingIndex());
                transform.transform.SetSiblingIndex(x);
            }
            else transform.position = _startPos;
            
            _isDragging = false;
            MenuController.instance.ExerciseHolder.UpdateLayout();
        }
        
        //collision logic
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Remove") && _isDragging)
            {
                header.GetComponent<Image>().color = Color.red;
                _removable = true;
            }

            else if (other.gameObject.CompareTag("GroupQuestionMenuBtn"))
            {
                header.GetComponent<Image>().color = Color.yellow;
                other.transform.GetComponent<ExerciseBtn>().header.GetComponent<Image>().color = Color.white;
                _changePos = true;
                _targetBtn = other.GetComponent<ExerciseBtn>();
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Remove") )
            {
                header.GetComponent<Image>().color=Color.white;
                _removable = false;
            }

            if (!other.gameObject.CompareTag("GroupQuestionMenuBtn")) return;
            header.GetComponent<Image>().color=Color.white;
            _changePos = false;
        }

        public float GetHeight() => RectTransform.sizeDelta.y;
    }
}
