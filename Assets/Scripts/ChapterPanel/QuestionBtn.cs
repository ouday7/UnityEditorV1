
using System;
using DG.Tweening;
using EditorMenu;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace ChapterPanel
{
    public class QuestionBtn : PoolableObject
    {
        public static event Action<QuestionBtn> OnClickQuestion; 
        
        [SerializeField] public Text btnName;
        [SerializeField] private Button deleteQstBtn;

        private QuestionData _data;
        public QuestionData Data=>_data;
        private bool _isInitialized=false;
        private ExerciseBtn _parentExercise;
        public static string _qstName = " سؤال ";

      
        public  void UpdateName()
        {
            btnName.text = $"{_qstName} {transform.GetSiblingIndex() + 1}";
        }
        
        public void Initialize(ExerciseBtn inParent)
        {
            if(_isInitialized) return;
            _parentExercise = inParent;
            deleteQstBtn.onClick.AddListener(() => 
                _parentExercise.DeleteQuestion(this));
            this.GetComponent<Button>().onClick.AddListener(() =>
            {
                
                MenuController.instance.mainContent.DOAnchorPos(new Vector2(0.5f, 1300), 0.01f);
                MenuController.instance.mainContent.gameObject.SetActive(true);
                MenuController.instance.mainContent.DOAnchorPos(new Vector2(0.5f, -233), 0.4f).OnComplete(() =>
                {
                    EditManager.Instance.UpdateGridLayout();
                    if (this._data.quizFields.IsNullOrEmpty())
                    {
                        EditManager.Instance.UpdateGridLayout();
                        return;
                    }
                    
                    EditManager.Instance.UpdateMainContentPosition(false);
                    EditManager.Instance.UpdateGridLayout();
                });
                OnClickQuestion?.Invoke(this);
                EditManager.Instance.UpdateGridLayout();
            });
            _isInitialized = true;
        }

        public void BindData(QuestionData quesData)
        {
            _data = quesData;
        }

        public void Select()
        {
            transform.DOScale(0.75f, 0.15f).OnComplete(() =>
            {
                transform.DOScale(1, 0.15f);
            });
        }
    }
}
