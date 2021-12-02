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

    [SerializeField] private GameObject popUpPanel;
    [SerializeField] private InputField inputFiledName;
    [SerializeField] private InputField inputFilOrder;
    [SerializeField] private GameObject dataInput;
    [SerializeField] private GameObject levelSection;
    [SerializeField] private GameObject subjectSection;


    [SerializeField] private Button closeBtn;
    [SerializeField] private Button submitBTn;


    [SerializeField] private Button Prefab;
    [SerializeField] private Transform holderSubject;
    [SerializeField] private Transform InChaptersparent;
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

    public void Initialize()
    {
        if (Instance != null) return;
        Instance = this;
        closeBtn.onClick.AddListener(ClosePanel);
        popUpPanel.gameObject.SetActive(false);
        submitBTn.onClick.AddListener(Submit);
        namePlaceHolder = inputFiledName.placeholder.GetComponent<RtlText>();
    }
    private void ClosePanel()
    {
        popUpPanel.gameObject.SetActive(false);
        ResetPopup();
    }

    public void LevelEdit(LevelBtn levelBtn)
    {
        Debug.Log("test");
        _selectedLevelButton = levelBtn;
        _editing = EditedData.Level;
        popUpPanel.gameObject.SetActive(true);
        dataInput.gameObject.SetActive(true);
        levelSection.gameObject.SetActive(false);
        subjectSection.gameObject.SetActive(true);
        namePlaceHolder.text = levelBtn.Data.name;
        selectedSubjects = _selectedLevelButton.Data.subjectsId;

        foreach (Transform child in holderSubject)
        {
            ObjectPooler.Instance.DeSpawn(child.transform);
        }
        foreach (var subject in GameManager.Instance.Data.subjects)
        {
            var toggleButton = ObjectPooler.Instance.Spawn<ToggleButton>(ObjectToPoolType.Toggle);
            toggleButton.Transform.SetParent(holderSubject.transform);
            toggleButton.Initialize();
            toggleButton.BindData(subject.name, subject.id);
            toggleButton.isToggle = true;
            toggleButton.OnSelectToggleButton += OnSelectSubjectToggleButton;
            toggleButton.MarkSelected(_selectedLevelButton.Data.subjectsId);
        }
        
    }

    private ToggleButton _selectedLevelButtonX;
    
    private void OnSelectSubjectToggleButton(ToggleButton inNewSelectedSubjectButton)
    {
        if (selectedSubjects.Contains(inNewSelectedSubjectButton.Id))
        {
            selectedSubjects.Remove(inNewSelectedSubjectButton.Id);
            inNewSelectedSubjectButton.Unselect();
        }
        else
        {
            selectedSubjects.Add(inNewSelectedSubjectButton.Id);
            inNewSelectedSubjectButton.Select();
        }
    }
    
    public void SubjectEdit(SubjectsBtn subjectsBtn)
    {
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
        foreach (Transform child in InChaptersparent.transform)
        {
            ObjectPooler.Instance.DeSpawn(child);
        }

        foreach (Transform child in holderSubject)
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

        foreach (var level in GameManager.Instance.Data.levels)
        {
            Button sub;
            sub = Instantiate(Prefab, InChaptersparent);
            if (chapter.Data.levelId == level.id)
            {
                sub.GetComponent<Image>().color = Color.green;
                currentButton = sub;
                sub.interactable = false;
                foreach (var subj in GameManager.Instance.Data.subjects)
                {
                    if (!level.subjectsId.Contains(subj.id)) continue;
                    var subject = Instantiate(Prefab, holderLevel);
                    subject.GetComponentInChildren<Text>().text = subj.name;
                    subject.GetComponent<Image>().color = Color.green;
                    subject.onClick.AddListener(() =>
                    {
                        subject.GetComponent<Image>().color = Color.red;
                        chapter.Data.subjectId = subj.id;
                        chapter.Data.levelId = level.id;
                    });
                }
            }
            else
            {
                sub.GetComponent<Image>().color = Color.red;
            }

            sub.GetComponentInChildren<Text>().text = level.name;
            sub.onClick.AddListener(() =>
            {
                ChangeChaptertoLevel(level.id, chapter, sub, currentButton, level);
                currentButton = sub;
            });
        }
    }

    private void ChangeChaptertoLevel(int levelid, ChaptersBtn chapter, Button sub, Button currentButton, Level level)
    {
        foreach (Transform child in holderSubject.transform)
        {
            Destroy(child.gameObject);
        }

        sub.GetComponent<Image>().color = Color.green;
        sub.interactable = false;
        currentButton.GetComponent<Image>().color = Color.red;
        currentButton.interactable = true;

        foreach (var subj in GameManager.Instance.Data.subjects)
        {
            if (!level.subjectsId.Contains(subj.id)) continue;
            var subject = Instantiate(Prefab, holderSubject);
            subject.GetComponentInChildren<Text>().text = subj.name;
            subject.GetComponent<Image>().color = Color.green;
            subject.onClick.AddListener(() =>
            {
                subject.GetComponent<Image>().color = Color.red;
                chapter.Data.subjectId = subj.id;
                chapter.Data.levelId = levelid;
            });
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
                Debug.Log("Editing a Button");
                NewUpdateLevel(inputFiledName.text, inputFilOrder.text);

                break;
            case EditedData.Subject:
                NewUpdateSubject(inputFiledName.text, inputFilOrder.text);
                break;
            case EditedData.Chapter:
                NewUpdateChapter(inputFiledName.text, inputFilOrder.text);
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
        inputFilOrder.text = "";
    }
}