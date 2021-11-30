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
    [SerializeField] private Button Prefab;
    [SerializeField] private Transform parent;
    [SerializeField] private Transform InChaptersparent;
    [SerializeField] private Transform IChaptersparent;
    [SerializeField] private GameObject subjectPanel;
    
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
        Verif();
    }
    public void LevelEdit(LevelBtn levelBtn)
    {
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
        _selectedLevelButton = levelBtn;
        _editing = EditedData.Level;
        popUpPanel.gameObject.SetActive(true);
        commonSection.gameObject.SetActive(true);
        levelSection.gameObject.SetActive(false);
        subjectSection.gameObject.SetActive(false);
        subjectPanel.gameObject.SetActive(true);
        inputfiledName.placeholder.GetComponent<RtlText>().text = levelBtn.Data.name;
        
        foreach (var subject in GameManager.Instance.infoListSubjects.subjects)
        {
            Button sub;
            {
                sub = Instantiate(Prefab, parent);
                if (levelBtn.Data.subjectsId.Contains(subject.id))
                {
                    sub.GetComponent<Image>().color = Color.green;
                }
                else
                {
                    sub.GetComponent<Image>().color = Color.red;
                }
                sub.GetComponentInChildren<Text>().text = subject.name;
                var currentSubid=subject.id;
                sub.onClick.AddListener(()=>SubjectsManipulaiton(currentSubid,levelBtn,sub));
            }
        }
    }

    private void SubjectsManipulaiton(int currentSubid,LevelBtn lvlbtn,Button sub)
    {
        if (lvlbtn.Data.subjectsId.Contains(currentSubid))
        {
            lvlbtn.Data.subjectsId.Remove(currentSubid);
            sub.GetComponent<Image>().color = Color.red;
        }
        else
        {
            lvlbtn.Data.subjectsId.Add(currentSubid); 
            sub.GetComponent<Image>().color = Color.green;
        }
    }
    public void SubjectEdit(SubjectsBtn subjectsBtn)
    {
        _selectSubjectsBtnButton = subjectsBtn;
        _editing = EditedData.Subject;
        popUpPanel.gameObject.SetActive(true);
        commonSection.gameObject.SetActive(true);
        subjectPanel.gameObject.SetActive(false);
        subjectSection.gameObject.SetActive(false);
        levelSection.gameObject.SetActive(false);
        
        inputfiledName.placeholder.GetComponent<RtlText>().text = subjectsBtn.Data.name;
    }
    public void ChapterEdit(ChaptersBtn chapter)
    {
        foreach (Transform child in IChaptersparent.transform)
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
        commonSection.gameObject.SetActive(true);
        subjectSection.gameObject.SetActive(true);
        subjectPanel.gameObject.SetActive(false);
        levelSection.gameObject.SetActive(false);
        
        inputfiledName.placeholder.GetComponent<RtlText>().text = chapter.Data.name;

        foreach (var level in GameManager.Instance.infoListLevels.levels)
        {
            Button sub;
            sub = Instantiate(Prefab, InChaptersparent);
                if (chapter.Data.levelId == level.id)
                {
                    sub.GetComponent<Image>().color = Color.green;
                    currentButton = sub;
                    sub.interactable = false;
                    foreach (var subj in GameManager.Instance.infoListSubjects.subjects)
                    {
                        if (!level.subjectsId.Contains(subj.id)) continue;
                        var subject = Instantiate(Prefab, IChaptersparent);
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
                    ChangeChaptertoLevel(level.id, chapter, sub, currentButton,level);
                    currentButton = sub;
                });
        }
    }

    private void ChangeChaptertoLevel(int levelid,ChaptersBtn chapter, Button sub,Button currentButton,Level level)
    {
        foreach (Transform child in IChaptersparent.transform)
        {
            Destroy(child.gameObject);
        }
        sub.GetComponent<Image>().color = Color.green;
        sub.interactable = false;
        currentButton.GetComponent<Image>().color = Color.red;
        currentButton.interactable = true;
        
        foreach (var subj in GameManager.Instance.infoListSubjects.subjects)
        {
            if (!level.subjectsId.Contains(subj.id)) continue;
            var subject = Instantiate(Prefab, IChaptersparent);
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
    private void Verif()
    {
        inputfiledName.text = "";
        inputfilOrder.text = "";
    }
}   
  

