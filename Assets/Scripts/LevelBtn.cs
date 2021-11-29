using System;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;


public class LevelBtn : EditorButtonBase
{
    [SerializeField] private Button editBtn;
    [SerializeField] private Button btn;
    [SerializeField] private RtlText text;
    
    private Level _data;
    private bool _isInitialized=false;
    private GameManager _gameManager;

    public Level Data => _data;

   
    
    public override void Initialize()
    {
        if(_isInitialized) return;
        editBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.LevelEdit(this);
            text.text = _data.name;
            gameObject.SetActive(true);
            

        });
        _isInitialized = true;
         btn.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.ShowSubjects(_data.subjectsId,_data.id));
    }
    public void BindData(Level level)
    {
        _data = level;
        text.text = _data.name;
        transform.SetSiblingIndex(level.order);
    }
    public void UpdateData(string newName, string newOrderText)
    {
        _data.name = newName;
        if (int.TryParse(newOrderText, out var newOrder)) _data.order = newOrder;
        BindData(_data);
        GameManager.Instance.LogJson();
    }

    public void UpdateDataSubjectOfLevel(int newSubject)
    {
        _data.subjectsId.Add(newSubject);
        // GameManager.Instance.LogJson();
    }
}
