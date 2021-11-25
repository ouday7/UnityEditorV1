using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;


public class SubjectsBtn : EditorButtonBase
{
    [SerializeField] private Button editBtn;
    [SerializeField] private Button btn;
    [SerializeField] private RtlText _text;
    
    private Subject _data;
    private bool _isInitialized=false;
    public Subject Data => _data;
    public override void Initialize()
    {
        if(_isInitialized) return;
        editBtn.onClick.AddListener(() =>
        {
            Debug.Log("Click Edit");
            UIManager.Instance.SubjectEdit(this);
            _text.text = _data.name;
            gameObject.SetActive(true);
        });
        _isInitialized = true;
        
        
        btn.GetComponent<Button>().onClick.AddListener(() =>
        {
          //  Debug.Log($"Select Subject[{subject.id}], from level[{id}]"); ;
            
            btn.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.ShowChapter(_data.id));

        });
    }
    public void BindData(Subject subject )
    {
        _data = subject;
        _text.text = _data.name;
        transform.SetSiblingIndex(subject.order);
    }
    public void UpdateData(string newName, string newOrderText)
    {
        _data.name = newName;
        if (int.TryParse(newOrderText, out var newOrder)) _data.order = newOrder;
        BindData(_data);
        GameManager.Instance.LogJson();
    }
    public void  UpdateLevelData()
    {
        
       // _data.levelId=
    }
}
