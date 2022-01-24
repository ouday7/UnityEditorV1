using System.Collections.Generic;
using ChapterPanel;
using QuizFields;
using UnityEngine;
using Random = System.Random;

namespace Templates
{
    public class SortingListTemplate: TemplateBase
    {
        [SerializeField] private DraggableButton _draggableButton;
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
                newBtn.Initialize(this);
                newBtn.BindData(inQuestionData.quizFields[i]);
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
                if (btn.GetComponent<DraggableButton>().Data.id == correctSort)
                {
                    correctSort++;
                    continue;
                }
                else
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
