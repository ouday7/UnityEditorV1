using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTextQuizField : QuizFieldBase
{
    [SerializeField] private Toggle myToggle;
    [SerializeField] private InputField inputFiled;
    [SerializeField] private Button buttonRemove;
    
    public void Awake()
    {
        buttonRemove.onClick.AddListener(Remove);
        myToggle.onValueChanged.AddListener(delegate { ToggleValueChanged(myToggle); });
        inputFiled.onEndEdit.AddListener(delegate { LockInput(inputFiled); });
    }
    private void Remove()
    {
        Destroy(gameObject);
    }
    private void ToggleValueChanged(Toggle change)
    {
        Debug.Log(myToggle.isOn);
    }
    private void LockInput(InputField input)
    {
        if (input.text.Length > 0)
        {
            Debug.Log(inputFiled.text);
        }
        else if (input.text.Length == 0)
        {
            Debug.Log(" Input ");
        }
    }

    public override void Initialize()
    {
        
    }

    public override void BindData(QuizFieldData inData)
    {
        
    }
}