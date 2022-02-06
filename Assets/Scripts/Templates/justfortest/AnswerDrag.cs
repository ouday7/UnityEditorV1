using ChapterPanel;
using UnityEngine;
using UnityEngine.EventSystems;
using UPersian.Components;


public class AnswerDrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform currentTransform;
    private Vector2 _currentPosition;
    private Vector2 _startPos;
    public RtlText text;
    private QuizFieldData data;

    public void Initialise(QuizFieldData inQuizFields)
    {
        data = inQuizFields;
        data.textA= inQuizFields.textA;
    }

    public void BindData()
    {
        text.text = data.textA;
    }
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
        
        //this.transform.position = _startPos;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("question"))
        {
            Debug.Log("Collision Detected");
            //this.transform.position = _startPos;
            // transform.position = other.transform.position;
            currentTransform.position = other.transform.position;
            _currentPosition = other.transform.position;
        }
    }
}