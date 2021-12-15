using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class AddNewQuestion : MonoBehaviour
    {
        [SerializeField] private Button showQstsBtn;
        [SerializeField] private Button addQstBtn;
        [SerializeField] private GameObject exScrollRect;
        [SerializeField] private Transform qstHolder;
        
        private void Start()
        {
            addQstBtn.onClick.AddListener(()=>
            {
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
            newQst.Initialize();
            newQst.transform.SetParent(qstHolder);
        }
    }
}
