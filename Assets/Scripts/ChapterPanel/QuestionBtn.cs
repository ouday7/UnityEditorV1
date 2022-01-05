
using System;
using EditorMenu;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class QuestionBtn : PoolableObject
    {
        public static event Action<QuestionBtn> OnClickQuestion; 
        
        [SerializeField] private Text btnName;
        [SerializeField] private Button deleteQstBtn;

        private QuestionData _data;
        public QuestionData Data=>_data;
        private bool _isInitialized=false;
        private ExerciseBtn _parentExercise;
        private const string _qstName = "سؤال";

        private void OnDestroy()
        {
            OnClickQuestion = null;
        }

        public  void UpdateName()
        {
            btnName.text = $"{_qstName} {transform.GetSiblingIndex() + 1}";
        }
        
        public void Initialize(ExerciseBtn inParent)
        {
            if(_isInitialized) return;
            _parentExercise = inParent;
            deleteQstBtn.onClick.AddListener(() => 
                _parentExercise.DeleteQuestion(this));
            this.GetComponent<Button>().onClick.AddListener(() =>
            {
                MenuController.instance.mainContent.gameObject.SetActive(true);
                OnClickQuestion?.Invoke(this);
            });
            _isInitialized = true;
        }

        public void BindData(QuestionData quesData)
        {
            _data = quesData;
            return;
            Data.mainQst = quesData.mainQst;
            Data.subQst = quesData.subQst;
            Data.quizFields = quesData.quizFields;
            Data.templateId = quesData.templateId;
            Data.situationData = quesData.situationData;
        }
        private void DeleteQst(ExerciseBtn inExerciseBtn)
        {

        }
    }
}
