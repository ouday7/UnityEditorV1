using System;

public class LevelBtn : EditorButtonBase
{
    public event Action<LevelBtn> OnSelectLevelButton;
    
    private Level _data;
    public Level Data => _data;
    
    
    private void OnDestroy()
    {
        OnSelectLevelButton = null;
    }
    
    public override void Initialize()
    {
        if(_isInitialized) return;
        base.Initialize();
        editBtn.onClick.AddListener(() => UIManager.Instance.LevelEdit(this));
        OnSelectAction += LevelButtonSelected;
    }

    public void BindData(Level level)
    {
        _data = level;
        text.text = _data.name;
        transform.SetSiblingIndex(level.order-1);
    }

    public void LevelButtonSelected()
    {
        OnSelectLevelButton?.Invoke(this);
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
        var newOrder = _data.order;
        if (int.TryParse(newOrderText, out newOrder)) _data.order = newOrder;
        BindData(_data);
        GameDataManager.Instance.SaveToJson();
    }
}
