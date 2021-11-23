using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

public class UIManager : MonoBehaviour
{
    private enum EditedData
    {
        None, Level, Subject, Chapter
    }
    
    public static UIManager Instance;

    [SerializeField] private InputField inputfiledName;
    [SerializeField] private TMP_InputField inputfilOrder;
    [SerializeField] private TMP_InputField inputfiledLevel;
    [SerializeField] private TMP_InputField inputfiledSubject;
    [SerializeField] private GameObject commonSection;
    [SerializeField] private GameObject levelSection;
    [SerializeField] private GameObject subjectSection;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button submitBTn;
    [SerializeField] private GameObject popUpPanel;
    public LevelBtn _LevelBtn;
    private LevelBtn _selectedLevelButton;
    private EditedData _editing;

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

    public void LevelEdit(LevelBtn levelBtn)
    {
        _selectedLevelButton = levelBtn;
        _editing = EditedData.Level;
        popUpPanel.gameObject.SetActive(true);
        commonSection.gameObject.SetActive(true);
        levelSection.gameObject.SetActive(false);
        subjectSection.gameObject.SetActive(false);
    }

    public void SubjectSelect()
    {
        popUpPanel.gameObject.SetActive(true);
        commonSection.gameObject.SetActive(true);
        levelSection.gameObject.SetActive(true);
        subjectSection.gameObject.SetActive(false);
    }

    public void SelectChapter()
    {
        popUpPanel.gameObject.SetActive(true);
        commonSection.gameObject.SetActive(true);
        levelSection.gameObject.SetActive(true);
        subjectSection.gameObject.SetActive(true);
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

    private void Submit()
    {
        Debug.Log("On Click Submit");
        switch (_editing)
        {
            case EditedData.None:
                break;
            case EditedData.Level:
                Debug.Log("Editing a Button");
                NewUpdateLevel(inputfiledName.text, inputfilOrder.text);
                break;
            case EditedData.Subject:
                break;
            case EditedData.Chapter:
                break;
        }
        ClosePanel();
        return;
        if (inputfiledName.text != "")
        {
            Text oldname = GameManager.Instance.selectedLevelBtn.GetComponentInChildren<Text>();
            GameManager.Instance.UpdateData(oldname, inputfiledName.text);
        }
    }

    private void NewUpdateLevel(string inputFieldNameText, string inputFieldOrderText)
    {
        _selectedLevelButton.UpdateData(inputFieldNameText, inputFieldOrderText);
    }
}   

