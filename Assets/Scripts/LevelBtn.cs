using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;


public class LevelBtn : MonoBehaviour
{

    public static LevelBtn Instance;
    [SerializeField] private Button editBtn;
    [SerializeField] private Button btn;
    [SerializeField] private RtlText _text;
     


    private void Start()
    {
        Instance = this;
        editBtn.onClick.AddListener(() => ManagerUI.Instance.LevelSelect());
        
    }

    public void GetData()
    {

    }
}
