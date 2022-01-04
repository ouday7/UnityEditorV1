using System;
using Components.GridLayout;
using EditorMenu;
using Envast.Layouts;
using UnityEditor;
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
        
        private bool _isDragging=false;
        private bool _removable;
        private bool _changePos;
        private RectTransform _rt;
        private float _startSize;
        private int _startIndex;
        private int _endIndex;
        private string fileName = "JsonFile.txt";
        private bool _isInitialized;
        private static int _exerciseNbr=1;
        private const string _exName = "  تمرين  ";
        private Vector2 startPos;
        private int _k=0;
        private ExerciseData _data;
        private bool updateSize;
        private int qstHeight = 85;
        private int holderWeight = 400;
        public ExerciseData Data => _data;
        public CustomGridLayout QstHolder => qstHolder;
        public  void Initialize()
        {
            if(_isInitialized) return;
            this.titleTxt.text = _exName + _exerciseNbr;
            _exerciseNbr++;
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
            MaximiseHolderSize(nbChild);
            MenuController.instance.UpdateExercisesHolderSize(nbChild);
            QstHolder.UpdateLayout();
            qstHolder.gameObject.SetActive(true);
            
            var newHeight = qstHolder.RectTransform.sizeDelta.y +
                            header.sizeDelta.y;
            RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, newHeight);
            
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
            var toDeleteBtn = transform.GetComponent<ExerciseBtn>();
            PoolSystem.instance.DeSpawn(transform);
            _exerciseNbr--;
            MenuController.instance.UpdateExercisesHolderSize(-1);
            MaximiseHolderSize(-1);
            MaximiseExerciseSize(qstHolder);
            MenuController.instance.currentExList.Remove(toDeleteBtn);
            GameDataManager.instance.Data.exercises.Remove(toDeleteBtn.Data);
            GameDataManager.instance.SaveToJson();
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
            MaximiseHolderSize(qstHolder.RectTransform.childCount);
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
        public void MaximiseHolderSize(int nbChild)
        {
            var newSize = new Vector2(holderWeight,  (nbChild * qstHeight));
            qstHolder.RectTransform.sizeDelta = newSize;
        }
        public void MaximiseExerciseSize(CustomGridLayout qslHolder)
        {
            var newHeight = qslHolder.RectTransform.sizeDelta.y +
                            header.sizeDelta.y;
            RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, newHeight);
        }
    }
}
