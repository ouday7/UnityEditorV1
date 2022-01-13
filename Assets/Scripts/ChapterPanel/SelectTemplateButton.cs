using System;
using ChapterPanel;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;


    public class SelectTemplateButton : EntryPointSystemBase
    {
        public static SelectTemplateButton instance;
        [SerializeField] public Image templateIcon;
        [SerializeField] public RtlText templateNameTxt;
        public int tempid;
        private EditManager _parent;
        private TemplateData _data;


      public override void Begin()
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
