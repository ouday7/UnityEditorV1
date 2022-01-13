using System;
using ChapterPanel;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class EditorModeManager : MonoBehaviour
{
        public static event Action<QuestionData> OnDesignClick;
    

        [BoxGroup("Unselected Mode")][LabelText("Color")] [SerializeField] private Color unselectedColor;
        [BoxGroup("Selected Mode")][LabelText("Color")] [SerializeField] private Color selectedColor;
        
        [SerializeField] private GameObject designmodePanel;
        [SerializeField] private Button designmodeButton;
        [SerializeField] private Button editmodeButton;
        
        private Button _btn;
        private Image _bg;

        private void Awake()
        {
            designmodeButton.onClick.AddListener(ClickDesignMode);
            editmodeButton.onClick.AddListener(OnClickEditMode);

            OnDesignClick += OnClickDesignMode;
        }
        
        private void ClickDesignMode()
        {
            OnDesignClick?.Invoke(EditManager.Instance.currentQuestionData);
        }

        private void OnClickDesignMode(QuestionData questionData)
        {
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
