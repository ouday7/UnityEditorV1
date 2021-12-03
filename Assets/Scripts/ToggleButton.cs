using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UPersian.Components;

public class ToggleButton : PoolableObject
{
    public event Action<ToggleButton> OnClickToggle;

    [SerializeField] internal bool _isToggle;
        
    [SerializeField] private RtlText toggleText;
    [SerializeField] private Button toggleButton;
    [SerializeField] private Image mainImage;

    [SerializeField] private Color selectedCase;
    [SerializeField] private Color unselectedCase;
    
    private int _id;
    private bool _isInitialized;

    public int Id => _id;

    public void Initialize()
    {
        if(_isInitialized) return;
        toggleButton.onClick.AddListener(SelectButton);
        _isInitialized = true;
    }

    public void BindData(string inLabel, int inId)
    {
        _id = inId;
        toggleText.text = inLabel;
    }

    public void CheckToggleState(int lvlid,int lvlidInChapter)
    {
        if(lvlid==lvlidInChapter) Select();
        
        else Unselect();
    }
    private void SelectButton() => OnClickToggle?.Invoke(this);

    public void MarkSelected(List<int> dataSubjectsId)
    {
        if (dataSubjectsId.Contains(_id)) 
        Select();
        
        else Unselect();
    }

    public void Select()
    {
        if(!_isToggle) toggleButton.interactable = false;
        mainImage.color = selectedCase;
    }

    public void Unselect()
    {
        if(!_isToggle) toggleButton.interactable = true;
        mainImage.color = unselectedCase;
    }
    
    public void MarkSelectedChapterToSubject(int chaptersubjId,int x )
    {
        if (chaptersubjId!=x) 
            Select();
        
        else Unselect();
    }
}