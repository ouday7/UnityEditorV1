using System;
using EditorMenu;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class QuestionBtn : PoolableObject
    {
        public static event Action<QuestionBtn> OnClickQuestion; 
        
        [SerializeField] private Text btnName;
        [SerializeField] private Button deleteQstBtn;

        private QuestionData Data;
        private bool _isInitialized=false;
        private const string _qstName = "  سؤال  ";

        public  void UpdateName()
        {
            this.btnName.text = _qstName + transform.GetSiblingIndex();
        }
        
        public void Initialize(QuestionBtn qstBtn)
        {
            if(_isInitialized) return;
            deleteQstBtn.onClick.AddListener(DeleteQst);
            btn.GetComponent<Button>().onClick.AddListener(() =>
            qstBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnClickQuestion?.Invoke(qstBtn);
                MenuController.instance.mainContent.gameObject.SetActive(true);
            });
            _isInitialized = true;
        }

        public void BindData(QuestionData quesData)
        {
            Data = quesData;
            Data.mainQst = quesData.mainQst;
            Data.subQst = quesData.subQst;
            Data.templateId = quesData.templateId;
            Data.situationData = quesData.situationData;
        }
        private void DeleteQst()
        {
            var exBtn = deleteQstBtn.transform.parent;
            PoolSystem.instance.DeSpawn(exBtn);
            MenuController.instance.currentQstList.Remove(exBtn.GetComponent<QuestionBtn>());
        }
    }
}
