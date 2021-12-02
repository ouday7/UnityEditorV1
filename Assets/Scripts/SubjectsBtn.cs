using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;
public class SubjectsBtn : PoolableObject
{
    public static SubjectsBtn Instance;
    [SerializeField] private Button editBtn;
    [SerializeField] private Button btn;
    [SerializeField] private RtlText _text;
    
    private Subject _data;
    private bool _isInitialized=false;
    public Subject Data => _data;
    
    public void Initialize()
    {
        if(_isInitialized) return;
        editBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.SubjectEdit(this);
            _text.text = _data.name;
            gameObject.SetActive(true);
        });
        _isInitialized = true;
        
    }
    public void BindData(Subject subject)
    {
        _data = subject;
        _text.text = _data.name;
        transform.SetSiblingIndex(subject.order-1);
    }
    public void UpdateData(string newName, string newOrderText)
    {
        if (newName.Length == 0)
        {
            newName = _data.name;
        }
        if (newOrderText.Length == 0)
        {
            newOrderText = _data.order.ToString();

        }
        _data.name = newName;
        if (int.TryParse(newOrderText, out var newOrder)) _data.order = newOrder;
        BindData(_data);
        GameManager.Instance.SaveToJson();
    }
}
