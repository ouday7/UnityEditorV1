using System.Collections.Generic;
using UnityEngine;

public class EditorButtonsManager : MonoBehaviour
{
    [SerializeField] private RectTransform levelsHolder;
    [SerializeField] private RectTransform subjectsHolder;
    [SerializeField] private RectTransform chaptersHolder;

    private List<Transform> subjectsList;
    private List<Transform> chapList;
    private LevelBtn _selectedLevel;
    private SubjectsBtn _selectedSubject;

    private ChaptersBtn _selectedChapter;

    private void OnDestroy()
    {
       LevelBtn.Instance.OnSelectLevelButton -= OnSelectLevelButton;
        SubjectsBtn.OnSelectSubjectButton -= OnSelectSubjectButton;
        ChaptersBtn.OnSelectChaptersButton -= OnSelectChapterButton;

    }

    private void OnSelectChapterButton(ChaptersBtn inNewSelectedChapterButton)
    {
        if (_selectedChapter != null) _selectedChapter.Unselect();
        _selectedChapter = inNewSelectedChapterButton;
        _selectedChapter.Select();
        
    }

    public void Initialize()
    {
        // LevelBtn.Instance.OnSelectLevelButton += OnSelectLevelButton;
        SubjectsBtn.OnSelectSubjectButton += OnSelectSubjectButton;
        ChaptersBtn.OnSelectChaptersButton += OnSelectChapterButton;


        subjectsList = new List<Transform>();
        chapList = new List<Transform>();
    }

    public void StartEditor(JsonData inData)
    {
        for (var i = 0; i < inData.levels.Count; i++)
        {
            var newLevelBtn = ObjectPooler.Instance.Spawn<LevelBtn>(ObjectToPoolType.Level);
            if (newLevelBtn == null) Debug.Log("Level Button is Null");
            newLevelBtn.OnSelectLevelButton += OnSelectLevelButton;
            newLevelBtn.Initialize();
            newLevelBtn.BindData(inData.levels[i]);
            newLevelBtn.Unselect();
            newLevelBtn.t.SetParent(levelsHolder);
            newLevelBtn.t.localScale = Vector3.one;
            if (i != 0) continue;
            newLevelBtn.Select();
            newLevelBtn.LevelButtonSelected();
        }
    }

    private void OnSelectLevelButton(LevelBtn inNewSelectedLevelButton)
    {
        if (_selectedLevel != null) _selectedLevel.Unselect();
        _selectedLevel = inNewSelectedLevelButton;
        _selectedLevel.Select();
        ShowSubjects(_selectedLevel.Data);
    }
    

    
    private void ShowSubjects(Level inLevelData)
    {
        ResetSubjectsHolder();
        for (var i = 0; i < inLevelData.subjects.Count; i++)
        {
            var subject = inLevelData.subjects[i];
            var subjectBtn = ObjectPooler.Instance.Spawn<SubjectsBtn>(ObjectToPoolType.Subject);
            subjectBtn.Initialize();
            subjectBtn.BindData(subject);
            subjectBtn.Unselect();
            subjectBtn.t.SetParent(subjectsHolder);
            subjectBtn.t.localScale = Vector3.one;
            subjectsList.Add(subjectBtn.t);
            if(i != 0) continue;
            subjectBtn.Select();
            subjectBtn.SubjectButtonSelected();
        }
        
    }

    private void OnSelectSubjectButton(SubjectsBtn inNewSubjectButton)
    {
        if (_selectedSubject != null) _selectedSubject.Unselect();
        _selectedSubject = inNewSubjectButton;
        _selectedSubject.Select();
        ShowChapter(_selectedSubject.Data);
    }

    private void ShowChapter(Subject inSubject)
    {
        ResetChaptersHolder();

        foreach (var chapter in inSubject.chapters)
        {
            var chapterBtn = ObjectPooler.Instance.Spawn<ChaptersBtn>(ObjectToPoolType.Chapter);
            var newBtn = chapterBtn.GetComponent<ChaptersBtn>();
            newBtn.Initialize();
            newBtn.BindData(chapter);
            newBtn.Unselect();
            newBtn.t.SetParent(chaptersHolder);
            newBtn.t.localScale = Vector3.one;
            chapList.Add(newBtn.t);
        }
    }

    private void ResetSubjectsHolder()
    {
        if (subjectsList == null || subjectsList.Count == 0) return;
        foreach (var child in subjectsList) ObjectPooler.Instance.DeSpawn(child);
        subjectsList.Clear();
    }

    private void ResetChaptersHolder()
    {
        if (chapList == null || chapList.Count == 0) return;
        foreach (var child in chapList) ObjectPooler.Instance.DeSpawn(child);
        chapList.Clear();
    }
}