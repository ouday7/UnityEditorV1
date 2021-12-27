using UnityEngine;
using UnityEngine.UI;

public class TemplateBtn : MonoBehaviour
{
    [SerializeField] public Image icon;
    [SerializeField] public Text title;
    private Button _btn;

    
    
    private TemplateDataInformation _template;
    public TemplateDataInformation Template => _template;
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

    public void  Initialize(TemplateDataInformation template)
    {
        this._template = _template;
        Debug.Log(_template.id);
        this.title.text = _template.name;
        this.icon.sprite = _template.icon;
        Unselect();
        button.onClick.AddListener(OnSelectTemplate);
    }


    private void OnSelectTemplate()
    {
        // onSelect?.Invoke(this);
    }

    private void OnSubmitTemplate()
    {
        // onSubmit?.Invoke(this, this._data);
    }

    public void Select()
    {
    }

    public void Unselect()
    {
    
    }
    
}