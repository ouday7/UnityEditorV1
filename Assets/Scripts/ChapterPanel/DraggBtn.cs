using System.Collections;
using System.Collections.Generic;
using ChapterPanel;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggBtn : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ExerciseBtn _exerciseBtn;
    
    public void Initialize(ExerciseBtn parentBtn)
    {
        this._exerciseBtn = parentBtn;
        Debug.Log("");
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        this._exerciseBtn.OnStartDrag();
        Debug.Log("OnBeginDrag ");

    }

    public void OnDrag(PointerEventData eventData)
    {
        this._exerciseBtn.OnDrag(eventData.position);
        Debug.Log("on Drag ");

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("on End Drag ");

        this._exerciseBtn.OnEndDrag();
    }
}
