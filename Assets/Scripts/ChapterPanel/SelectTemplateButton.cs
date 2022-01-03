using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;


    public class SelectTemplateButton : MonoBehaviour
    {
        [SerializeField] public Image templateIcon;
      [SerializeField] public RtlText templateNameTxt;
      private EditManager _parent;
      private TemplateDataInformation _data;
    
      public void Initialize(EditManager parent)
      {
          this._parent = parent;
          var btn = GetComponent<Button>();
          btn.onClick.AddListener(OnClick);
          templateNameTxt.text = "test";

      }
      private void OnClick()
      {
          
      }
      
      public void SetTemplate(TemplateDataInformation data)
      {
          Debug.Log(data.category+"categ");
          this._data = data;
          templateIcon.sprite = data.icon;
          templateNameTxt.text = data.name;
      }
    }
