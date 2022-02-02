using System.Collections.Generic;
using System.Linq;
using ChapterPanel;
using ModeManager;
using UnityEngine;
using UnityEngine.UI;

namespace Templates
{
    public class SelectPhrasesTemplate : TemplateBase
    {
        [SerializeField] private ChoiceButton _choiceButton;
        [SerializeField] private CustomGridLayout _buttonsList;
        private List<ChoiceButton> _currentChoices;
        private bool _result;
        private  Button _btn;
    
       
        public override void Initialize()
        {
            _currentChoices = new List<ChoiceButton>();
        }

        public override void BindData(QuestionData inQuestionData)
        {
            foreach (var quizFieldData in inQuestionData.quizFields)
            {
                var newBtn = Instantiate(_choiceButton, _buttonsList.RectTransform);
                newBtn.Initialize(quizFieldData);
                newBtn.BindData(quizFieldData);
                OnClickChoiceButton(newBtn);
            }
            _buttonsList.UpdateLayout();
        }

        private void OnClickChoiceButton(ChoiceButton newBtn)
        {
            newBtn.GetComponent<Button>()
                .onClick.AddListener(() =>
                {
                    if (!newBtn.IsSelected)
                    {
                        newBtn.Select();
                        _currentChoices.Add(newBtn);
                        return;
                    }

                    newBtn.Unselect();
                    _currentChoices.Remove(newBtn);
                });
        }
        
        public override bool GetResult()
        {
            return _currentChoices.All(choice => choice.data.toggleA);
        }
        public override void ResetTemplate()
        {
            var nbChild = _buttonsList.RectTransform.childCount;
            while (nbChild>0)
            {
                Destroy(_buttonsList.transform.GetChild(0).gameObject);
                nbChild--;
            }
            _currentChoices.Clear();
        }
        public override void OnDestroy()
        {
            ResetTemplate();
        }
    }
}


