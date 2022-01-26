using UnityEngine;
using UnityEngine.EventSystems;
using UPersian.Components;


public class AnswerDrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform currentTransform;
    public RectTransform startTransform;
    private Vector2 _currentPosition;
    private Vector2 _startPos;
    public RtlText text;
    public bool isDrag;
    public bool endDrag;
    
    void Start()
    {
        currentTransform = GetComponent<RectTransform>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _currentPosition = currentTransform.position;
        _startPos = currentTransform.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        currentTransform.position =
            new Vector3(eventData.position.x, eventData.position.y, currentTransform.position.z);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        currentTransform.position =
            new Vector3(eventData.position.x, eventData.position.y, currentTransform.position.z);
        var anchoredPosition = currentTransform.anchoredPosition;
        _currentPosition = new Vector3(anchoredPosition.x, anchoredPosition.y);

        if (endDrag == true)
        {
            this.transform.position = _startPos;
            transform.position = currentTransform.position;
        }
        //this.transform.position = _startPos;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("question") && (endDrag))
        {
            Debug.Log("Collision Detected");
            //this.transform.position = _startPos;
            // transform.position = other.transform.position;
            currentTransform.position = other.transform.position;
            _currentPosition = other.transform.position;
        }
    }
}