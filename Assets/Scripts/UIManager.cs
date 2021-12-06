using System;
using System.Collections.Generic;
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

    //game objects
    [SerializeField] private GameObject popUpPanel;
    [SerializeField] private InputField inputFiledName;
    [SerializeField] private InputField inputFieldOrder;
    [SerializeField] private GameObject dataInput;
    [SerializeField] private GameObject levelSection;
    [SerializeField] private GameObject subjectSection;


    //buttons
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button submitBTn;
    
    //holders
    [SerializeField] private Transform holderSubject;
    [SerializeField] private Transform holderLevel;
    
    private EditedData _editing;
    private LevelBtn _selectedLevelButton;
    private SubjectsBtn _selectSubjectsBtnButton;
    private ChaptersBtn _selectChaptersBtnBtnButton;
    private List<Subject> _availableSubjects;
    private List<Level> _availableLevels;
    private Button currentButton;
    private RtlText namePlaceHolder;

    private List<int> selectedSubjects;
    private List<Transform> currentSubjectsToggle;
    private List<Transform> currentLevelsToggle;
    private Vector2 subjectsholderposition;

    public void Initialize()
    {
        if (Instance != null) return;
        Instance = this;
        closeBtn.onClick.AddListener(ClosePanel);
        popUpPanel.gameObject.SetActive(false);
        submitBTn.onClick.AddListener(Submit);
        namePlaceHolder = inputFiledName.placeholder.GetComponent<RtlText>();
        subjectsholderposition = subjectSection.transform.position;
        currentSubjectsToggle=new List<Transform>();
        currentLevelsToggle=new List<Transform>();
    }
    private void ClosePanel()
    {
        popUpPanel.gameObject.SetActive(false);
        ResetPopup();
    }

    public void LevelEdit(LevelBtn levelBtn)
    {
        _selectedLevelButton = levelBtn;
        _editing = EditedData.Level;
        popUpPanel.gameObject.SetActive(true);
        dataInput.gameObject.SetActive(true);
        levelSection.gameObject.SetActive(false);
        subjectSection.gameObject.SetActive(true);
        subjectSection.transform.position = levelSection.transform.position;
        namePlaceHolder.text = levelBtn.Data.name;
        selectedSubjects = _selectedLevelButton.Data.subjectsId;
        
            foreach (var child in currentSubjectsToggle)
             {
               ObjectPooler.Instance.DeSpawn(child);
             }
        
        foreach (var subject in GameDataManager.Instance.Data.subjects)
        {
            var toggleButton = ObjectPooler.Instance.Spawn<ToggleButton>(ObjectToPoolType.Toggle);
            var newBtn = toggleButton.GetComponent<ToggleButton>();
            newBtn.Initialize();
            newBtn.BindData(subject.name, subject.id);
            newBtn._isToggle = true;
            newBtn.Transform.SetParent(holderSubject.transform);
            currentSubjectsToggle.Add(newBtn.transform);
            
            toggleButton.OnClickToggle += OnClickSubjectToggle;

            
            // call current toggle state & apply changes
            toggleButton.MarkSelected(_selectedLevelButton.Data.subjectsId);
        }
        
    }
    private void OnClickSubjectToggle(ToggleButton newSubject)
    {
        if (selectedSubjects.Contains(newSubject.Id))
        {
            selectedSubjects.Remove(newSubject.Id);
            newSubject.Unselect();
        }
        else
        {
            selectedSubjects.Add(newSubject.Id);
            newSubject.Select();
        }
    }
    
    public void SubjectEdit(SubjectsBtn subjectsBtn)
    {
        subjectSection.transform.position = subjectsholderposition;
        _selectSubjectsBtnButton = subjectsBtn;
        _editing = EditedData.Subject;
        popUpPanel.gameObject.SetActive(true);
        dataInput.gameObject.SetActive(true);
        levelSection.gameObject.SetActive(false);
        subjectSection.gameObject.SetActive(false);

        namePlaceHolder.text = subjectsBtn.Data.name;
    }

    public void ChapterEdit(ChaptersBtn chapter)
    {
       //unfinished function, while trying to finish it project was about to crush
        subjectSection.transform.position = subjectsholderposition;
        
        foreach (var child in currentLevelsToggle)
        {
            ObjectPooler.Instance.DeSpawn(child);
        }

        foreach (var child in currentSubjectsToggle)
        {
            ObjectPooler.Instance.DeSpawn(child);
        }

        _selectChaptersBtnBtnButton = chapter;
        _editing = EditedData.Chapter;
        popUpPanel.gameObject.SetActive(true);
        dataInput.gameObject.SetActive(true);
        levelSection.gameObject.SetActive(true);
        subjectSection.gameObject.SetActive(true);
        namePlaceHolder.text = chapter.Data.name;

        foreach (var lvl in GameDataManager.Instance.Data.levels)
        {
            var toggleButton = ObjectPooler.Instance.Spawn<ToggleButton>(ObjectToPoolType.Toggle);
            var newBtn = toggleButton.GetComponent<ToggleButton>();
            newBtn.CheckToggleState(lvl.id,chapter.Data.levelId);
            newBtn.Initialize();
            newBtn.BindData(lvl.name, lvl.id);
            newBtn._isToggle = true;
            newBtn.Transform.SetParent(holderLevel.transform);
            currentLevelsToggle.Add(newBtn.transform);
           
        }
}
    private void Submit()
    {
        Debug.Log("On Click Submit");
        switch (_editing)
        {
            case EditedData.None:
                break;
            case EditedData.Level:
                NewUpdateLevel(inputFiledName.text, inputFieldOrder.text);

                break;
            case EditedData.Subject:
                NewUpdateSubject(inputFiledName.text, inputFieldOrder.text);
                break;
            case EditedData.Chapter:
                NewUpdateChapter(inputFiledName.text, inputFieldOrder.text);
                break;
        }

        ClosePanel();
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

    private void ResetPopup()
    {
        inputFiledName.text = "";
        inputFieldOrder.text = "";
    }
}