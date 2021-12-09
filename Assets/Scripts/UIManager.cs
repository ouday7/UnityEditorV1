using System;
using System.Collections.Generic;
using System.Linq;
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
    private ChaptersBtn _selectChaptersButton;
    private List<Subject> _availableSubjects;
    private List<Level> _availableLevels;
    private Button currentButton;
    private RtlText namePlaceHolder;
    

    private List<Transform> currentSubjectsToggle;
    private List<Transform> currentLevelsToggle;
    private Vector2 subjectsholderposition;
    
    private List<int> _levelSubjects;
    private List<int> _selectedSubjects;

    private int selectedLevel;
    private int selectedSubject;

    private ToggleButton oldLevelBtn;
    private ToggleButton oldSubjectBtn;

    private ToggleButton currentChapter;
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

    private void OnClickLevelToggle(ToggleButton newLevelButton)
    {
        selectedLevel = newLevelButton.Id;
        if(oldLevelBtn!=null) oldLevelBtn.Unselect();
        newLevelButton.Select();
        newLevelButton._isToggle = false;
        oldLevelBtn = newLevelButton;

        foreach (var sub in currentSubjectsToggle)
        {
            ObjectPooler.Instance.DeSpawn(sub);
        }
        
        foreach (var sub in GameDataManager.Instance.Data.subjects)
        {
            var newBtn = ObjectPooler.Instance.Spawn<ToggleButton>(ObjectToPoolType.Toggle);
            newBtn.Initialize();
            newBtn.BindData(sub.name, sub.id);
            newBtn.CheckToggle(sub.id,_selectChaptersButton.Data.subjectId);
            newBtn._isToggle = true;
            if (newBtn.IsSelected)
            {
                oldSubjectBtn = newBtn;
                newBtn._isToggle = false;
            }
            newBtn.Transform.SetParent(holderSubject.transform);
            currentSubjectsToggle.Add(newBtn.transform);
            newBtn.OnClickToggle += OnClickSubjectLevel;
        }
    }
    private void OnClickSubjectLevel(ToggleButton newSubjectButton)
    {
        selectedSubject = newSubjectButton.Id;
        if(oldSubjectBtn!=null) oldSubjectBtn.Unselect();
        newSubjectButton.Select();
        newSubjectButton._isToggle = false;
        oldSubjectBtn = newSubjectButton;

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
        _selectChaptersButton = chapter;
         _editing = EditedData.Chapter;
         popUpPanel.gameObject.SetActive(true);
         dataInput.gameObject.SetActive(true);
         levelSection.gameObject.SetActive(true);
         subjectSection.gameObject.SetActive(true);
         namePlaceHolder.text = chapter.Data.name;
         subjectSection.transform.position = subjectsholderposition;
         
        
         foreach (var child in currentLevelsToggle)
         {
             ObjectPooler.Instance.DeSpawn(child);
         }
 
         foreach (var child in currentSubjectsToggle)
         {
             ObjectPooler.Instance.DeSpawn(child);
         }
         
         foreach (var lvl in GameDataManager.Instance.Data.levels)
         {
             var newBtn = ObjectPooler.Instance.Spawn<ToggleButton>(ObjectToPoolType.Toggle);
             newBtn.Initialize();
             newBtn.BindData(lvl.name, lvl.id);
             newBtn.CheckToggleState(lvl.id,chapter.Data.levelId);
             newBtn._isToggle = true;
             newBtn.Transform.SetParent(holderLevel.transform);
             currentLevelsToggle.Add(newBtn.transform);
             if (newBtn.IsSelected)
             {
                 oldLevelBtn = newBtn;
                 newBtn._isToggle = false;
             }
             newBtn.OnClickToggle += OnClickLevelToggle;
         }
    }

    private void Submit()
    {
        switch (_editing)
        {
            case EditedData.None:
                break;
            
            case EditedData.Level:
                UpdateLevel(inputFiledName.text, inputFieldOrder.text);
                break;
            
            case EditedData.Subject:
                UpdateSubject(inputFiledName.text, inputFieldOrder.text);
                break;
            
            case EditedData.Chapter:
                UpdateChapter(inputFiledName.text, inputFieldOrder.text);
                break;
        }

        ClosePanel();
    }

    private void UpdateLevel(string inputFieldNameText, string inputFieldOrderText)
    {
      //  if (inputFieldNameText == "" && inputFieldOrderText == "") return;
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

    private void UpdateSubject(string inputFieldNameText, string inputFieldOrderText)
    {
        _selectSubjectsBtnButton.UpdateData(inputFieldNameText, inputFieldOrderText);
    }

    private void UpdateChapter(string inputFieldNameText, string inputFieldOrderText)
    {
        _selectChaptersButton.UpdateData(inputFieldNameText, inputFieldOrderText);
        
        Debug.Log(_selectSubjectsBtnButton.Data.chapters);
        _selectSubjectsBtnButton.Data.chapters.Remove(_selectChaptersButton.Data);
        
        foreach (var lvl in GameDataManager.Instance.Data.levels)
        {
            if (lvl.id != selectedLevel || !lvl.subjectsId.Contains(selectedSubject)) continue;
            _selectSubjectsBtnButton.Data.chapters.Add(_selectChaptersButton.Data);
        }
        _selectChaptersButton.Data.levelId = selectedLevel;
        _selectChaptersButton.Data.subjectId = selectedSubject;
    }

    private void ResetPopup()
    {
        inputFiledName.text = "";
        inputFieldOrder.text = "";
    }
    
}