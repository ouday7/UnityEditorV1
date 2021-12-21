using ChapterPanel;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggBtn : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ExerciseBtn _exerciseBtn;
    
    public void Initialize(ExerciseBtn parentBtn)
    {
       _exerciseBtn = parentBtn;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        this._exerciseBtn.OnStartDrag();
    }

    public void OnDrag(PointerEventData eventData)
    {
        this._exerciseBtn.OnDrag(eventData.position);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this._exerciseBtn.OnEndDrag();
    }
}
