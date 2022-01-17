using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;
using DG.Tweening;
using Templates;

public class TrueFalseTemplate : TemplateBase
{
    [SerializeField] private RtlText questionTxt;
    [SerializeField] private Image imageTxt;
    [SerializeField] private Button btnA;
    [SerializeField] private Button btnB;
    private QuestionData _questionData;
    private bool _result, _a, _b;

    public override void BindData(QuestionData inQuestionData)
    {
        _questionData = inQuestionData;
        if (_questionData.quizFields[0].textA == null)
        {
            questionTxt.text = "";
        }

        if (imageTxt.sprite == null)
        {
            Debug.Log("sprite null");
        }

        _result = false;
        imageTxt.sprite = _questionData.quizFields[0].imageOne;
        questionTxt.text = _questionData.quizFields[0].textA;
    }

    public override void Initialize()
    {
        btnA.GetComponentInChildren<RtlText>().text = _questionData.quizFields[1].textA;
        btnB.GetComponentInChildren<RtlText>().text = _questionData.quizFields[2].textA;

        btnA.onClick.AddListener(OnClickButtonA);
        btnB.onClick.AddListener(OnClickButtonB);
    }

    private void OnClickButtonA()
    {
        _a = _questionData.quizFields[1].toggleA;
        _result = _a;
        btnA.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.InOutBack);
        
        btnB.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.InOutBack);
        btnA.interactable = false;
        btnB.interactable = true;
    }

    private void OnClickButtonB()
    {
        _b = _questionData.quizFields[2].toggleA;
        _result = _b;
        btnB.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.InOutBack);

        btnA.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.InOutBack);
        btnA.interactable = true;
        btnB.interactable = false;

    }

    public override void ResetTemplate()
    {
        questionTxt.text = "";
        _a = false;
        _b = false;
        _result = false;
        btnA.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.InOutBack);
        btnB.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.InOutBack);
    }


    public override bool GetResult()
    {
        return _result;
    }

    public override void OnDestroy()
    {
        ResetTemplate();
    }
}