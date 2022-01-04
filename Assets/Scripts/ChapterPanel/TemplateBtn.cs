using System;
using UnityEngine;
using UnityEngine.UI;

public class TemplateBtn : MonoBehaviour
{
    public static event Action<TemplateBtn> onSelect;
    public static event Action<TemplateBtn, TemplateData> onSubmit;

    private bool _submitted;
    [SerializeField] public Image icon;
    [SerializeField] public Text title;
    [SerializeField] private Button selectBtn;
    
    private TemplateData _data;
    public TemplateData Data => _data;
    private Button _btn;

    private Button button
    {
        get
        {
            if (_btn == null) _btn = GetComponent<Button>();
            return _btn;
        }
    }


    
    public void Initialize(TemplateData data)
    {
        this._data = data;
        this.icon.sprite = _data.icon;
        this.title.text = _data.name;
        _submitted = false;
        Unselect();
        selectBtn.onClick.AddListener(OnSubmitTemplate);
        button.onClick.AddListener(OnSelectTemplate);
    }


    private void OnSelectTemplate()
    {
        onSelect?.Invoke(this);
    }

    private void OnSubmitTemplate()
    {
        selectBtn.interactable = false;
        onSubmit?.Invoke(this,this._data);
    }

    public void Select()
    {
        selectBtn.gameObject.SetActive(true);
    }

    public void Unselect()
    {
        if(_submitted) return;
            
        selectBtn.interactable = true;
        selectBtn.gameObject.SetActive(false);
    }

    public void SetVisibility(bool status)
    {
        this.gameObject.SetActive(status);
    }
    
}