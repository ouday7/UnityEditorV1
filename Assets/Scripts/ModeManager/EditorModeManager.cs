using System;
using ChapterPanel;
using EditorMenu;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Templates;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

public class EditorModeManager : EntryPointSystemBase
{
    public static EditorModeManager Instance;
    public static event Action<QuestionData> OnDesignClick;
    public static event Action<TemplateBase, QuestionData> OnGetTemplateComplete;

    [BoxGroup("Unselected Mode")] [LabelText("Color")] [SerializeField]
    private Color unselectedColor;

    [BoxGroup("Selected Mode")] [LabelText("Color")] [SerializeField]
    private Color selectedColor;

    [SerializeField] private GameObject designModePanel;
    [SerializeField] private Button designModeButton;
    [SerializeField] private Button editModeButton;
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private Button hideWarningPanelBtn;
    [SerializeField] private RtlText mainQuestion;
    [SerializeField] private RtlText subQuestion;
    [SerializeField] public Button resultBtn;
    [SerializeField] public GameObject winPanel;
    [SerializeField] public GameObject losePanel;
    private Button _btn;
    private Image _bg;
    public static QuestionData currentQuestionData;
    private TemplateBase _currentTemplate;

    private void OnDestroy()
    {
        QuestionBtn.OnClickQuestion -= InitiateDesignMode;
    }

    public override  void Begin()
    {
        if(Instance!=null) return;
        Instance=this;
        
        designModeButton.onClick.AddListener(ClickDesignMode);
        editModeButton.onClick.AddListener(OnClickEditMode);
        hideWarningPanelBtn.onClick.AddListener(() => warningPanel.SetActive(false));
        OnDesignClick += OnClickDesignMode;
        QuestionBtn.OnClickQuestion += InitiateDesignMode;
        OnGetTemplateComplete += StartTemplate;
    }
    
    private void StartTemplate(TemplateBase template, QuestionData currentQuestion)
    {
        if(_currentTemplate != null) Destroy(_currentTemplate.gameObject);
        Debug.Log(template);
        _currentTemplate = template;
        _currentTemplate.Initialize();
        _currentTemplate.BindData(currentQuestion);
        _currentTemplate.transform.position = new Vector2(960, 0);
        mainQuestion.text = currentQuestion.mainQst;
        subQuestion.text = currentQuestion.subQst;
    }

    private void InitiateDesignMode(QuestionBtn questionButton)
    {
        if (questionButton.Data.quizFields.IsNullOrEmpty())
        {
            EditManager.Instance.MaximiseMainContentHolder(0);
            return;
        }
        currentQuestionData = questionButton.Data;
    }

    private void GenerateTemplate(int templateId, QuestionData currentQuestionData)
    {
        var newTemplate = TemplatesHandler.GetTemplateById(templateId);
        if (newTemplate == null)
        {
            Debug.Log("Prefab is null!");
            return;
        }
        newTemplate.transform.SetParent(designModePanel.transform);
        newTemplate.Transform.localScale = Vector3.one;
        OnGetTemplateComplete?.Invoke(newTemplate, currentQuestionData);
    }

    private void ClickDesignMode()
    {
        OnDesignClick?.Invoke(currentQuestionData);
    }

    private void OnClickDesignMode(QuestionData questionData)
    {
        if (EditManager.Instance.QuizFieldsHolder.RectTransform.childCount<1)
        {
            warningPanel.SetActive(true);
            return;
        }
        var templateId = questionData.templateId;
        GenerateTemplate(templateId, questionData);
        designModePanel.gameObject.SetActive(true);
        designModeButton.interactable = false;
        designModeButton.image.color = selectedColor;
        editModeButton.interactable = true;
        editModeButton.image.color = unselectedColor;
    }

    private void OnClickEditMode()
    {
        designModePanel.gameObject.SetActive(false);

        editModeButton.interactable = false;
        editModeButton.image.color = selectedColor;

        designModeButton.interactable = true;
        designModeButton.image.color = unselectedColor;
    }
}