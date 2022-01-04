using System;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;


    public class SelectTemplateButton : MonoBehaviour
    {
        [SerializeField] public Image templateIcon;
      [SerializeField] public RtlText templateNameTxt;
      private EditManager _parent;
      private TemplateData _data;


      public void Start()
      {
        Initialize(EditManager);
      }

      private EditManager EditManager { get; set; }

      private void Initialize(EditManager parent)
      {
          this._parent = parent;
          var btn = GetComponent<Button>();
          btn.onClick.AddListener(OnClick);
          //templateNameTxt.text = "test";

      }
      private void OnClick()
      {
          
      }
      
      
      public void SetTemplate(TemplateData data)
      {
          Debug.Log("category : "+data.category);
          this._data = data;
          templateIcon.sprite = data.icon;
          templateNameTxt.text = data.name;
      }
    }
