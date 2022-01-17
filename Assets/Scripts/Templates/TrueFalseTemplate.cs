
using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;
using DG.Tweening;
using Unity.VisualScripting;

public class TrueFalseTemplate : TemplateBase
{
    [SerializeField] private RtlText questionTxt;
    [SerializeField] private Image imageTxt;
    [SerializeField] private Button btnA;
    [SerializeField] private Button btnB;
    private QuestionData questionData;
    private bool _result, _a, _b;


    public override void SetData()
    {
    }

    public override void Initialize(QuestionData questionData)
    {
        if (questionData.quizFields[0].textA == null)
        {
            questionTxt.text = "";
        }

        if (imageTxt.sprite == null)
        {
            Debug.Log("sprite null");
        }

        _result = false;
        imageTxt.sprite = questionData.quizFields[0].imageOne;
        questionTxt.text = questionData.quizFields[0].textA;
        
        btnA.GetComponentInChildren<RtlText>().text = questionData.quizFields[1].textA;
        btnB.GetComponentInChildren<RtlText>().text = questionData.quizFields[2].textA;

    
        btnA.onClick.AddListener(OnClickButtonA);
        btnB.onClick.AddListener(OnClickButtonB);

    }

    private void OnClickButtonA()
    {
     
        _a = questionData.quizFields[1].toggleA;
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
        _b = questionData.quizFields[2].toggleA;
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


        // btnA.transform.DOScale(new Vector3(0.75f, 0.5f, 1f),duration:5f)
        //     .SetEase(Ease.InOutSine)
        //     .SetLoops(-1, LoopType.Yoyo);
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