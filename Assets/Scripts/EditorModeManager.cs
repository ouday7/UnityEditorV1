using System;
using System.Linq;
using ChapterPanel;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;
public class EditorModeManager : MonoBehaviour
{
        public static event Action<QuestionData> OnDesignClick;
        public static event Action<TemplateBase,QuestionData> OnGetTemplateComplete; 

        [BoxGroup("Unselected Mode")][LabelText("Color")] [SerializeField] private Color unselectedColor;
        [BoxGroup("Selected Mode")][LabelText("Color")] [SerializeField] private Color selectedColor;
        
        [SerializeField] private GameObject designmodePanel;
        [SerializeField] private Button designmodeButton;
        [SerializeField] private Button editmodeButton;
        [SerializeField] private GameObject warningPanel;
        [SerializeField] private Button HidewarningPanelBtn;
        [SerializeField] private GameObject gameplayPanel;
    
        private Button _btn;
        private Image _bg;
        private QuestionData currentQstData;
        

        private void Awake()
        {
            designmodeButton.onClick.AddListener(ClickDesignMode);
            editmodeButton.onClick.AddListener(OnClickEditMode);
            HidewarningPanelBtn.onClick.AddListener(()=>warningPanel.SetActive(false));
            OnDesignClick += OnClickDesignMode;
            QuestionBtn.OnClickQuestion += PrepareDesignMode;
            OnGetTemplateComplete += ShowTemplateGamePlay;
        }

        private void ShowTemplateGamePlay(TemplateBase obj,QuestionData currentQuestion)
        {
            obj.Initialize(currentQuestion);
        }
        private void PrepareDesignMode(QuestionBtn questionButton)
        {
            if (questionButton.Data.quizFields==null)
                return;
            
           var currentQstData=questionButton.Data;
           var templateId = currentQstData.templateId;
           GetTemplate(templateId,currentQstData);

        }
        private void GetTemplate(int templateId,QuestionData currentQuestionData)
        {
            var type = (TemplatesNames) templateId;
            Debug.Log(type.ToString());
            var prefab = TemplatesHandler.Instance.templatesCatalog.FirstOrDefault(obj => obj.type==type).prefab;
            if (prefab == null)
            {
                Debug.Log("Prefab is null !");
                return;
            }
            var template = Instantiate(prefab, gameplayPanel.transform).gameObject.GetComponent<TemplateBase>();
            template.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
            OnGetTemplateComplete?.Invoke(template,currentQuestionData);
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
            GetTemplate(templateId,questionData);
            designmodePanel.gameObject.SetActive(true);
            designmodeButton.interactable = false;
            designmodeButton.GetComponent<Image>().color = selectedColor;
            editmodeButton.interactable = true;
            editmodeButton.GetComponent<Image>().color = unselectedColor;
        }
        
        private void OnClickEditMode()
        {
            designmodePanel.gameObject.SetActive(false);
            
            editmodeButton.interactable = false;
            editmodeButton.GetComponent<Image>().color = selectedColor;
            
            designmodeButton.interactable = true;
            designmodeButton.GetComponent<Image>().color = unselectedColor;
        }

      
    }
