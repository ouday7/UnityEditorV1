﻿using ChapterPanel;
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
    //public QuestionData Data;
    
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
        mainQuestionText.text = btn.data.mainQst;
        subQuestionText.text = btn.data.subQst;
        helpQuestionText.text = btn.data.helpQst;
    }

    private void  AddDataField()
    {
        foreach (var exBtn in MenuController.Instance.currentExList)
        {
            if (MenuController.Instance.currentExbtn._data.exerciseId !=
                exBtn._data.exerciseId)
            {
                data.exercises.Add(MenuController.Instance.currentExbtn._data);
                    
            }
        }
    }

    private void SetDatafiled()
    {
        for (int i = 0; i < MenuController.Instance.currentExList.Count; i++)
        {
            
        }
    }


    
    public void Clear()
    {
        mainQuestionText.text = "";
        subQuestionText.text = "";
        helpQuestionText.text = "";
    }

  
}