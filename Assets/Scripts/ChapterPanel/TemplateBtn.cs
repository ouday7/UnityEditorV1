using System;
using UnityEngine;
using UnityEngine.UI;

public class TemplateBtn : MonoBehaviour
{
    
    
    
    public static event Action<TemplateBtn> onSelect;
    public static event Action<TemplateBtn, TemplateDataInformation> onSubmit;
    
    private bool _submitted;

    
    [SerializeField] public Image icon;
    [SerializeField] public Text title;
    [SerializeField] private Button selectBtn;

    private Button _btn;
    
    private Button button
    {
        get
        {
            if (_btn == null) _btn = GetComponent<Button>();
            return _btn;
        }
    }

    private TemplateDataInformation _data;
    public TemplateDataInformation Data => _data;
    public void  Initialize(TemplateDataInformation data)
    {

        this._data = data;
        this.icon.sprite = _data.icon;
        this.title.text = _data.templateName.ToString();
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
    
    public void SetMarkStatus(bool status)
    {
        if(!_submitted) selectBtn.interactable = status;
    }

    public void Submitted(bool status)
    {
        this._submitted = status;
        if (!status)
        {
            Unselect();
        }
    }
    
}