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

        public QuestionData data;
        private bool _isInitialized=false;
        private const string QstName = "  سؤال  ";

        public  void UpdateName()
        {
            this.btnName.text = QstName + transform.GetSiblingIndex();
        }
        public void Initialize(QuestionBtn qstBtn)
        {
            if(_isInitialized) return;
            deleteQstBtn.onClick.AddListener(DeleteQst);
            qstBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnClickQuestion?.Invoke(qstBtn);

                MenuController.Instance.mainContent.gameObject.SetActive(true);

                OnClickQuestion?.Invoke(this);
            });
            _isInitialized = true;
        }
        public void BindData(QuestionData quesData)
        {
            data = quesData;
            data.questionId = 1;
            data.mainQst = quesData.mainQst;
            data.subQst = quesData.subQst;
            data.helpQst = quesData.helpQst;
            data.templateId = quesData.templateId;
            data.situationData = quesData.situationData;
        }
        private void DeleteQst()
        {
            var exBtn = deleteQstBtn.transform.parent;
            PoolSystem.instance.DeSpawn(exBtn);
            MenuController.Instance.currentQstList.Remove(exBtn.GetComponent<QuestionBtn>());
        }
    }
}
