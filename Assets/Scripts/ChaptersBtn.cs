using System;

public class ChaptersBtn : EditorButtonBase
{

    public static event Action<ChaptersBtn> OnSelectChaptersButton;

    private Chapter _data;
    public Chapter Data => _data;

    
    
    
    private void OnDestroy()
    {
        OnSelectChaptersButton = null;
    }
    public override void Initialize()
    {
        if(_isInitialized) return;
        base.Initialize();

        editBtn.onClick.AddListener(() => UIManager.Instance.ChapterEdit(this));
        OnSelectAction += OnSelectChapterButton;
    }

    public void BindData(Chapter chapter )
    {
        _data = chapter;
        text.text = _data.name;
        transform.SetSiblingIndex(chapter.order-1);
    }

    private void OnSelectChapterButton()
    {
        
        OnSelectChaptersButton?.Invoke(this);

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
        GameDataManager.Instance.SaveToJson();
    }
}