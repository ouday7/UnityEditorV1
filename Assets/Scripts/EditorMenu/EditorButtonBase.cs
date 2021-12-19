﻿using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

public abstract class EditorButtonBase : PoolableObject
{
    protected event Action OnSelectAction;
    protected static event Action OnUpdateIndex;
    
    [SerializeField] protected Button btn;
    [SerializeField] protected RtlText text;
    [SerializeField] protected Image background;
    [SerializeField] protected Button editBtn;
    
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color unselectedColor;
    
    protected bool _isInitialized;
    private bool _isOpen;
    
    private void OnDestroy()
    {
        OnSelectAction = null;
    }

    public virtual void Initialize()
    {
        if(_isInitialized) return;
        btn.onClick.AddListener(OnClickMainButton);
        OnUpdateIndex += UpdateDataIndex;
        _isInitialized = true;
    }

    protected virtual void UpdateDataIndex() { }

    private void OnClickMainButton()
    {
        Select();
        OnSelectAction?.Invoke();
    }
    public void Select()
    {
        _isOpen = true;
        background.color = selectedColor;
        editBtn.gameObject.SetActive(true);
    
        btn.interactable = false;
    }
    public void Unselect()
    {
        _isOpen = false;
        background.color = unselectedColor;
        editBtn.gameObject.SetActive(false);
        btn.interactable = true;
    }

    protected void InvokeUpdateChaptersOrder() => OnUpdateIndex?.Invoke();
}