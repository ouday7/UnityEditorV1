using System.Collections.Generic;
using System.IO;
using System.Linq;
using ChapterPanel;
using UnityEngine;

namespace EditorMenu
{
    public class GameDataManager : MonoBehaviour
    {
        public static GameDataManager Instance;
        public GameSettingsData gameSettingsData;

        [SerializeField] private string fileName = "JsonFile.txt";

        [SerializeField] private JsonData jsonData;
        public JsonData Data => jsonData;

        public void Initialize()
        {
            if (Instance != null) return;
            Instance = this;
            Begin();
        }

        private void Begin()
        {
            JsontoScriptableObject();
            var data = File.ReadAllText($"{Application.streamingAssetsPath}/{fileName}");
            jsonData = JsonUtility.FromJson<JsonData>(data);


            LinkData();

            Data.levels = Data.levels.OrderBy(level => level.order).ToList();
            Data.subjects = Data.subjects.OrderBy(subject => subject.order).ToList();
        }

        private void LinkData()
        {
            for (var i = 0; i < Data.levels.Count; i++)
            {
                var level = Data.levels[i];

                level.subjects = new Dictionary<int, SubjectData>();
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
                    level.subjects.Add(subjectData.id, subjectData);
                }
            }
        }

        public void SaveToJson()
        {
            var jsonString = JsonUtility.ToJson(Data, true);
            File.WriteAllText($"{Application.streamingAssetsPath}/JsonFile.txt", jsonString);
        }

        private void JsontoScriptableObject()
        {
            var x = File.ReadAllText($"{Application.streamingAssetsPath}/{fileName}");
            gameSettingsData.dataGameSettings = JsonUtility.FromJson<JsonData>(x);
        }
    }
}