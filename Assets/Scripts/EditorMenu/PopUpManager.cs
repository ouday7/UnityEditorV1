using System.Collections.Generic;
using System.Linq;
using ChapterPanel;
using EditorMenu;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;
using DG.Tweening;


public class PopUpManager : MonoBehaviour
{
    public static PopUpManager instance;

    private enum EditedData
    {
        None,
        Level,
        Subject,
        Chapter
    }

    //game objects
    [SerializeField] private RectTransform popUpPanel;
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
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    private EditedData _editing;
    private LevelButton _selectedLevelButton;
    private SubjectButton selectSubjectButton;
    private ChapterButton _selectChaptersButton;
    private List<SubjectData> _availableSubjects;
    private List<LevelData> _availableLevels;
    private Button _currentButton;
    private RtlText _namePlaceHolder;
    private List<Transform> _currentSubjectsToggle;
    private List<Transform> _currentLevelsToggle;
    private Vector2 _subjectsHolderPosition;
    private List<int> _levelSubjects;
    private List<int> _selectedSubjects;
    private int _selectedLevelId;
    private int _selectedSubjectId;
    private ToggleButton _selectedLevelBtn;
    private ToggleButton _oldSubjectBtn;
    private ToggleButton _currentChapter;

    public void Initialize()
    {
        if (instance != null) return;
        instance = this;
        closeBtn.onClick.AddListener(ClosePanel);
        popUpPanel.gameObject.SetActive(false);
        submitBTn.onClick.AddListener(Submit);
        _namePlaceHolder = inputFiledName.placeholder.GetComponent<RtlText>();

        _subjectsHolderPosition = subjectSection.transform.position;
        _currentSubjectsToggle = new List<Transform>();
        _currentLevelsToggle = new List<Transform>();
    }

    private void ClosePanel()
    {
        popUpPanel.DOAnchorPos(new Vector2(-2000, 0), duration).SetEase(ease).OnComplete(() =>
        {
            popUpPanel.gameObject.SetActive(false); 
            ResetPopup();
        });
    }

    public void LevelEdit(LevelButton levelButton) 
    {
        _selectedLevelButton = levelButton;
        _editing = EditedData.Level;
        popUpPanel.gameObject.SetActive(true);
        popUpPanel.DOAnchorPos(new Vector2(0, 0), duration).SetEase(ease);
        dataInput.gameObject.SetActive(true);
        levelSection.gameObject.SetActive(false);
        subjectSection.gameObject.SetActive(true);
        subjectSection.transform.position = levelSection.transform.position;
        _namePlaceHolder.text = levelButton.Data.name;

        _levelSubjects = _selectedLevelButton.Data.subjectsId;
        _selectedSubjects = new List<int>(_levelSubjects);

        foreach (var child in _currentSubjectsToggle)
        {
            PoolSystem.instance.DeSpawn(child);
        }

        foreach (var subject in GameDataManager.instance.Data.subjects)
        {
            var newBtn = PoolSystem.instance.Spawn<ToggleButton>(ObjectToPoolType.Toggle);
            newBtn.Initialize();
            newBtn.BindData(subject.name, subject.id);
            newBtn.isToggle = true;
            newBtn.Transform.SetParent(holderSubject.transform);
            newBtn.Transform.localScale = Vector3.one;
            _currentSubjectsToggle.Add(newBtn.transform);
            newBtn.RemoveEventListeners();
            newBtn.OnClickToggle += OnClickSubjectToggle;
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
        if (_selectedLevelBtn != null) _selectedLevelBtn.Unselect();
        _selectedLevelBtn = newLevelButton;
        _selectedLevelBtn.Select();
        _selectedLevelId = _selectedLevelBtn.Id;

        foreach (var sub in _currentSubjectsToggle)
        {
            PoolSystem.instance.DeSpawn(sub);
        }

        _currentSubjectsToggle?.Clear();

        var level = GameDataManager.instance.Data.levels.FirstOrDefault(lev => lev.id == _selectedLevelBtn.Id);

        if (level == null)
        {
            return;
        }

        foreach (var sub in level.Subjects)
        {
            var newBtn = PoolSystem.instance.Spawn<ToggleButton>(ObjectToPoolType.Toggle);
            newBtn.Initialize();
            newBtn.BindData(sub.Value.name, sub.Value.id);
            newBtn.CheckToggleState(_selectChaptersButton.Data.subjectId);
            newBtn.Transform.SetParent(holderSubject.transform);
            newBtn.Transform.localScale = Vector3.one;
            _currentSubjectsToggle.Add(newBtn.transform);
            newBtn.RemoveEventListeners();
            newBtn.OnClickToggle += OnClickSubjectLevel;
            if (newBtn.IsSelected) OnClickSubjectLevel(newBtn);
        }
    }

    private void OnClickSubjectLevel(ToggleButton newSubjectButton)
    {
        _selectedSubjectId = newSubjectButton.Id;
        if (_oldSubjectBtn != null) _oldSubjectBtn.Unselect();
        newSubjectButton.Select();
        _oldSubjectBtn = newSubjectButton;
    }

    public void SubjectEdit(SubjectButton subjectButton)
    {
        subjectSection.transform.position = _subjectsHolderPosition;
        selectSubjectButton = subjectButton;
        _editing = EditedData.Subject;
        popUpPanel.gameObject.SetActive(true);
        popUpPanel.DOAnchorPos(new Vector2(0, 0), duration).SetEase(ease);
        dataInput.gameObject.SetActive(true);
        levelSection.gameObject.SetActive(false);
        subjectSection.gameObject.SetActive(false);

        _namePlaceHolder.text = subjectButton.Data.name;
    }

    public void ChapterEdit(ChapterButton chapter)
    {
        _selectChaptersButton = chapter;
        _editing = EditedData.Chapter;
        popUpPanel.gameObject.SetActive(true);
        popUpPanel.DOAnchorPos(new Vector2(0, 0), duration).SetEase(ease);
        dataInput.gameObject.SetActive(true);
        levelSection.gameObject.SetActive(true);
        subjectSection.gameObject.SetActive(true);
        _namePlaceHolder.text = _selectChaptersButton.Data.name;
        subjectSection.transform.position = _subjectsHolderPosition;
        _selectedLevelId = _selectChaptersButton.Data.levelId;
        _selectedSubjectId = _selectChaptersButton.Data.subjectId;

        foreach (var child in _currentLevelsToggle)
        {
            PoolSystem.instance.DeSpawn(child);
        }

        foreach (var child in _currentSubjectsToggle)
        {
            PoolSystem.instance.DeSpawn(child);
        }

        foreach (var lvl in GameDataManager.instance.Data.levels)
        {
            var newBtn = PoolSystem.instance.Spawn<ToggleButton>(ObjectToPoolType.Toggle);
            newBtn.Initialize();
            newBtn.BindData(lvl.name, lvl.id);
            newBtn.CheckToggleState(chapter.Data.levelId);
            newBtn.Transform.SetParent(holderLevel.transform);
            newBtn.Transform.localScale = Vector3.one;
            _currentLevelsToggle.Add(newBtn.transform);
            newBtn.RemoveEventListeners();
            newBtn.OnClickToggle += OnClickLevelToggle;
            if (!newBtn.IsSelected) continue;
            OnClickLevelToggle(newBtn);
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
                EditorButtonsManager.instance.ShowSubjects(_selectedLevelButton.Data);
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
        _selectedLevelButton.UpdateData(inputFieldNameText, inputFieldOrderText);

        foreach (var subject in GameDataManager.instance.Data.subjects)
        {
            if (_selectedSubjects.Contains(subject.id))
            {
                if (_levelSubjects.Contains(subject.id)) continue;
                _levelSubjects.Add(subject.id);
                _selectedLevelButton.Data.Subjects.Add(subject.id, subject);
                continue;
            }

            if (_selectedSubjects.Contains(subject.id)) continue;
            if (!_levelSubjects.Contains(subject.id)) continue;

            if (_selectedLevelButton.Data.Subjects.ContainsKey(subject.id))
            {
                _selectedLevelButton.Data.Subjects.Remove(subject.id);
                _levelSubjects.Remove(subject.id);
            }
        }
        GameDataManager.instance.SaveToJson();
        
    }

    private void UpdateSubject(string inputFieldNameText, string inputFieldOrderText)
    {
        selectSubjectButton.UpdateData(inputFieldNameText, inputFieldOrderText);
    }

    private void UpdateChapter(string inputFieldNameText, string inputFieldOrderText)
    {
        _selectChaptersButton.UpdateName(inputFieldNameText);
        _selectChaptersButton.UpdateOrder(inputFieldOrderText);
        UpdateChapterRoot();
        GameDataManager.instance.SaveToJson();
    }

    private void UpdateChapterRoot()
    {
        if (_selectChaptersButton.Data.levelId == _selectedLevelId &&
            _selectChaptersButton.Data.subjectId == _selectedSubjectId) return;

        _selectChaptersButton.Data.levelId = _selectedLevelId;
        _selectChaptersButton.Data.subjectId = _selectedSubjectId;

        _selectChaptersButton.GetSelectedSubjectData().chapters.Remove(_selectChaptersButton.Data);
        var levelToUpdate = GameDataManager.instance.Data.levels.FirstOrDefault(lvl => lvl.id == _selectedLevelId);
        if (levelToUpdate == null)
        {
            Debug.Log("//. Level Is Null");
            return;
        }

        if (!levelToUpdate.Subjects.ContainsKey(_selectedSubjectId))
        {
            Debug.Log("//. No Subject found");
            return;
        }

        var subjectToUpdate = levelToUpdate.Subjects[_selectedSubjectId];
        subjectToUpdate.chapters.Add(_selectChaptersButton.Data);
        PoolSystem.instance.DeSpawn(_selectChaptersButton.Transform);
        _selectChaptersButton = null;
    }

    private void ResetPopup()
    {
        inputFiledName.text = "";
        inputFieldOrder.text = "";
    }
}