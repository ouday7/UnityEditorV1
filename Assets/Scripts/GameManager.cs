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

    /*
       private JsonData _jsonData;
       public JsonData Data =>_jsonData 
     */
    public JsonData Data { get; set; }
    
    private List<Transform> _subjList;
    private List<Transform> _chapList;

    public void Init()
    {
        if (Instance != null) return;
        Instance = this;
    }
    public void Begin()
    {
       _subjList=new List<Transform>();
       _chapList=new List<Transform>() ;
        const string filepath = "F:\\2\\GitProjects\\UnityEditorV1\\Assets\\JsonFile.txt";
        using (var streamReader = new StreamReader(filepath))
        {
            var json = streamReader.ReadToEnd();
            Data = JsonUtility.FromJson<JsonData>(json);
        }
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
    }
    public void ShowSubjects(List<int> id,int lvlid)
    {
        foreach (var child in _subjList)
        {
            ObjectPooler.Instance.DeSpawn(child);
        }
        foreach (var subject in Data.subjects)
        {
            if (!id.Contains((subject.id))) continue;
           
            var subjectBtn = ObjectPooler.Instance.Spawn<SubjectsBtn>(ObjectToPoolType.Subject);
            var newBtn = subjectBtn.GetComponent<SubjectsBtn>();
            newBtn.Initialize();
            newBtn.BindData(subject);
            newBtn.transform.SetParent(subjectsHolder);
            _subjList.Add(newBtn.transform);
            newBtn.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.ShowChapter(subject.id,lvlid));
        }
    }
    public void ShowChapter(int Subjectid,int lvlid)
    {
        foreach (var child in _chapList) 
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
            _chapList.Add(newBtn.transform);
        }
    }
    public void SaveToJson()
    {
        var jsonString = JsonUtility.ToJson(Data);
    }
}
