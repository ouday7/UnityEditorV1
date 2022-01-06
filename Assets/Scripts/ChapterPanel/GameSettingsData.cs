using UnityEngine;

namespace ChapterPanel
{
    [CreateAssetMenu(fileName = "DataGame", menuName = "DataGameManager", order = 1)]
    public class GameSettingsData : ScriptableObject
    {
        public JsonData gameData;
        public LevelData selectedLevel;
        public SubjectData selectedSubject;
        public ChapterData selectedChapter;
        
    }
}


