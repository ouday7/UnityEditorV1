using UnityEngine;

namespace EditorMenu
{
    public class BeginHandler : MonoBehaviour
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
            editorButtons.Initialize();
            editorButtons.StartEditor(gm.Data);
        }
    }
}