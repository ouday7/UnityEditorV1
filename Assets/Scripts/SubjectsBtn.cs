using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;


public class SubjectsBtn : MonoBehaviour
{
    [SerializeField] private Button editBtn;
    [SerializeField] private Button btn;
    [SerializeField] private RtlText _text;

    private void Start()
    { 
        editBtn.onClick.AddListener(() => UIManager.Instance.SubjectSelect());
    }
}
