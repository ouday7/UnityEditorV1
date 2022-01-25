using System;
using ChapterPanel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Templates
{
    public class DragController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform currentTransform;
        [SerializeField] private Text labelText;
        [SerializeField] public Text labelIndex;
        [SerializeField] private int _distance;
        [NonSerialized]public QuizFieldData data;
    
        private CustomGridLayout _choicesList;
        private Vector2 _currentPosition;
        private QuizFieldData _quizFieldData;
        private int _totalChild;
  
        public void Initialize(CustomGridLayout optionsList)
        {
            _choicesList = optionsList;
            Invoke(nameof(Init),.05f);
        }
        public void Init()
        {
            var label = transform.GetSiblingIndex() + 1;
            labelIndex.text = label.ToString();
        }
        public void BindData(QuizFieldData inQuizFields)
        {
            data = inQuizFields;
            labelText.text = data.textA;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _currentPosition = currentTransform.position;
            _totalChild = _choicesList.transform.childCount;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var position = currentTransform.position;
            position=new Vector2(position.x, eventData.position.y);
            currentTransform.position = position;

            OnTrigger();
        }

        private void OnTrigger()
        {
            for (var i = 0; i < _totalChild; i++)
            {
                if (i == currentTransform.GetSiblingIndex()) continue;
                var otherTransform = _choicesList.transform.GetChild(i);
                var distance = (int) Vector2.Distance(currentTransform.position,
                    otherTransform.position);

                if (distance > _distance) continue;
                UpdatePositions(otherTransform);
                Invoke(nameof(UpdateListLayout), .01f);
            }
        }

        private void UpdatePositions(Transform otherTransform)
        {
            var targetPosition = otherTransform.position;
            var target = otherTransform;
            var position = currentTransform.position;
            var otherTransformOldPosition = targetPosition;
            
            targetPosition = new Vector2(targetPosition.x, _currentPosition.y);
            var label = target.transform.GetSiblingIndex();
            target.GetComponent<DragController>().labelIndex.text = label.ToString();
            otherTransform.position = targetPosition;
            
            SetCurrentTransform(otherTransform, position, otherTransformOldPosition);
        }

        private void SetCurrentTransform(Transform otherTransform, Vector3 position, Vector3 otherTransformOldPosition)
        {
            position = new Vector2(position.x, otherTransformOldPosition.y);
            currentTransform.position = position;
            currentTransform.SetSiblingIndex(otherTransform.GetSiblingIndex());
            var currentLabel = currentTransform.transform.GetSiblingIndex() + 1;
            currentTransform.GetComponent<DragController>().labelIndex.text = currentLabel.ToString();
            _currentPosition = position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            currentTransform.position = _currentPosition;
            Invoke(nameof(UpdateListLayout),.01f);
        }
        private void UpdateListLayout() =>_choicesList.UpdateLayout();
        
    }
}