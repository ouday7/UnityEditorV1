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
        private Button _currentChoice;
        private ChoiceButton _correctChoice;
    
        public override void Initialize()
        {
        }

        public override void BindData(QuestionData inQuestionData)
        {
            foreach (var quizFieldData in inQuestionData.quizFields)
            {
                var newBtn = Instantiate(_choiceButton, _buttonsList.RectTransform);
                //todo: Bind Data SINGLE RESPONSIBILITY BELEHI
                newBtn.GetComponentInChildren<Text>().text = quizFieldData.textA;
               
                if (newBtn.GetComponentInChildren<Text>().text == quizFieldData.toggleA.ToString()) 
                    _correctChoice = newBtn;
                newBtn.GetComponent<Button>().onClick.AddListener(newBtn.OnClickChoiceButton);
            }
            _buttonsList.UpdateLayout();
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


