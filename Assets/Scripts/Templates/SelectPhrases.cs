using System.Collections.Generic;
using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

namespace Templates
{
    public class SelectPhrases : TemplateBase
    {
        [SerializeField] private List<ChoiceButton> choices;
        [SerializeField] private ChoiceButton _choiceButton;
        [SerializeField] private CustomGridLayout _buttonsList;
        private Button _currentChoice;
        private ChoiceButton _correctChoice;
    
        public override void Initialize(QuestionData question)
        {
            mainQuestionTxt.text = question.mainQst;
            subQuestionTxt.text = question.subQst;
        
            foreach (var t in question.quizFields)
            {
                var newBtn = Instantiate(_choiceButton, _buttonsList.RectTransform);
                newBtn.GetComponentInChildren<Text>().text = t.textA;
               
                if (newBtn.GetComponentInChildren<Text>().text == t.toggleA.ToString())
                {
                    _correctChoice = newBtn;
                };
                newBtn.GetComponent<Button>().onClick.AddListener(newBtn.OnClickChoiceButton);
            }
            _buttonsList.UpdateLayout();
        }

        public override void SetData()
        {
            
        }

        public override bool GetResult()
        {
            return false;
        }

        public override void ResetTemplate()
        {
           
        }

        public override void OnDestroy()
        {
            
        }
    }
}


