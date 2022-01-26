using System.Collections.Generic;
using ChapterPanel;
using EditorMenu;
using ModeManager;
using UnityEngine;
using UnityEngine.UI;

namespace Templates
{
    public class SelectPhrasesTemplate : TemplateBase
    {
        [SerializeField] private ChoiceButton _choiceButton;
        [SerializeField] private CustomGridLayout _buttonsList;
        private ChoiceButton _currentChoice;
        private bool _isFirstToggle;
        private bool _result;
    
        public override void Initialize()
        {
            EditorModeManager.Instance.resultBtn.onClick.AddListener(()=>GetResult());
        }

        public override void BindData(QuestionData inQuestionData)
        {
            foreach (var quizFieldData in inQuestionData.quizFields)
            {
                if (!_isFirstToggle)
                {
                    _isFirstToggle = true;
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
        
        public override bool GetResult()
        {
            if (_currentChoice == null)
            {
                _result = false;
                EditorModeManager.Instance.losePanel.SetActive(true);
                return _result;
            }
            if (_currentChoice.data.toggleA)
            {
                _result = true;
                EditorModeManager.Instance.winPanel.SetActive(true);
                return _result;
            }
            _result = false;
            EditorModeManager.Instance.losePanel.SetActive(true);
            return _result;
        }
        
        public override void ResetTemplate()
        {
        }
        
        public override void OnDestroy()
        {
        }
    }
}


