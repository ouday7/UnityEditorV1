using System.Collections.Generic;
using UnityEngine;

public class EditorButtonsManager : MonoBehaviour
{
    [SerializeField] private RectTransform levelsHolder;
    [SerializeField] private RectTransform subjectsHolder;
    [SerializeField] private RectTransform chaptersHolder;

    private List<Transform> _subjectsList;
    private List<Transform> _chapList;
    private LevelBtn _selectedLevel;
    private SubjectsBtn _selectedSubject;
    private ChaptersBtn _selectedChapter;
    public SubjectData SelectedSubject => _selectedSubject.Data;

    private void OnDestroy()
    {
        LevelBtn.Instance.OnSelectLevelButton -= OnSelectLevelButton;
        SubjectsBtn.OnSelectSubjectButton -= OnSelectSubjectButton;
        ChaptersBtn.OnSelectChaptersButton -= OnSelectChapterButton;
    }

    public void Initialize()
    {
        // LevelBtn.Instance.OnSelectLevelButton += OnSelectLevelButton;
        SubjectsBtn.OnSelectSubjectButton += OnSelectSubjectButton;
        ChaptersBtn.OnSelectChaptersButton += OnSelectChapterButton;

        _subjectsList = new List<Transform>();
        _chapList = new List<Transform>();
    }

    public void StartEditor(JsonData inData)
    {
        for (var i = 0; i < inData.levels.Count; i++)
        {
            var newLevelBtn = PoolSystem.instance.Spawn<LevelBtn>(ObjectToPoolType.Level);
            if (newLevelBtn == null) Debug.Log("Level Button is Null");
            newLevelBtn.OnSelectLevelButton += OnSelectLevelButton;
            newLevelBtn.Initialize();
            newLevelBtn.BindData(inData.levels[i]);
            newLevelBtn.Unselect();
            newLevelBtn.Transform.SetParent(levelsHolder);
            newLevelBtn.Transform.localScale = Vector3.one;
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

    private void ShowSubjects(LevelData inLevelDataData)
    {
        ResetSubjectsHolder();
        var isFirst = true;
        foreach (var subjectPair in inLevelDataData.subjects)
        {
            var subject = subjectPair.Value;
            var subjectBtn = PoolSystem.instance.Spawn<SubjectsBtn>(ObjectToPoolType.Subject);
            subjectBtn.Initialize();
            subjectBtn.BindData(subject);
            subjectBtn.Unselect();
            subjectBtn.Transform.SetParent(subjectsHolder);
            subjectBtn.Transform.localScale = Vector3.one;
            _subjectsList.Add(subjectBtn.Transform);
            if(!isFirst) continue;
            subjectBtn.Select();
            subjectBtn.SubjectButtonSelected();
            isFirst = false;
        }
    }

    private void OnSelectSubjectButton(SubjectsBtn inNewSubjectButton)
    {
        if (_selectedSubject != null) _selectedSubject.Unselect();
        _selectedSubject = inNewSubjectButton;
        _selectedSubject.Select();
        ShowChapter(_selectedSubject.Data);
    }

    private void ShowChapter(SubjectData inSubjectData)
    {
        ResetChaptersHolder();

        foreach (var chapter in inSubjectData.chapters)
        {
            var chapterBtn = PoolSystem.instance.Spawn<ChaptersBtn>(ObjectToPoolType.Chapter);
            var newBtn = chapterBtn.GetComponent<ChaptersBtn>();
            newBtn.Initialize();
            newBtn.BindData(chapter, this);
            newBtn.Unselect();
            newBtn.Transform.SetParent(chaptersHolder);
            newBtn.Transform.localScale = Vector3.one;
            _chapList.Add(newBtn.Transform);
        }
    }
    
    private void OnSelectChapterButton(ChaptersBtn inNewSelectedChapterButton)
    {
        if (_selectedChapter != null) _selectedChapter.Unselect();
        _selectedChapter = inNewSelectedChapterButton;
        _selectedChapter.Select();
    }

    private void ResetSubjectsHolder()
    {
        if (_subjectsList == null || _subjectsList.Count == 0) return;
        foreach (var child in _subjectsList) PoolSystem.instance.DeSpawn(child);
        _subjectsList.Clear();
    }

    private void ResetChaptersHolder()
    {
        if (_chapList == null || _chapList.Count == 0) return;
        foreach (var child in _chapList) PoolSystem.instance.DeSpawn(child);
        _chapList.Clear();
    }
}