using ChapterPanel;
using EditorMenu;
using UnityEngine;
using UnityEngine.UI;

public class QuestionFields : MonoBehaviour
{
    [SerializeField] private InputField mainQuestionText;
    [SerializeField] private InputField subQuestionText;
    [SerializeField] private InputField helpQuestionText;
    [SerializeField] private InputField time;
    [SerializeField] private InputField points;
    [SerializeField] private Button saveBtn;
    [SerializeField] private TemplateBtn templateBtn;
    [SerializeField] private TemplateCategory templateCategory;

    private EditManager _editManager;
    public JsonData data;

    public void Start()
    {
        QuestionBtn.OnClickQuestion += QuestionClick;
        //  saveBtn.onClick.AddListener(SaveData);
    }

    private void QuestionClick(QuestionBtn qstBtn)
    {
        //  if(qstBtn.Data.mainQst=="")  

        // saveBtn.onClick.AddListener(SaveData);
    }

    public void SaveData()
    {
        GameDataManager.Instance.SaveToJson();
    }

    private void OnUpdateFields(QuestionBtn btn)
    {
        mainQuestionText.text = btn.Data.mainQst;
        subQuestionText.text = btn.Data.subQst;
        helpQuestionText.text = btn.Data.helpQst;
    }

 


    public void Clear()
    {
        mainQuestionText.text = "";
        subQuestionText.text = "";
        helpQuestionText.text = "";
    }
}