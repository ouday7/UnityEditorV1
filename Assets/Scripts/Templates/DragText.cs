using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.ProceduralImage;
using UPersian.Components;

public class DragText : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public event Action<PointerEventData> OnDragCallback;
    public event Action<PointerEventData> OnBeginDragCallback;
    public event Action<PointerEventData> OnEndDragCallback;
    public RtlText text;
    private Vector2 _startPos;

    public ProceduralImage img;
    private bool _checkDrag;
    private bool _isDragging;
    private bool _changePos;


    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPos = transform.position;
        OnBeginDragCallback?.Invoke(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        _isDragging = true;
        transform.position = new Vector2(eventData.position.x, eventData.position.y);
        OnDragCallback?.Invoke(eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //_isDragging = false;
        if (_checkDrag)
        {
            _startPos = transform.position;
            OnEndDragCallback?.Invoke(eventData);
        }
        else
        {
            transform.position = new Vector2(transform.position.x, transform.position.y);
            
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.CompareTag("answer") && _isDragging)
        {
            img.color = Color.red;
            _checkDrag = true;
        }

        if (gameObject.CompareTag("question"))
        {
            img.color = Color.black;
            _changePos = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
            img.color = Color.yellow;
            _checkDrag = false;

        
    
        // if (!other.gameObject.CompareTag("question")) return;
        // img.color = Color.white;
        // _changePos = false;
    }
}