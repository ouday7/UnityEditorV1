﻿using System;
using System.Collections.Generic;
using ChapterPanel;
using Envast.Components.GridLayout;
using UnityEngine;

namespace EditorMenu
{
    public class EditorButtonsManager : MonoBehaviour
    {
        public static EditorButtonsManager instance;
        [SerializeField] private CustomGridLayout levelsHolder;
        [SerializeField] private CustomGridLayout subjectsHolder;
        [SerializeField] private CustomGridLayout chaptersHolder;
    
        private List<Transform> _subjectsList;
        private List<Transform> _chapList;
        [NonSerialized] public LevelBtn _selectedLevel;
        [NonSerialized] public SubjectsBtn _selectedSubject;
        [NonSerialized]public ChaptersBtn _selectedChapter;
        private RectTransform _rt;
        private Vector2 startSize;
        public SubjectData SelectedSubject => _selectedSubject.Data;

        private void OnDestroy()
        {
            SubjectsBtn.OnSelectSubjectButton -= OnSelectSubjectButton;
            ChaptersBtn.OnSelectChaptersButton -= OnSelectChapterButton;
        }

        public void Initialize()
        {
            if (instance != null && instance != this)
                Destroy(gameObject);
            else
            {
                instance = this;
            }
            SubjectsBtn.OnSelectSubjectButton += OnSelectSubjectButton;
            ChaptersBtn.OnSelectChaptersButton += OnSelectChapterButton;
            ChaptersBtn.OnSubmitButton += OnSubmitChapter;

            _subjectsList = new List<Transform>();
            _chapList = new List<Transform>();
            startSize.y = chaptersHolder.RectTransform.sizeDelta.y;
        }

        private void OnSubmitChapter(ChaptersBtn inChapterButton)
        {
            GameDataManager.instance.SetSelectedChapter(inChapterButton.Data);
            SceneHandler.instance.LoadScene(SceneNames.ChapterConfig);
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
                newLevelBtn.Transform.SetParent(levelsHolder.RectTransform);
                newLevelBtn.Transform.localScale = Vector3.one;
                if (i != 0) continue;
                newLevelBtn.Select();
                newLevelBtn.LevelButtonSelected();
            }
            levelsHolder.UpdateLayout();
        }

        private void OnSelectLevelButton(LevelBtn inNewSelectedLevelButton)
        {
            if (_selectedLevel != null) _selectedLevel.Unselect();
            _selectedLevel = inNewSelectedLevelButton;
            _selectedLevel.Select();
            ShowSubjects(_selectedLevel.Data);
            GameDataManager.instance.SetSelectedLevel(inNewSelectedLevelButton.Data);
        }

        public void ShowSubjects(LevelData inLevelDataData)
        {
            ResetSubjectsHolder();
            var isFirst = true;
            foreach (var subjectPair in inLevelDataData.Subjects)
            {
                var subject = subjectPair.Value;
                var subjectBtn = PoolSystem.instance.Spawn<SubjectsBtn>(ObjectToPoolType.Subject);
                subjectBtn.Initialize();
                subjectBtn.BindData(subject);
                subjectBtn.Unselect();
                subjectBtn.Transform.SetParent(subjectsHolder.RectTransform);
                subjectBtn.Transform.localScale = Vector3.one;
                _subjectsList.Add(subjectBtn.Transform);
                if(!isFirst) continue;
                subjectBtn.Select();
                subjectBtn.SubjectButtonSelected();
                isFirst = false;
            }
            subjectsHolder.UpdateLayout();
        }

        private void OnSelectSubjectButton(SubjectsBtn inNewSubjectButton)
        {
            if (_selectedSubject != null) _selectedSubject.Unselect();
            _selectedSubject = inNewSubjectButton;
            _selectedSubject.Select();
            
            chaptersHolder.RectTransform.sizeDelta =
                new Vector2(chaptersHolder.RectTransform.sizeDelta.x,
                    startSize.y);
            GameDataManager.instance.SetSelectedSubject(inNewSubjectButton.Data);
            ShowChapter(_selectedSubject.Data);
        }

        private void ShowChapter(SubjectData inSubjectData)
        {
            ResetChaptersHolder();

            foreach (var chapter in inSubjectData.chapters)
            {
                var newBtn = PoolSystem.instance.Spawn<ChaptersBtn>(ObjectToPoolType.Chapter);
                newBtn.Initialize();
                newBtn.BindData(chapter, this);
                newBtn.Unselect();
                newBtn.configBtn.gameObject.SetActive(false);
                newBtn.Transform.SetParent(chaptersHolder.RectTransform);
                newBtn.Transform.localScale = Vector3.one;
                _chapList.Add(newBtn.Transform);
            }
            var nbChild = chaptersHolder.RectTransform.childCount;
            if (nbChild> 6)
            {
                chaptersHolder.RectTransform.sizeDelta = new Vector2(chaptersHolder.RectTransform.sizeDelta.x,
                    chaptersHolder.RectTransform.sizeDelta.y +((nbChild-6)* 105));
            }
            chaptersHolder.UpdateLayout();
        }
    
        private void OnSelectChapterButton(ChaptersBtn inNewSelectedChapterButton)
        {
            if (_selectedChapter != null)
            {
                _selectedChapter.Unselect();
                _selectedChapter.configBtn.gameObject.SetActive(false);
            }
            _selectedChapter = inNewSelectedChapterButton;
            _selectedChapter.Select();
            _selectedChapter.configBtn.gameObject.SetActive(true);
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
        private RectTransform RectTransform
        {
            get
            {
                if (_rt == null) _rt = GetComponent<RectTransform>();
                return _rt;
            }
        }
    }
}