using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class ExerciseController : MonoBehaviour
    {
        public static ExerciseController instance;
        
        [SerializeField] private Button showQstsBtn;
        [SerializeField] private Button addQstBtn;
        [SerializeField] private GameObject exScrollRect;
        [SerializeField] private Transform qstHolder;

        private int _qstNbr;
        
        private ExerciseData _data;
        public ExerciseData Data => _data;

        [FormerlySerializedAs("_currentExList")] public List<ExerciseBtn> currentExList;
        public List<QuestionBtn> _currentQstList;
        
        public void Begin()
        {
            if (instance != null && instance != this)
                Destroy(this.gameObject);
            else
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        public void Start()
        {
            currentExList = new List<ExerciseBtn>();
            _currentQstList = new List<QuestionBtn>();
            addQstBtn.onClick.AddListener(() =>
            {
                _qstNbr = addQstBtn.transform.parent.Find("Scroll View").Find("Viewport").Find("Content").childCount;
                //qstNbr = addQstBtn.transform.parent.GetComponent<ScrollRect>().content.childCount;
                AddNewQst();
            });
            showQstsBtn.onClick.AddListener(ShowQsts);
        }

        public void AddExercise()
        {
            var newExBtn = PoolSystem.instance.Spawn<ExerciseBtn>(ObjectToPoolType.Exercise);
            newExBtn.Initialize();
            newExBtn.transform.SetParent(EditController.instance.exerciseHolder.transform);
            newExBtn.transform.localScale = Vector3.one;
            currentExList.Add(newExBtn);
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

        private void AddNewQst()
        {
           // exScrollRect.SetActive(true);
            var newQst = PoolSystem.instance.Spawn<QuestionBtn>(ObjectToPoolType.Question);
            //newQst.transform.localScale = Vector3.one;

            if (_qstNbr == 0)
                newQst.Initialize();
            else
                newQst.Init(_qstNbr);

           newQst.transform.SetParent(transform.parent);
           // newQst.transform.SetParent(qstHolder);
            Debug.Log("Spawn !");
            //newQst.Transform.localScale = Vector3.one;

            _currentQstList.Add(newQst);
        }
    }
}