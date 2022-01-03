using System;
using ChapterPanel;
using Envast.Layouts;
using UnityEngine;
using UnityEngine.UI;


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


    

    public void Start()
    {
        openPanel.onClick.AddListener(OpenPanel);

    }



    private void OpenPanel()
    {
        panelPopUp.SetActive(true);
    }
}