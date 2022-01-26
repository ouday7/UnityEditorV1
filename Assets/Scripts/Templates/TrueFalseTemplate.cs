using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;
using Templates;

public class TrueFalseTemplate : TemplateBase
{
    [SerializeField] private RtlText questionTxt;
    [SerializeField] private Image imageTxt;
    [SerializeField] private ChoiceBtn btnA;
    [SerializeField] private ChoiceBtn btnB;
    private QuestionData _questionData;
    private bool _result, _resultBtnA, _resultBtnB;

    public override void Initialize()
    {
        btnA.OnClicked += OnClickButtonA;
        btnB.OnClicked += OnClickButtonB;
    }

    public override void BindData(QuestionData inQuestionData)
    {
        _questionData = inQuestionData;
        _result = false;
        _resultBtnA = false;
        _resultBtnB = false;
        questionTxt.text = _questionData.quizFields[0].textA;
        btnA.btnRtlText.text = _questionData.quizFields[1].textA;
        btnB.btnRtlText.text  = _questionData.quizFields[2].textA;
       
// todo set image 
       //  imageTxt.GetComponent<Image>().sprite = 
       //      Resources.Load<Sprite>("image");
       // imageTxt.sprite = _questionData.quizFields[0].imageOne;
       // imageTxt.sprite=imageTxt.sprite = Resources.Load <Sprite> ("image");
    }
    private void OnClickButtonA(ChoiceBtn bA)
    {
        _resultBtnB = _questionData.quizFields[2].toggleA;
        _result = _resultBtnB;
        bA.Select();
        btnB.UnSelect();
    }

    private void OnClickButtonB(ChoiceBtn bB)
    {
        _resultBtnB = _questionData.quizFields[2].toggleA;
        _result = _resultBtnB;
        bB.Select();
        btnA.UnSelect();
   
    }

    public override bool GetResult()
    {
        return _result;
    }
    public override void ResetTemplate()
    {
        questionTxt.text = "";
        _result = false;
        _resultBtnA = false;
        _resultBtnB = false;
        btnA.UnSelect();
        btnB.UnSelect();
        btnA.OnClicked -= OnClickButtonA;
        btnB.OnClicked -= OnClickButtonB;
    }

    public override void OnDestroy()
    {
        ResetTemplate();
    }
}