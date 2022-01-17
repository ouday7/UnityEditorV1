﻿using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;

namespace Templates
{
    public abstract class TemplateBase : MonoBehaviour
    {
        public Text mainQuestionTxt;//todo QUESTION IS SEPARATE FROM TEMPLATE
        public Text subQuestionTxt;
        public Button resultBtn;

        private Transform _t;
        public Transform Transform
        {
            get
            {
                if (_t == null) _t = transform;
                return _t;
            }
        }
    
        public abstract void Initialize();
        public abstract void BindData(QuestionData inQuestionData);
        public abstract bool GetResult();
        public abstract void ResetTemplate();
        public abstract void OnDestroy();
    }
}
