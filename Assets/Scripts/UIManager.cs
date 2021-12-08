using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    

    private List<Transform> currentSubjectsToggle;
    private List<Transform> currentLevelsToggle;
    private Vector2 subjectsholderposition;
    
    private List<int> _levelSubjects;
    private List<int> _selectedSubjects;

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
        
        _levelSubjects = _selectedLevelButton.Data.subjectsId;
        _selectedSubjects = new List<int>(_levelSubjects);
        
        
        foreach (var child in currentSubjectsToggle)
        {
           ObjectPooler.Instance.DeSpawn(child);
        }
        
        foreach (var subject in GameDataManager.Instance.Data.subjects)
        {
            var newBtn = ObjectPooler.Instance.Spawn<ToggleButton>(ObjectToPoolType.Toggle);
            newBtn.Initialize();
            newBtn.BindData(subject.name, subject.id);
            newBtn._isToggle = true;
            newBtn.Transform.SetParent(holderSubject.transform);
            currentSubjectsToggle.Add(newBtn.transform);
            
            newBtn.OnClickToggle += OnClickSubjectToggle;
            
            // call current toggle state & apply changes
            var state = _selectedLevelButton.Data.subjectsId.Contains(subject.id);
            newBtn.MarkSelected(state);
        }
    }

    private void OnClickSubjectToggle(ToggleButton newSubjectButton)
    {
        if (newSubjectButton.IsSelected)
        {
            _selectedSubjects.Remove(newSubjectButton.Id);
            newSubjectButton.Unselect();
        }
        else
        {
            _selectedSubjects.Add(newSubjectButton.Id);
            newSubjectButton.Select();
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
            var newBtn = ObjectPooler.Instance.Spawn<ToggleButton>(ObjectToPoolType.Toggle);
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

        foreach (var subject in GameDataManager.Instance.Data.subjects)
        {
            if (_selectedSubjects.Contains(subject.id))
            {
                if (!_levelSubjects.Contains(subject.id))
                {
                    _levelSubjects.Add(subject.id);
                    _selectedLevelButton.Data.subjects.Add(subject);
                }
            }
            else if (!_selectedSubjects.Contains(subject.id))
            {
                if (_levelSubjects.Contains(subject.id))
                {
                    var item = _selectedLevelButton.Data.subjects.FirstOrDefault(sub => sub.id == subject.id);
                    _selectedLevelButton.Data.subjects.Remove(item);
                    _levelSubjects.Remove(subject.id);
                }

            }

        }
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