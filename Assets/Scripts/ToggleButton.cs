using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

public class ToggleButton : PoolableObject
{
    public event Action<ToggleButton> OnSelectToggleButton;

    public bool isToggle;
        
    [SerializeField] private RtlText label;
    [SerializeField] private Button mainButton;
    [SerializeField] private Image mainImage;

    [SerializeField] private Color selected;
    [SerializeField] private Color unselected;
    
    private int _id;
    private bool _isInitialized;

    public int Id => _id;

    public void Initialize()
    {
        if(_isInitialized) return;
        mainButton.onClick.AddListener(SelectButton);
        _isInitialized = true;
    }

    public void BindData(string inLabel, int inId)
    {
        _id = inId;
        label.text = inLabel;
    }

    private void SelectButton() => OnSelectToggleButton?.Invoke(this);

    public void MarkSelected(List<int> dataSubjectsId)
    {
        if (dataSubjectsId.Contains(_id)) Select();
        else Unselect();
    }

    public void Select()
    {
        if(!isToggle) mainButton.interactable = false;
        mainImage.color = selected;
    }

    public void Unselect()
    {
        if(!isToggle) mainButton.interactable = true;
        mainImage.color = unselected;
    }
}