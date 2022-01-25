using System;
using System.Collections.Generic;
using ChapterPanel;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace EditorMenu
{
    public class EditorButtonsManager : MonoBehaviour
    {
        public static EditorButtonsManager instance;
        [SerializeField] private CustomGridLayout levelsHolder;
        [SerializeField] private CustomGridLayout subjectsHolder;
        [SerializeField] private CustomGridLayout chaptersHolder;
        [SerializeField] private Ease ease;
        private List<Transform> _subjectsList;
        private List<Transform> _chapList;
        [NonSerialized] public LevelButton _selectedLevel;
        [NonSerialized] public SubjectButton selectedSubject;
        [NonSerialized]public ChapterButton _selectedChapter;
        private RectTransform _rt;
        private Vector2 startSize;
        private static float _delay=0.38f;
        public SubjectData SelectedSubject => selectedSubject.Data;

        private void OnDestroy()
        {
            SubjectButton.OnSelectSubjectButton -= OnSelectSubjectButton;
            ChapterButton.OnSelectChaptersButton -= OnSelectChapterButton;
        }

        public void Initialize()
        {
            if (instance != null && instance != this)
                Destroy(gameObject);
            else
            {
                instance = this;
            }
            
            SubjectButton.OnSelectSubjectButton += OnSelectSubjectButton;
            ChapterButton.OnSelectChaptersButton += OnSelectChapterButton;
            ChapterButton.OnSubmitButton += OnSubmitChapter;

            _subjectsList = new List<Transform>();
            _chapList = new List<Transform>();
            startSize.y = chaptersHolder.RectTransform.sizeDelta.y;
        }

        private void OnSubmitChapter(ChapterButton inChapterButton)
        {
            GameDataManager.instance.SetSelectedChapter(inChapterButton.Data);
            SceneHandler.instance.LoadScene(SceneNames.ChapterConfig);
        }

        public void StartEditor(JsonData inData)
        {
            for (var i = 0; i < inData.levels.Count; i++)
            {
                var newLevelBtn = PoolSystem.instance.Spawn<LevelButton>(ObjectToPoolType.Level);
                if (newLevelBtn == null) Debug.Log("Level Button is Null");
                newLevelBtn.OnSelectLevelButton += OnSelectLevelButton;
                newLevelBtn.Initialize();
                newLevelBtn.BindData(inData.levels[i]);
                newLevelBtn.Unselect();
                newLevelBtn.Transform.SetParent(levelsHolder.RectTransform);
                newLevelBtn.transform.localScale=Vector3.one;
                if (i != 0) continue;
                newLevelBtn.Select();
                newLevelBtn.LevelButtonSelected();
            }
            levelsHolder.UpdateLayout();
        }

        private void OnSelectLevelButton(LevelButton inNewSelectedLevelButton)
        {
            if (_selectedLevel != null) _selectedLevel.Unselect();
            _selectedLevel = inNewSelectedLevelButton;
            
            _selectedLevel.Select();
            GameDataManager.instance.SetSelectedLevel(inNewSelectedLevelButton.Data);
            ShowSubjects(_selectedLevel.Data);
        }

        public void ShowSubjects(LevelData inLevelDataData)
        {
            ResetSubjectsHolder();
            var isFirst = true;
            foreach (var subjectPair in inLevelDataData.Subjects)
            {
                var subject = subjectPair.Value;
                var subjectBtn = PoolSystem.instance.Spawn<SubjectButton>(ObjectToPoolType.Subject);
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

        private void OnSelectSubjectButton(SubjectButton inNewSubjectButton)
        {
            if (selectedSubject != null)
            {
                selectedSubject.ResetTextColor();
                selectedSubject.Unselect();
            }
            selectedSubject = inNewSubjectButton;
            selectedSubject.Select();
            chaptersHolder.RectTransform.sizeDelta =
                new Vector2(chaptersHolder.RectTransform.sizeDelta.x,
                    startSize.y);
            GameDataManager.instance.SetSelectedSubject(inNewSubjectButton.Data);
            ShowChapter(selectedSubject.Data);
        }

        private void ShowChapter(SubjectData inSubjectData)
        {
            ResetChaptersHolder();

            foreach (var chapter in inSubjectData.chapters)
            {
                var newBtn = PoolSystem.instance.Spawn<ChapterButton>(ObjectToPoolType.Chapter);
                newBtn.Initialize();
                newBtn.BindData(chapter, this);
                newBtn.Unselect();
                newBtn.ResetTextColor();
                newBtn.configBtn.gameObject.SetActive(false);
                newBtn.Transform.SetParent(chaptersHolder.RectTransform);
                newBtn.Transform.localScale = Vector3.one;
                _chapList.Add(newBtn.Transform);
            }
            ResizeChaptersHolder();
        }

        private void ResizeChaptersHolder()
        {
            var nbChild = chaptersHolder.RectTransform.childCount;
            if (nbChild > 6)
            {
                var sizeDelta = chaptersHolder.RectTransform.sizeDelta;
                sizeDelta = new Vector2(sizeDelta.x,
                    sizeDelta.y + (nbChild - 6) * 105);
                chaptersHolder.RectTransform.sizeDelta = sizeDelta;
            }
            chaptersHolder.UpdateLayout();
        }

        private void OnSelectChapterButton(ChapterButton inNewSelectedChapterButton)
        {
            if (_selectedChapter != null)
            {
                _selectedChapter.Unselect();
                _selectedChapter.ResetTextColor();
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