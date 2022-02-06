using System;
using System.Collections;
using System.Collections.Generic;
using ChapterPanel;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;
using UPersian.Components;

public class QuestionConnectTextToText: MonoBehaviour
{
    public RtlText btnRtlText;
    public ProceduralImage img;
    private QuestionData _questionData;
    public QuizFieldData data;


    public void Initialise(QuizFieldData inQiuzField)
    {
        data = inQiuzField;
        data.textTwo = inQiuzField.textTwo;
    }

    public void BindData()
    {
        btnRtlText.text = data.textTwo; 
    }
    public void Start()
    {
        
    }

    public void CheckQuestionAnswer()
    {
        
        
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("question") )
        {
            Debug.Log("Collision Detected");
            //this.transform.position = _startPos;
            // transform.position = other.transform.position;
        }
    }
}
