﻿using UnityEngine;

namespace EditorMenu
{
    public class MainEditorEntryPoint : MonoBehaviour
    {
        [SerializeField]private PoolSystem pool;
        [SerializeField]private GameDataManager gm;
        [SerializeField] private PopUpManager popUp;
        [SerializeField] private EditorButtonsManager editorButtons;

        private void Awake()
        {
            pool.Initialize();
            gm.Initialize();
            popUp.Initialize();
            gm.GenerateData();
            editorButtons.Initialize();
            editorButtons.StartEditor(gm.Data);
        }
    }
}