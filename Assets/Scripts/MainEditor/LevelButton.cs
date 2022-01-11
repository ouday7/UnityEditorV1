using System;
using ChapterPanel;
using EditorMenu;

public class LevelButton : EditorButtonBase
{
   
    public event Action<LevelButton> OnSelectLevelButton;
    
    private LevelData _data;
    private bool _isInitialized;
    public LevelData Data => _data;
    private void OnDestroy()
    {
        OnSelectLevelButton = null;
    }
    
    public override void Initialize()
    {
        if(_isInitialized) return;
        base.Initialize();
        editBtn.onClick.AddListener(() => PopUpManager.instance.LevelEdit(this));
        OnSelectAction += LevelButtonSelected;
    }

    public void BindData(LevelData levelData)
    {
        _data = levelData;
        text.text = _data.name;
        transform.SetSiblingIndex(levelData.order-1);
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
        GameDataManager.instance.SaveToJson();
    }
}
