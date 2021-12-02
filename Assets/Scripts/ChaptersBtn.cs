using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;
public class ChaptersBtn : PoolableObject
{
    [SerializeField] private Button editBtn;
    [SerializeField] private RtlText _text;
    private Chapter _data;
    private bool _isInitialized=false;
    public Chapter Data => _data;
    public void Initialize()
    {
        if(_isInitialized) return;
        editBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.ChapterEdit(this);
            _text.text = _data.name;
            gameObject.SetActive(true);
        });
        _isInitialized = true;
    }
    public void BindData(Chapter chapter )
    {
        _data = chapter;
        _text.text = _data.name;
        transform.SetSiblingIndex(chapter.order-1);
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