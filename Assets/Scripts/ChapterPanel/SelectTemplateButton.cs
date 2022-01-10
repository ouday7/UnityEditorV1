using System;
using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;


    public class SelectTemplateButton : MonoBehaviour
    {
        public static SelectTemplateButton instance;
        [SerializeField] public Image templateIcon;
        [SerializeField] public RtlText templateNameTxt;
        private EditManager _parent;
        private TemplateData _data;


      public void Begin()
      {
          if(instance!=null) return;
          instance = this;
        Initialize(EditManager);
      }

      private EditManager EditManager { get; set; }

      private void Initialize(EditManager parent)
      {
          this._parent = parent;
      }
      public void SetTemplate(TemplateData data)
      {
          Debug.Log("category : "+data.category);
          this._data = data;
          templateIcon.sprite = data.icon;
          templateNameTxt.text = data.name;
      }
    }
