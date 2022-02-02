﻿using System;
using ChapterPanel;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Templates;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UPersian.Components;

namespace ModeManager
{
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
        [SerializeField] private Button resultBtn;
        [SerializeField] public GameObject quizoWon;
        [SerializeField] public GameObject quizoLost;
        private Button _btn;
        private Image _bg;
        public static QuestionData currentQuestionData;
        private TemplateBase _currentTemplate;
        

        private void OnDestroy()
        {
            QuestionBtn.OnClickQuestion -= InitiateDesignMode;
            OnDesignClick -= OnClickDesignMode;
            OnGetTemplateComplete -= StartTemplate;
        }

        public override  void Begin()
        {
            if(Instance!=null) return;
            Instance=this;
            
            resultBtn.onClick.AddListener(Result);
            designModeButton.onClick.AddListener(ClickDesignMode);
            editModeButton.onClick.AddListener(OnClickEditMode);
            hideWarningPanelBtn.onClick.AddListener(() => warningPanel.SetActive(false));
            OnDesignClick += OnClickDesignMode;
            QuestionBtn.OnClickQuestion += InitiateDesignMode;
            OnGetTemplateComplete += StartTemplate;
        }

        private void Result()
        {
            if (!_currentTemplate.GetResult())
            {
                QuizLost.lastTemplate = _currentTemplate.gameObject;
                _currentTemplate.gameObject.SetActive(false);
                quizoLost.SetActive(true);
                return;
            }
            
            QuizoWon.lastTemplate = _currentTemplate.gameObject;
            _currentTemplate.gameObject.SetActive(false);
            quizoWon.gameObject.SetActive(true);
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
                EditManager.Instance.MaximiseMainContentHolder();
                return;
            }
            currentQuestionData = questionButton.Data;
        }

        private void GenerateTemplate(int templateId, QuestionData currentQuestionData)
        {
             TemplatesHandler.Instance.GetTemplateById(templateId,OnComplete);
            
        }

        private void OnComplete(TemplateBase obj)
        {
            if (obj == null)
            {
                Debug.Log("Prefab is null!");
                return;
            }
            obj.transform.SetParent(designModePanel.transform);
            obj.Transform.localScale = Vector3.one;
            OnGetTemplateComplete?.Invoke(obj, currentQuestionData);
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
            quizoLost.SetActive(false);
            quizoWon.SetActive(false);
            editModeButton.interactable = false;
            editModeButton.image.color = selectedColor;

            designModeButton.interactable = true;
            designModeButton.image.color = unselectedColor;
        }
    }
}