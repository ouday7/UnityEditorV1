using EditorMenu;
using UnityEngine;

public class BeginHandler : MonoBehaviour
{
    [SerializeField]private PoolSystem pool;
    [SerializeField]private GameDataManager gm;
    [SerializeField] private UIManager ui;
    [SerializeField] private EditorButtonsManager editorButtons;

    private void Awake()
    {
        pool.Initialize();
        gm.Initialize();
        ui.Initialize();
        editorButtons.Initialize();
        editorButtons.StartEditor(gm.Data);
    }
}