using System.Collections.Generic;
using ChapterPanel;
using ModeManager;
using QuizFields;
using UnityEngine;
using Random = System.Random;

namespace Templates
{
    public class SortingListTemplate: TemplateBase
    {
        [SerializeField] private DragController _draggableButton;
        [SerializeField] private CustomGridLayout _optionsList;
        private bool result;
        public override void Initialize()
        {
            EditorModeManager.Instance.resultBtn.onClick.AddListener(ReturnResult);
        }
        public override void BindData(QuestionData inQuestionData)
        {
            var itemsNumber = inQuestionData.quizFields.Count;
            for (var i = 0; i < itemsNumber; i++)
            {
                var newBtn = Instantiate(_draggableButton, _optionsList.RectTransform);
                newBtn.BindData(inQuestionData.quizFields[i]);
                newBtn.Initialize(_optionsList);
            }
            _optionsList.ShuffleLayout();
        }
        public void UpdateList()
        {
            _optionsList.UpdateLayout();
        }
        private void ReturnResult()
        {
            var correctSort=1;
            foreach (Transform btn in _optionsList.RectTransform)
            {
                if (btn.GetComponent<DragController>().data.id == correctSort)
                {
                    correctSort++;
                    continue;
                }
                {
                    QuizLost.lastTemplate = gameObject;
                    this.gameObject.SetActive(false);
                    EditorModeManager.Instance.quizoLost.SetActive(true);
                    return;
                }
            }
            QuizoWon.lastTemplate = gameObject;
            this.gameObject.SetActive(false);
            EditorModeManager.Instance.quizoWon.gameObject.SetActive(true);
        }
        
        public override bool GetResult()
        {
            return result;
        }

        public override void ResetTemplate()
        {
            var nbChild = _optionsList.RectTransform.childCount;
            while (nbChild>0)
            {
                Destroy(_optionsList.transform.GetChild(0).gameObject);
                nbChild--;
            }
            
        }

        public override void OnDestroy()
        {
            ResetTemplate();
        }
    }
}
