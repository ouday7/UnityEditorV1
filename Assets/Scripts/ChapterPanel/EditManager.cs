using System;
using Envast.Layouts;
using UnityEngine;
using UnityEngine.UI;


public class EditManager : MonoBehaviour
{
    [SerializeField] private Text minFieldsTxt;

    [SerializeField] private QuestionFields questionFields;

    //[SerializeField] private QuizFieldsContainer fieldsContainer;
    [SerializeField] private CustomSizeFitter holder;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private SelectTemplate selectTemplateBtn;


    [SerializeField] private TemplateCategory currentTemplate;

    [SerializeField] private Button openPanel;
    [SerializeField] private GameObject panelPopUp;


    public void Start()
    {
        openPanel.onClick.AddListener(OpenPanel);

    }

    private void OpenPanel()
    {
        panelPopUp.SetActive(true);
    }
}