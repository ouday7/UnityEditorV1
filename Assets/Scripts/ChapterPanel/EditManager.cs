using EditorMenu;
using Envast.Layouts;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class EditManager : MonoBehaviour
    {
        [SerializeField] private Text minFieldsTxt;
        [SerializeField] private QuestionFields questionFields;
        [SerializeField] private SelectTemplate selectTemplate;
        [SerializeField] private CustomSizeFitter holder;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private TemplateCategory currentTemplate;
        [SerializeField] private Button openPanel;
        [SerializeField] private GameObject panelPopUp;
        [SerializeField] private SelectTemplateButton selectTemplateBtn;
        [SerializeField] private TemplateDataInformation _currentTemplate;
        [SerializeField] private InputField mainQuestion;
        [SerializeField] private InputField subQuestion;
        [SerializeField] private InputField helpQuestion;
        public void Start()
        {
            QuestionBtn.OnClickQuestion += ClickQuestion;
        }

        private void ClickQuestion(QuestionBtn qstBtn)
        { 
            mainQuestion.text = qstBtn.Data.mainQst;
            subQuestion.text = qstBtn.Data.subQst;
            helpQuestion.text = qstBtn.Data.subQst;

            SaveNewQuestionData(qstBtn);
   
        }
        private void SaveNewQuestionData(QuestionBtn qstBtn)
        {
             qstBtn.Data.mainQst = mainQuestion.text;
             qstBtn.Data.subQst = subQuestion.text;
             qstBtn.Data.helpQst = helpQuestion.text;
             GameDataManager.Instance.SaveToJson();
        }
    }
}