using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private enum EditedData
    {
        None,
        Level,
        Subject,
        Chapter
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

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
        closeBtn.onClick.AddListener(ClosePanel);
        popUpPanel.gameObject.SetActive(false);
        submitBTn.onClick.AddListener(Submit);
    }
    private void ClosePanel()
    {
        popUpPanel.gameObject.SetActive(false);
        Reset();
    }

    public void LevelEdit(LevelBtn levelBtn)
    {
        foreach (Transform child in holderSubject.transform)
        {
            Destroy(child.gameObject);
        }

        _selectedLevelButton = levelBtn;
        _editing = EditedData.Level;
        popUpPanel.gameObject.SetActive(true);
        dataInput.gameObject.SetActive(true);
        levelSection.gameObject.SetActive(false);
        subjectSection.gameObject.SetActive(true);
       inputFiledName.placeholder.GetComponent<RtlText>().text = levelBtn.Data.name;
       
        foreach (var subject in GameManager.Instance.Data.subjects)
        {
            Button sub;
            {
                sub = Instantiate(Prefab, holderSubject);
                if (levelBtn.Data.subjectsId.Contains(subject.id))
                {
                    sub.GetComponent<Image>().color = Color.green;
                }
                else
                {
                    sub.GetComponent<Image>().color = Color.red;
                }

                sub.GetComponentInChildren<Text>().text = subject.name;
                var currentSubid = subject.id;
                //sub.onClick.AddListener(() => SubjectsManipulaiton(currentSubid, levelBtn, sub));
            }
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
        inputFiledName.placeholder.GetComponent<RtlText>().text = subjectsBtn.Data.name;
    }

    public void ChapterEdit(ChaptersBtn chapter)
    {
        foreach (Transform child in holderLevel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in InChaptersparent.transform)
        {
            Destroy(child.gameObject);
        }

        _selectChaptersBtnBtnButton = chapter;
        _editing = EditedData.Chapter;
        popUpPanel.gameObject.SetActive(true);
        dataInput.gameObject.SetActive(true);
        levelSection.gameObject.SetActive(true);
        subjectSection.gameObject.SetActive(true);

        inputFiledName.placeholder.GetComponent<RtlText>().text = chapter.Data.name;

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
        foreach (Transform child in holderLevel.transform)
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
            var subject = Instantiate(Prefab, holderLevel);
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
                NewUpdateLevel(inputFiledName.text,inputFilOrder.text);
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
    private void Reset()
    {
        inputFiledName.text = "";
        inputFilOrder.text = "";
    }
}