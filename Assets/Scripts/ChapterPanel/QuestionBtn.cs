using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class QuestionBtn : PoolableObject
    {
        [SerializeField] private Text btnName;
        [SerializeField] private Button deleteQstBtn;

        private QuestionData Data;

        private const string _qstName = "  سؤال  ";

        public  void UpdateName()
        {
            this.btnName.text = _qstName + transform.GetSiblingIndex();
        }
        
        public void Initialize(QuestionBtn btn)
        {
            deleteQstBtn.onClick.AddListener(DeleteQst);

            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                EditController.instance.mainContent.gameObject.SetActive(true);
            });
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
            EditController.instance.currentQstList.Remove(exBtn.GetComponent<QuestionBtn>());
        }
    }
}
