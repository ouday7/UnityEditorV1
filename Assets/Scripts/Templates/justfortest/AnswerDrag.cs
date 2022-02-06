using ChapterPanel;
using UnityEngine;
using UnityEngine.EventSystems;
using UPersian.Components;


public class AnswerDrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform currentTransform;
    private Vector2 _defaultPosition;
    private Vector2 firstPos;
    private Vector2 otherPosition;
    public RtlText text;
    private QuizFieldData data;
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
        firstPos =GetComponent<RectTransform>().position;
    }
        
    public void OnPointerDown(PointerEventData eventData)
    {
        currentTransform = GetComponent<RectTransform>();
        _defaultPosition = currentTransform.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        currentTransform.position =
            new Vector3(eventData.position.x, eventData.position.y, currentTransform.position.z);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isTrigger)
        {
            transform.position = firstPos;
            return;
        }

        transform.position = new Vector3(otherPosition.x, otherPosition.y);
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("question")) return;
        isTrigger = true;
        otherPosition = other.transform.position;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("question"))
        {
            isTrigger = false;
        }
    }
}