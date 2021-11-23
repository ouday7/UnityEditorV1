using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

public class ManagerUI : MonoBehaviour
{
    public static ManagerUI Instance;

    [SerializeField] private InputField inputfiledName;
    [SerializeField] private TMP_InputField inputfilOrder;
    [SerializeField] private TMP_InputField inputfiledLevel;
    [SerializeField] private TMP_InputField inputfiledSubject;
    [SerializeField] private GameObject levelPanel;
    [SerializeField] private GameObject subjectPanel;
    [SerializeField] private GameObject cheapterPanel;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button submitBTn;
    [SerializeField] private GameObject popUpPanel;
    public LevelBtn _LevelBtn;

    private void Awake()
    {

        if (Instance != null) return;
        Instance = this;

    }

    void Start()
    {
        closeBtn.onClick.AddListener(ClosePanel);
        popUpPanel.gameObject.SetActive(false);
        submitBTn.onClick.AddListener(Submit);

    }

    void ClosePanel()
    {
        popUpPanel.gameObject.SetActive(false);
    }

    public void LevelSelect()
    {

        popUpPanel.gameObject.SetActive(true);
        levelPanel.gameObject.SetActive(true);
        subjectPanel.gameObject.SetActive(false);
        cheapterPanel.gameObject.SetActive(false);


    }

    public void SubjectSelect()
    {
        popUpPanel.gameObject.SetActive(true);
        levelPanel.gameObject.SetActive(true);
        subjectPanel.gameObject.SetActive(true);
        cheapterPanel.gameObject.SetActive(false);


    }

    public void CheapSelect()
    {
        popUpPanel.gameObject.SetActive(true);
        levelPanel.gameObject.SetActive(true);
        subjectPanel.gameObject.SetActive(true);
        cheapterPanel.gameObject.SetActive(true);
    }

    public void UpdateLevel()
    {

    }

    public void UpdateSubject()
    {

    }

    public void Updatecheapter()
    {

    }

    public void Submit()
    {
        if (inputfiledName.text != "")
        {
            Text oldname = GameManager.Instance.levelbtn.GetComponentInChildren<Text>();
            GameManager.Instance.Updatedata(oldname, inputfiledName.text);

        }
    }
}   

