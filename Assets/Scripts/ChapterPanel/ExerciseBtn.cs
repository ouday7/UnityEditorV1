using EditorMenu;
using Envast.Components.GridLayout;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class ExerciseBtn : PoolableObject
    {
        [SerializeField] private Button showQstsBtn;
        [SerializeField] private Button addQstBtn;
        [SerializeField] private Text titleTxt;
        [SerializeField] private DraggBtn dragBtn;
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
        private bool _isInitialized;
        private const string _exName = "  تمرين  ";
        private Vector2 startPos;
        private ExerciseData _data;
        private bool updateSize;
        private int qstHeight = 85;
        private int holderWeight = 400;
        public ExerciseData Data => _data;
        public CustomGridLayout QstHolder => qstHolder;
        
        public  void Initialize()
        {
            if(_isInitialized) return;
            this.titleTxt.text = _exName + (transform.GetSiblingIndex()+1);
            _isInitialized = true;
            dragBtn.Initialize(this);
            
            addQstBtn.onClick.AddListener(() =>
            {
                MenuController.instance.AddNewQst(this);
            });
            showQstsBtn.onClick.AddListener(ShowQuestions);
            _startSize = RectTransform.sizeDelta.y;
        }
        public void BindData(ExerciseData newExerciseData)
        {
            _data = newExerciseData;
            Data.chapterId = newExerciseData.chapterId;
            Data.questions = newExerciseData.questions;
        }
        private void ShowQuestions()
        {
            var nbChild = qstHolder.transform.childCount;
            if (qstHolder.gameObject.activeInHierarchy)
            {
                qstHolder.gameObject.SetActive(false);
                RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x,
                    _startSize);
                MenuController.instance.UpdateExercisesHolderSize(-nbChild);
                return;
            }

            if (nbChild == 0) return;
            MenuController.instance.UpdateExercisesHolder();
            MaximiseHolderSize();
            MenuController.instance.UpdateExercisesHolderSize(nbChild);
            QstHolder.UpdateLayout();
            qstHolder.gameObject.SetActive(true);
            var newHeight = qstHolder.RectTransform.sizeDelta.y +
                            header.sizeDelta.y;
            RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, newHeight);

        }
        
        public void OnStartDrag()
        {
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
            var toDeleteBtn = transform.GetComponent<ExerciseBtn>();
            PoolSystem.instance.DeSpawn(transform);
            MenuController.instance.UpdateExercisesHolderSize(-1);
            MaximiseHolderSize();
            MaximiseExerciseSize();
            MenuController.instance.currentExList.Remove(toDeleteBtn);
            GameDataManager.instance.Data.exercises.Remove(toDeleteBtn.Data);
            GameDataManager.instance.SaveToJson();
            RenameExercises();
            MenuController.instance.UpdateExercisesHolder();
        }

        private void RenameExercises()
        {
            var exercicseNumber = 1;
            foreach (Transform ex in MenuController.instance.ExerciseHolder.transform)
            {
                ex.GetComponent<ExerciseBtn>().titleTxt.text = _exName+exercicseNumber;
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Remove") && _isDragging)
            {
                header.GetComponent<Image>().color = Color.red;
                _removable = true;
            }

            else if (other.gameObject.CompareTag("GroupQuestionMenuBtn"))

            {
                this.header.GetComponent<Image>().color = Color.yellow;
                other.transform.Find("Header").GetComponent<Image>().color = Color.white;
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

        public void AddQuestionChild(QuestionBtn newQuestionButton)
        {
            newQuestionButton.transform.SetParent(qstHolder.RectTransform);
            MaximiseHolderSize();
            qstHolder.UpdateLayout();
        }

        private RectTransform RectTransform
        {
            get
            {
                if (_rt == null) _rt = GetComponent<RectTransform>();
                return _rt;
            }
        }
        public void MaximiseHolderSize()
        {
            var nbChild = qstHolder.RectTransform.childCount;
            var newSize = new Vector2(holderWeight,  nbChild * qstHeight);
            qstHolder.RectTransform.sizeDelta = newSize;
            qstHolder.UpdateLayout();
        }
        public void MaximiseExerciseSize()
        {
            var newHeight = qstHolder.RectTransform.sizeDelta.y +
                            header.sizeDelta.y;
            RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, newHeight);
        }

        public void ExpandQuestions()
        {
            MaximiseExerciseSize();
            qstHolder.gameObject.SetActive(true);
        }

        public void DeleteQuestion(QuestionBtn questionBtn)
        {
            PoolSystem.instance.DeSpawn(questionBtn.Transform);
            MenuController.instance.currentQstList.Remove(questionBtn);
            Data.questions.Remove(questionBtn.Data);
            GameDataManager.instance.SaveToJson();
            RenameQuestions();
            MenuController.instance.UpdateExercisesHolderSize(-1);
            MaximiseHolderSize();
            MaximiseExerciseSize();
            MenuController.instance.UpdateExercisesHolder();
        }
    }
}
