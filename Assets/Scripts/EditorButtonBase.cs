using System;
using UnityEngine;
using UnityEngine.UI;

    public enum ButtonTypes
    {
        Level, Subject, Chapter
    }

    public abstract class EditorButtonBase : MonoBehaviour
    {
        public abstract void Initialize();
    }
