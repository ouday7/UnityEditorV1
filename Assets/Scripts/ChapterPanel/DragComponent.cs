using System;
using ChapterPanel;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragComponent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public event Action<PointerEventData> OnDragCallback;
    public event Action<PointerEventData> OnBeginDragCallback;
    public event Action<PointerEventData> OnEndDragCallback; 

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragCallback?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragCallback?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragCallback?.Invoke(eventData);
    }
}
