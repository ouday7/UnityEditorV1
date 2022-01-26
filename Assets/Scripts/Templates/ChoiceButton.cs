﻿using System.Collections.Generic;
using ChapterPanel;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UPersian.Components;

namespace Templates
{
    public class ChoiceButton : MonoBehaviour
    {
        [BoxGroup("Unselected ")][LabelText("Color")] [SerializeField] private Color unselectedColor;
        [BoxGroup("Selected ")][LabelText("Color")] [SerializeField] private Color selectedColor;
        
        public RtlText buttonLabel;

        private bool _correctChoice;
        private List<bool> correctChoices;
        public QuizFieldData data;
        public void Select()
        {
            transform.DOScale(0.7f, 0.15f).OnComplete(()=>
            {
                transform.DOScale(1.2f, 0.20f);
                transform.GetComponent<Image>().DOColor(selectedColor,0.05f);
                transform.GetComponent<Button>().interactable = false;
            });
        }
        public void Unselect()
        {
            transform.GetComponent<Image>().DOColor(unselectedColor,0.15f);
            transform.DOScale(1, 0.1f);
            transform.GetComponent<Button>().interactable = true;
        }

        public void Initialize(QuizFieldData inQuizFields)
        {
            data = inQuizFields;
            data.textA = inQuizFields.textA;
            data.toggleA = inQuizFields.toggleA;
        }

        public void BindData(QuizFieldData inQuizFields)
        {
            buttonLabel.text = inQuizFields.textA;
            _correctChoice = inQuizFields.toggleA;
        }
    }
}
