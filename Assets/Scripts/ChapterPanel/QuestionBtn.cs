
using EditorMenu;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class QuestionBtn : PoolableObject
    {
        public delegate void QstClick(QuestionBtn qstBtn);
        public static event QstClick OnClickQuestion; 
        
        [SerializeField] private Text btnName;
        [SerializeField] private Button deleteQstBtn;

        private QuestionData _data;
        public QuestionData Data=>_data;
        private bool _isInitialized=false;
        private const string _qstName = "  سؤال  ";

        public  void UpdateName()
        {
            this.btnName.text = _qstName + transform.GetSiblingIndex();
        }
        
        public void Initialize()
        {
            if(_isInitialized) return;
            deleteQstBtn.onClick.AddListener(()=>DeleteQst(this.transform.
                parent.transform.parent.transform.GetComponent<ExerciseBtn>()));
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
            Data.mainQst = quesData.mainQst;
            Data.subQst = quesData.subQst;
            Data.quizFields = quesData.quizFields;
            Data.templateId = quesData.templateId;
            Data.situationData = quesData.situationData;
        }
        private void DeleteQst(ExerciseBtn inExerciseBtn)
        {
            var qstBtn = deleteQstBtn.transform.parent;
            PoolSystem.instance.DeSpawn(qstBtn);
            MenuController.instance.currentQstList.Remove(qstBtn.GetComponent<QuestionBtn>());
            inExerciseBtn.Data.questions.Remove(qstBtn.GetComponent<QuestionBtn>().Data);
            GameDataManager.Instance.SaveToJson();
            
            MenuController.instance.UpdateExercisesHolderSize(-1);
            inExerciseBtn.MaximiseHolderSize(inExerciseBtn.QstHolder.transform.childCount);
            inExerciseBtn.MaximiseExerciseSize(inExerciseBtn.QstHolder);
            inExerciseBtn.QstHolder.UpdateLayout();
          
        }
    }
}
