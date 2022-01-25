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
                    EditorModeManager.Instance.losePanel.SetActive(true);
                    return;
                }
            }
            EditorModeManager.Instance.winPanel.SetActive(true);
        }
        
        public override bool GetResult()
        {
            return true;
        }

        public override void ResetTemplate()
        {
            
        }

        public override void OnDestroy()
        {
            
        }
    }
}
