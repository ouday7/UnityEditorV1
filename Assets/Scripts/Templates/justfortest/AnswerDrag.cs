using ChapterPanel;
using UnityEngine;
using UnityEngine.EventSystems;
using UPersian.Components;


public class AnswerDrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector2 firstPos;
    private Vector2 _defaultPosition;
    private Vector2 otherPosition;
    private static Vector2 saturedPos=new Vector2(0,0);
    
    public RtlText text;
    public QuizFieldData data;
    private bool isTrigger;

    public void Initialise(QuizFieldData inQuizFields)
    {
        data = inQuizFields;
        data.textA= inQuizFields.textA;
    }

    public void BindData()
    {
        text.text = data.textA;
    }

    private void Start()
    {
        _defaultPosition = transform.position;
         firstPos = _defaultPosition;
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _defaultPosition = transform.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position =
            new Vector2(eventData.position.x, eventData.position.y);
    }
    public void OnPointerUp(PointerEventData eventData)    
    {
        if (!isTrigger)
        {
            transform.position = new Vector2(firstPos.x, firstPos.y);
            return;
        }
        
        if (saturedPos.Equals(otherPosition))
        {
            transform.position = new Vector2(firstPos.x, firstPos.y);
            return;
        }

        transform.position = new Vector2(otherPosition.x,otherPosition.y);
        saturedPos = otherPosition;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("question")) return;
        isTrigger = true;
        otherPosition = other.transform.position;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("question")) return;
        isTrigger = false;
    }
}