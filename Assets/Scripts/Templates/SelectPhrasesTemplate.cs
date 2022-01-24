using System.Collections.Generic;
using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;

namespace Templates
{
    public class SelectPhrasesTemplate : TemplateBase
    {
        [SerializeField] private List<ChoiceButton> choices;
        [SerializeField] private ChoiceButton _choiceButton;
        [SerializeField] private CustomGridLayout _buttonsList;
        private ChoiceButton _currentChoice;
        private ChoiceButton _correctChoice;
        private int _fieldNumber=0;
    
        public override void Initialize()
        {
            EditorModeManager.Instance.resultBtn.onClick.AddListener(ReturnResult);
        }

        public override void BindData(QuestionData inQuestionData)
        {
            foreach (var quizFieldData in inQuestionData.quizFields)
            {
                if (_fieldNumber == 0)
                {
                    _fieldNumber++;
                    continue;
                }
                var newBtn = Instantiate(_choiceButton, _buttonsList.RectTransform);
                newBtn.Initialize(quizFieldData);
                newBtn.BindData(quizFieldData);
                OnClickChoiceButton(newBtn);
            }
            _buttonsList.UpdateLayout();
        }
        
        private void OnClickChoiceButton(ChoiceButton newBtn)
        {
            newBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (_currentChoice != null)
                {
                    _currentChoice.Unselect();
                    newBtn.Select();
                    _currentChoice = newBtn;
                    return;
                }
                newBtn.Select();
                _currentChoice = newBtn;
            });
        }

        private void ReturnResult()
        {
            if (_currentChoice == null)
            {
                EditorModeManager.Instance.losePanel.SetActive(true);
                return;
            }
            if (_currentChoice.data.toggleA)
            {
                EditorModeManager.Instance.winPanel.SetActive(true);
                return;
            }
            EditorModeManager.Instance.losePanel.SetActive(true);
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


