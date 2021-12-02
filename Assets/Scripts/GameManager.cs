using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private GameObject levelsPrefab;
    [SerializeField] private GameObject subjectPrefab;
    [SerializeField] private GameObject chaptersPrefab;
    [SerializeField] private RectTransform levelsHolder;
    [SerializeField] private RectTransform subjectsHolder;
    [SerializeField] private RectTransform chaptersHolder;

    [SerializeField] private string fileName= "JsonFile.txt";

    
    /*
       private JsonData _jsonData;
       public JsonData Data =>_jsonData 
     */
    public JsonData Data { get; set; }

    public void Init()
    {
        if (Instance != null) return;
        Instance = this;
        
    }
    public void Begin()
    {

        
        
        
        var data= File.ReadAllText($"{Application.streamingAssetsPath}/{fileName}");
        Debug.Log("x");
       
        
        Data = JsonUtility.FromJson<JsonData>(data);
        
       
        
        
        
        // File.WriteAllText($"{Application.streamingAssetsPath}/JsonFile.txt", data);
        
    
        Data.subjects = Data.subjects.OrderBy(subject => subject.order).ToList();
        SpawnLevels();
    }
 
    private void SpawnLevels()
    {
        var levels = Data.levels.OrderBy(tab => tab.order).ToList();

        foreach (var level in levels)
        {
            var newLevelBtn = ObjectPooler.Instance.Spawn<LevelBtn>(ObjectToPoolType.Level);
            if(newLevelBtn==null) Debug.Log("null new btn");
            var newBtn = newLevelBtn.GetComponent<LevelBtn>();
            newBtn.Initialize();
            newBtn.BindData(level);
            newBtn.transform.SetParent(levelsHolder);
        }
        ShowSubjects(levels[0].subjectsId, levels[0].id);
        ShowChapter(Data.subjects[0].id,levels[0].id);
    }
    public void ShowSubjects(List<int> id,int lvlid)
    {
        foreach (PoolableObject child in subjectsHolder.transform)
        {
            Debug.Log("enter to despawn");
            ObjectPooler.Instance.DeSpawn(child.GetComponent<PoolableObject>());
        }
        foreach (var subject in Data.subjects)
        {
            if (!id.Contains((subject.id))) continue;
           
            var subjectBtn = ObjectPooler.Instance.Spawn<SubjectsBtn>(ObjectToPoolType.Subject);
            var newBtn = subjectBtn.GetComponent<SubjectsBtn>();
            newBtn.Initialize();
            newBtn.BindData(subject);
            newBtn.transform.SetParent(subjectsHolder);
            newBtn.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.ShowChapter(subject.id,lvlid));
        }
    }
    public void ShowChapter(int Subjectid,int lvlid)
    {
        foreach (PoolableObject child in chaptersHolder.transform) 
        {
            ObjectPooler.Instance.DeSpawn(child);
        }
        foreach (var t in Data.chapters)
        {
            if (t.subjectId != Subjectid || t.levelId!=lvlid )continue;
            
            var chapterBtn = Instantiate(chaptersPrefab, chaptersHolder.transform, false);
            var newBtn = chapterBtn.GetComponent<ChaptersBtn>();
            newBtn.Initialize();
            newBtn.BindData(t);
        }
    }
    public void SaveToJson()
    {
        var jsonString = JsonUtility.ToJson(Data);
        File.WriteAllText($"{Application.streamingAssetsPath}/JsonFile.txt", jsonString);
        

    }
}
