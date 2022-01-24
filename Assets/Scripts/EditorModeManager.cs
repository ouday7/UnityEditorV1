using System;
using ChapterPanel;
using Sirenix.OdinInspector;
using Templates;
using UnityEngine;
using UnityEngine.UI;

public class EditorModeManager : MonoBehaviour
{
    public static event Action<QuestionData> OnDesignClick;
    public static event Action<TemplateBase, QuestionData> OnGetTemplateComplete;

    [BoxGroup("Unselected Mode")] [LabelText("Color")] [SerializeField]
    private Color unselectedColor;

    [BoxGroup("Selected Mode")] [LabelText("Color")] [SerializeField]
    private Color selectedColor;

    [SerializeField] private GameObject designmodePanel;
    [SerializeField] private Button designmodeButton;
    [SerializeField] private Button editmodeButton;
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private Button HidewarningPanelBtn;
    [SerializeField] private GameObject gameplayPanel;

    private Button _btn;
    private Image _bg;
    private QuestionData _currentQuestion;
    private TemplateBase _currentTemplate;

    private void OnDestroy()
    {
        QuestionBtn.OnClickQuestion -= InitiateDesignMode;
    }

    private void Awake()
    {
        designmodeButton.onClick.AddListener(ClickDesignMode);
        editmodeButton.onClick.AddListener(OnClickEditMode);
        HidewarningPanelBtn.onClick.AddListener(() => warningPanel.SetActive(false));
        OnDesignClick += OnClickDesignMode;
        QuestionBtn.OnClickQuestion += InitiateDesignMode;
        OnGetTemplateComplete += StartTemplate;
    }

    private void StartTemplate(TemplateBase template, QuestionData currentQuestion)
    {
        if(_currentTemplate != null) Destroy(_currentTemplate.gameObject);
        _currentTemplate = template;
        _currentTemplate.Initialize();
        _currentTemplate.BindData(currentQuestion);
    }

    private void InitiateDesignMode(QuestionBtn questionButton)
    {
        if (questionButton.Data.quizFields == null)
        {
            /*foreach (Transform obj in gameplayPanel.transform)
            {
                Destroy(obj);                  
            }*/
            return;
        }

        _currentQuestion = questionButton.Data;
        // var currentQstData = questionButton.Data;
        // var templateId = currentQstData.templateId;
        // GetTemplate(templateId, currentQstData);
    }

    private void GenerateTemplate(int templateId, QuestionData currentQuestionData)
    {
        var type = (TemplatesNames) templateId;
        Debug.Log(type.ToString());
        var newTemplate = TemplatesHandler.GetTemplateById(templateId);
        if (newTemplate == null)
        {
            Debug.Log("Prefab is null!");
            return;
        }
        Debug.Log(newTemplate.name + "prefab name");
        newTemplate.Transform.SetParent(gameplayPanel.transform);
        newTemplate.Transform.localScale = Vector3.one;
        OnGetTemplateComplete?.Invoke(newTemplate, currentQuestionData);
    }
    private void ClickDesignMode()
    {
        OnDesignClick?.Invoke(EditManager.Instance.currentQuestionData);
    }
    private void OnClickDesignMode(QuestionData questionData)
    {
        if (questionData.quizFields.Count == 0)
        {
            warningPanel.SetActive(true);
            return;
        }

        var templateId = questionData.templateId;
        GenerateTemplate(templateId, questionData);
        designmodePanel.gameObject.SetActive(true);
        designmodeButton.interactable = false;
        designmodeButton.image.color = selectedColor;
        editmodeButton.interactable = true;
        editmodeButton.image.color = unselectedColor;
    }
    private void OnClickEditMode()
    {
        designmodePanel.gameObject.SetActive(false);

        editmodeButton.interactable = false;
        editmodeButton.image.color = selectedColor;

        designmodeButton.interactable = true;
        designmodeButton.image.color = unselectedColor;
    }
}