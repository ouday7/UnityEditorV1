using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChapterPanel;
using EditorMenu;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;
    
    [SerializeField] private string fileName = "JsonFile.txt";
    
    [SerializeField] private GameSettingsData data;
    public JsonData Data => data.gameData;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (instance != null) return;
        instance = this;
    }
    
    public void GenerateData()
    {
        var dataString = File.ReadAllText($"{Application.streamingAssetsPath}/{fileName}");
        data.gameData = JsonUtility.FromJson<JsonData>(dataString);

        LinkData();

        Data.levels = Data.levels.OrderBy(level => level.order).ToList();
        Data.subjects = Data.subjects.OrderBy(subject => subject.order).ToList();
    }

    private void LinkData()
    {
        for (var i = 0; i < Data.levels.Count; i++)
        {
            var level = Data.levels[i];

            level.Subjects = new Dictionary<int, SubjectData>();
            for (var j = 0; j < Data.subjects.Count; j++)
            {
                var subject = Data.subjects[j];
                if (!level.subjectsId.Contains(subject.id)) continue;
                var subjectData = new SubjectData
                {
                    id = subject.id,
                    chapters = Data.chapters
                        .Where(chapter => chapter.levelId == level.id && subject.id == chapter.subjectId)
                        .ToList(),
                    name = subject.name,
                    order = subject.order
                };
                level.Subjects.Add(subjectData.id, subjectData);
            }
        }
    }

    public void SaveToJson()
    {
        var jsonString = JsonUtility.ToJson(Data, true);
        File.WriteAllText($"{Application.streamingAssetsPath}/JsonFile.txt", jsonString);
    }

    public void SetSelectedChapter(ChapterData inChapterData) => data.selectedChapter = inChapterData;
    public void SetSelectedLevel(LevelData inLevelData) => data.selectedLevel = inLevelData;
    public void SetSelectedSubject(SubjectData inSubjectData) => data.selectedSubject = inSubjectData;
    public ChapterData GetSelectedChapter() => data.selectedChapter;
    public LevelData GetSelectedLevel() => data.selectedLevel;
    
    public SubjectData GetSelectedSubject() => data.selectedSubject;
}