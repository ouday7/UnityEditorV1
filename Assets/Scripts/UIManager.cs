using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private enum EditedData
    {
        None, Level, Subject, Chapter
    }
    [SerializeField] private InputField inputfiledName;
    [SerializeField] private TMP_InputField inputfilOrder;
    [SerializeField] private GameObject commonSection;
    [SerializeField] private GameObject levelSection;
    [SerializeField] private GameObject subjectSection;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button submitBTn;
    [SerializeField] private GameObject popUpPanel;
    private EditedData _editing;
    private LevelBtn _selectedLevelButton;
    private SubjectsBtn _selectSubjectsBtnButton;
    private ChaptersBtn _selectChaptersBtnBtnButton;
   
    public TMP_Dropdown _DropdownChapter;
    public TMP_Dropdown _DropdownSubjects;



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
    public void SubjectEdit(SubjectsBtn subjectsBtn)
    {
        _selectSubjectsBtnButton = subjectsBtn;
        _editing = EditedData.Subject;
        popUpPanel.gameObject.SetActive(true);
        commonSection.gameObject.SetActive(true);
        levelSection.gameObject.SetActive(true);
        subjectSection.gameObject.SetActive(false);
        SetDropDownSubjectsValues();
        SetDropDownChaptersValues();
    }
    public void ChapterEdit(ChaptersBtn chapter)
    {
        
        _selectChaptersBtnBtnButton = chapter;
        _editing = EditedData.Chapter;
        popUpPanel.gameObject.SetActive(true);
        commonSection.gameObject.SetActive(true);
        levelSection.gameObject.SetActive(true);
        subjectSection.gameObject.SetActive(true);
        SetDropDownSubjectsValues();
        SetDropDownChaptersValues();

        
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
                NewUpdateSubject(inputfiledName.text, inputfilOrder.text);

                break;
            case EditedData.Chapter:
                NewUpdateChapter(inputfiledName.text, inputfilOrder.text);
                break;
        }
        ClosePanel();
        return;
        
       /* 
        if (inputfiledName.text != "")
        {
            Text oldname = GameManager.Instance.selectedLevelBtn.GetComponentInChildren<Text>();
            GameManager.Instance.UpdateData(oldname, inputfiledName.text);
        }*/

        
    }
    private void NewUpdateLevel(string inputFieldNameText, string inputFieldOrderText)
    {
        _selectedLevelButton.UpdateData(inputFieldNameText, inputFieldOrderText);
    }
    private void NewUpdateSubject(string inputFieldNameText, string inputFieldOrderText)
    {
        _selectSubjectsBtnButton.UpdateData(inputFieldNameText, inputFieldOrderText);
    }
    private void NewUpdateChapter(string inputFieldNameText, string inputFieldOrderText)
    {
        _selectChaptersBtnBtnButton.UpdateData(inputFieldNameText, inputFieldOrderText);
    }
    private void SetDropDownChaptersValues()
    {
        _DropdownChapter.options.Clear();

        var subjectsOrders = new List<int>();
        
        for (int i = 0; i < GameManager.Instance.infoListLevels.levels.Length; i++)
        {
            subjectsOrders.Add(GameManager.Instance.infoListLevels.levels[i].id);
        }

        foreach (var subjectsOrder in subjectsOrders)
        {
            _DropdownChapter.options.Add(new TMP_Dropdown.OptionData(){text = subjectsOrder.ToString()});
        }
    }
    private void SetDropDownSubjectsValues()
    {
        _DropdownSubjects.options.Clear();

        var subjectsOrders = new List<int>();
        
        for (int i = 0; i < GameManager.Instance.infoListSubjects.subjects.Length; i++)
        {
            subjectsOrders.Add(GameManager.Instance.infoListSubjects.subjects[i].order);
        }

        foreach (var subjectsOrder in subjectsOrders)
        {
            _DropdownSubjects.options.Add(new TMP_Dropdown.OptionData(){text = subjectsOrder.ToString()});
        }
    }
    private void Verif()
    {
       // if (inputfiledName.text=="")&&(_DropdownChapter.se)
        
    }
}   
  

