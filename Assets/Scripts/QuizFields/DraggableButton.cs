using ChapterPanel;
using Templates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UPersian.Components;

namespace QuizFields
{
    public class DraggableButton : MonoBehaviour
    {
        [SerializeField] private Text _sortText;
        [SerializeField] private RtlText buttonLabel;
        private DragComponent _dragComponent;
        private int _missingPosition;
        private int _defaultPosition;
        private bool _canExchange;
        private DraggableButton _targetBtn;
        private int targetPos;
        private SortingListTemplate _sortingList;
        private bool _permut;
        public QuizFieldData Data;

        private void OnDestroy()
        {
            _dragComponent.OnBeginDragCallback -= BeginDragging;
            _dragComponent.OnDragCallback -= Dragging;
            _dragComponent.OnEndDragCallback -= EndDragging;
        }

        public void Initialize(SortingListTemplate sortingTemplate)
        {
            _sortingList=sortingTemplate;
            _dragComponent = GetComponent<DragComponent>();
            _dragComponent.OnBeginDragCallback += BeginDragging;
            _dragComponent.OnDragCallback += Dragging;
            _dragComponent.OnEndDragCallback += EndDragging;
        }
        public void BindData(QuizFieldData inQuizFields)
        {
            Data = inQuizFields;
            buttonLabel.text = Data.textA;
        }
        private void BeginDragging(PointerEventData data)
        {
            _defaultPosition=transform.GetSiblingIndex();
            _missingPosition = _defaultPosition;
        }
        private void Dragging(PointerEventData data)
        {
            transform.position = new Vector2(transform.position.x, data.position.y);
        }
        private void EndDragging(PointerEventData data)
        {
            transform.SetSiblingIndex(_missingPosition);
            _sortingList.UpdateList();
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            targetPos = other.transform.GetSiblingIndex();
            other.transform.SetSiblingIndex(_missingPosition);
            _missingPosition = targetPos;
            _sortingList.UpdateList();
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            
        }
    }
}