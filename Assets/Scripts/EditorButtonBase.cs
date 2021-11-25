using System;
using UnityEngine;
using UnityEngine.UI;

    public enum ButtonTypes
    {
        Level, Subject, Chapter
    }
    
    public abstract class EditorButtonBase : MonoBehaviour
    {
        public static event Action<EditorButtonBase> OnClickEdit;
        public ButtonTypes type;
        
        public Transform bodyTransform;
        protected Button mainButton;
        protected Button editButton;

        public abstract void Initialize();
    }
