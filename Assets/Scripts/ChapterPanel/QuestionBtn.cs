using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class QuestionBtn : PoolableObject
    {
        [SerializeField] private Transform btnPrefab;
        [SerializeField] private Text btnName;
        [SerializeField] private Button deleteQstBtn;

        private static int _exercicseNbr=1;
        private const string _exName = "  سؤال  ";

        private void Start()
        {
            deleteQstBtn.onClick.AddListener(DeleteQst);
        } 
        public  void Initialize()
        {
            this.btnName.text = _exName + _exercicseNbr;
            _exercicseNbr++;
        }

        private void DeleteQst()
        {
            PoolSystem.instance.DeSpawn(deleteQstBtn.transform.parent);
        }
    }
}
