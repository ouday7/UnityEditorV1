using System;
using System.Collections.Generic;
using EditorMenu;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UPersian.Components;

public class ToggleButton : PoolableObject
{
    public event Action<ToggleButton> OnClickToggle;

    [HideInInspector] public bool isToggle; 
    
    [SerializeField] private RtlText toggleText;
    [SerializeField] private Button toggleButton;
    [SerializeField] private Image mainImage;
    [SerializeField] private Color selectedCase;
    [SerializeField] private Color unselectedCase;
    
    private int _id;
    private bool _isInitialized;
    public bool IsSelected { get; private set; }
    public int Id => _id;

    public void Initialize()
    {
        if(_isInitialized) return;
        toggleButton.onClick.AddListener(() => OnClickToggle?.Invoke(this));
        _isInitialized = true;
    }

    public void BindData(string inLabel, int inId)
    {
        _id = inId;
        toggleText.text = inLabel;
    }

    public void CheckToggleState(int inIdToCompare)
    {
        if (_id == inIdToCompare) 
            Select();
        else 
            Unselect();
    }

    public void MarkSelected(bool isSelected)
    {
        if (isSelected) 
            Select();
        else 
            Unselect();
    }

    public void Select()
    {
        if(!isToggle) toggleButton.interactable = false;
        mainImage.color = selectedCase;
        IsSelected = true;
    }

    public void Unselect()
    {
        if(!isToggle) toggleButton.interactable = true;
        mainImage.color = unselectedCase;
        IsSelected = false;
    }

    public void RemoveEventListeners() => OnClickToggle = null;

    public override void Spawn()
    {
        Debug.Log($"//. Spawn {gameObject.name}");
    }

    public override void DeSpawn()
    {
        Debug.Log($"//. DeSpawn {gameObject.name}");
    }
}