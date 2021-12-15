using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace ChapterPanel
{
    public class AddNewQuestion : MonoBehaviour
    {
        [SerializeField] private Button showQstsBtn;
        [SerializeField] private Button addQstBtn;
        [SerializeField] private GameObject exScrollRect;
        [SerializeField] private Transform qstHolder;

        public static List<QuestionBtn> _currentQstList;
        private int qstNbr;
        
        private void Start()
        {
            _currentQstList = new List<QuestionBtn>();
            addQstBtn.onClick.AddListener(()=>
            {
                qstNbr= addQstBtn.transform.parent.Find("Scroll View").Find("Viewport").Find("Content").childCount;
                //qstNbr = addQstBtn.transform.parent.GetComponent<ScrollRect>().content.childCount;
                AddNewQst();
            });
            showQstsBtn.onClick.AddListener(ShowQsts);
        }

        private void ShowQsts()
        {
            exScrollRect.SetActive(true);
        }

        private void AddNewQst()
        {
            exScrollRect.SetActive(true);
            var newQst = PoolSystem.instance.Spawn<QuestionBtn>(ObjectToPoolType.Question);
            newQst.transform.localScale = Vector3.one;

            if (qstNbr == 0) newQst.Initialize();
            else newQst.ContinueInit(qstNbr);
            
            newQst.transform.SetParent(qstHolder);
            _currentQstList.Add(newQst);
        }
    }
}
