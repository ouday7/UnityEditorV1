using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class QuestionBtn : PoolableObject
    {
        [SerializeField] private Text btnName;
        [SerializeField] private Button deleteQstBtn;
        [SerializeField] private InputField mainQuestion; 
        [SerializeField] private  InputField subQuestion;
        [SerializeField] private InputField HelpQuestion;
        
        
        private const string _qstName = "  سؤال  ";

        private void Start()
        {
            deleteQstBtn.onClick.AddListener(DeleteQst);
        } 
        public  void Initialize()
        {
            const int newId = 1;
            this.btnName.text = _qstName + newId ;
        }
        public void Init(int qstNbr)
        {
            var newId = qstNbr + 1;
            btnName.text = _qstName + newId;
        }
        private void DeleteQst()
        {
            var exBtn = deleteQstBtn.transform.parent;
            PoolSystem.instance.DeSpawn(exBtn);
            ExerciseController.instance._currentQstList.Remove(exBtn.GetComponent<QuestionBtn>());
        }

        
    }
}
