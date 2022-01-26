using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

public class ChoiceBtn : MonoBehaviour
{
    public event Action<ChoiceBtn> OnClicked;
    public event Action<ChoiceBtn> onSelect;
    public event Action<ChoiceBtn> onUnselect;

    public RtlText btnRtlText;
    public bool updateBtnChange = true;

    private Button _btn;
    private Image _img;
    private bool _isSelected;
    private RectTransform _rectTransform;

    private Image Image
    {
        get
        {
            if (_img == null) _img = GetComponent<Image>();
            return _img;
        }
    }

    private Button Button
    {
        get
        {
            if (_btn == null) _btn = GetComponent<Button>();
            return _btn;
        }
    }

    private RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }

    private void Start()
    {
        _isSelected = false;
        Image.enabled = false;
        Button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        OnClicked?.Invoke(this);
        if (_isSelected)
        {
            onUnselect?.Invoke(this);
            if (!updateBtnChange) UnSelect();
        }
        else
        {
            onSelect?.Invoke(this);
            if (!updateBtnChange) Select();
        }

        _isSelected = !_isSelected;
    }

    public void Select()
    {
        Image.enabled = true;
        RectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        RectTransform.DOScale(Vector3.one, 0.15f).SetEase(Ease.InOutBack);
    }

    public void UnSelect()
    {
        Image.enabled = false;
        RectTransform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        RectTransform.DOScale(Vector3.one, 0.15f).SetEase(Ease.InOutBack);
    }
}