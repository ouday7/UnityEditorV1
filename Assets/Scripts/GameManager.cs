using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private TextAsset textJson;
    [SerializeField] private GameObject levelsPrefab;
    [SerializeField] private GameObject subjectPrefab;
    [SerializeField] private GameObject chaptersPrefab;
    [SerializeField] private RectTransform levelsHolder;
    [SerializeField] private RectTransform subjectsHolder;
    [SerializeField] private RectTransform chaptersHolder;

    private JsonData _jsonData;

    public JsonData Data => _jsonData;

    public void Initialize()
    {
        if (Instance != null) return;
        Instance = this;
    }

    public void StartGame()
    {
        _jsonData = JsonUtility.FromJson<JsonData>(textJson.text);
        _jsonData.subjects = _jsonData.subjects.OrderBy(subject => subject.order).ToList();
        SpawnLevels();
    }

    private void SpawnLevels()
    {
        var levels = _jsonData.levels.OrderBy(tab => tab.order).ToList();

        foreach (var level in levels)
        {
            // Debug.Log($"Level Order = {level.order}");
            var newLevelBtn = PoolSystem.Instance.Spawn<LevelBtn>(PoolType.Level);
            newLevelBtn.Initialize();
            newLevelBtn.BindData(level);
            newLevelBtn.transform.SetParent(levelsHolder);
        }

        ShowSubjects(levels[0].subjectsId, levels[0].id);
    }

    public void ShowSubjects(List<int> levelSubjects,int lvlId)
    {
        foreach (Transform child in subjectsHolder.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var subject in _jsonData.subjects)
        {
            if (!levelSubjects.Contains(subject.id)) continue;
            var subjectBtn = PoolSystem.Instance.Spawn<SubjectsBtn>(PoolType.Subject);
            subjectBtn.Initialize();
            subjectBtn.BindData(subject, lvlId);
            subjectBtn.transform.SetParent(subjectsHolder);
        }
    }

    public void ShowChapter(int subjectId,int lvlId)
    {
        foreach (Transform child in chaptersHolder.transform)
        {
            PoolSystem.Instance.DeSpawn(child.GetComponent<ChaptersBtn>());
        }
        
        foreach (var chapter in _jsonData.chapters)
        {
            if (chapter.subjectId != subjectId || chapter.levelId!=lvlId )continue;

            var chapterBtn = PoolSystem.Instance.Spawn<ChaptersBtn>(PoolType.Chapter);
            chapterBtn.Initialize();
            chapterBtn.BindData(chapter);
            chapterBtn.Transform.SetParent(chaptersHolder);
        }
    }

    public void SaveToJson()
    {
        var jsonString = JsonUtility.ToJson(_jsonData);
    }

    public void LogJson()
    {
        //see changes
    }
}
