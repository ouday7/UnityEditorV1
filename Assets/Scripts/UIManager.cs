using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

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
    public TMP_Dropdown subjectsDropDown;
    public TMP_Dropdown levelsDropDown;
    private List<Subject> _availableSubjects;
    private List<Level> _availableLevels;

    private Subject _selectedSubject;
    private Level _SelectedLevel;

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
        inputfiledName.placeholder.GetComponent<RtlText>().text = levelBtn.Data.name;


    }
    public void SubjectEdit(SubjectsBtn subjectsBtn)
    {
        _selectSubjectsBtnButton = subjectsBtn;
        _editing = EditedData.Subject;
        popUpPanel.gameObject.SetActive(true);
        commonSection.gameObject.SetActive(true);
        levelSection.gameObject.SetActive(true);
        subjectSection.gameObject.SetActive(false);
        SetLevelDropDown();
        SetSubjectsDropDown();
        inputfiledName.placeholder.GetComponent<RtlText>().text = subjectsBtn.Data.name;

    }
    public void ChapterEdit(ChaptersBtn chapter)
    {
        
        _selectChaptersBtnBtnButton = chapter;
        _editing = EditedData.Chapter;
        popUpPanel.gameObject.SetActive(true);
        commonSection.gameObject.SetActive(true);
        levelSection.gameObject.SetActive(true);
        subjectSection.gameObject.SetActive(true);
        SetLevelDropDown();
        SetSubjectsDropDown();
        inputfiledName.placeholder.GetComponent<RtlText>().text = chapter.Data.name;

        
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
                NewUpdateLevel(inputfiledName.text, inputfilOrder.text,_SelectedLevel);
                
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
    private void NewUpdateLevel(string inputFieldNameText, string inputFieldOrderText,Level newLevel)
    {
        _selectedLevelButton.UpdateData(inputFieldNameText, inputFieldOrderText,newLevel);
    }
    private void NewUpdateSubject(string inputFieldNameText, string inputFieldOrderText)
    {
        _selectSubjectsBtnButton.UpdateData(inputFieldNameText, inputFieldOrderText);
    }
    private void NewUpdateChapter(string inputFieldNameText, string inputFieldOrderText)
    {
        _selectChaptersBtnBtnButton.UpdateData(inputFieldNameText, inputFieldOrderText);
    }
    private void SetSubjectsDropDown()
    {
        subjectsDropDown.options.Clear();
        _availableSubjects = GameManager.Instance.infoListSubjects.subjects.ToList();
        
        for (var i = 0; i < _availableSubjects.Count; i++)
        {
            var subject = _availableSubjects[i];
            subjectsDropDown.options.Add(
                new TMP_Dropdown.OptionData {text = subject.name});
        }
        
        subjectsDropDown.onValueChanged.AddListener(SelectSubject);
    }

    private void SelectSubject(int selectedIndex)
    {
        _selectedSubject = _availableSubjects[selectedIndex];
    }

    private void SetLevelDropDown()
    {
        levelsDropDown.options.Clear();
        _availableLevels = GameManager.Instance.infoListLevels.levels.ToList();
        
        for (var i = 0; i < _availableLevels.Count; i++)
        {
            var level = _availableLevels[i];
            _availableLevels.Add(level);
            levelsDropDown.options.Add(
                new TMP_Dropdown.OptionData {text = level.name});
        }
        
        levelsDropDown.onValueChanged.AddListener(SelectChapter);
    }
    private void SelectChapter(int selectedIndex)
    {
        _SelectedLevel = _availableLevels[selectedIndex];
    }
    private void SelectLevel(int levelIndex)
    {
    }

    private void Verif()
    {
       // if (inputfiledName.text=="")&&(_DropdownChapter.se)
        
    }
}   
  

