using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
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

    public LevelsData infoListLevels = new LevelsData();
    public ChaptersData infoListChapters = new ChaptersData();
    public SubjectsData infoListSubjects = new SubjectsData();

    private JsonData _data;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }
    private void Start()
    {

        infoListLevels = JsonUtility.FromJson<LevelsData>(textJson.text);
        infoListChapters = JsonUtility.FromJson<ChaptersData>(textJson.text);
        infoListSubjects = JsonUtility.FromJson<SubjectsData>(textJson.text);
        infoListSubjects.subjects = infoListSubjects.subjects.OrderBy(subject => subject.order).ToArray();
        _data = JsonUtility.FromJson<JsonData>(textJson.text);
        SpawnLevels();
    }
    public void SpawnLevels()
    {
        var levels = infoListLevels.levels.OrderBy(tab => tab.order);
        foreach (var level in levels)
        {
            var selectedLevelBtn = ObjectPooler.Instance.GetPooledObject(levelsPrefab);
            if (selectedLevelBtn == null) continue;
            var newBtn = selectedLevelBtn.GetComponent<LevelBtn>();
            newBtn.Initialize();
            newBtn.BindData(level);
            selectedLevelBtn.transform.SetParent(levelsHolder);
           // selectedLevelBtn.GetComponent<Button>().onClick.AddListener(() => ShowSubjects(level.id));
        }
        ShowSubjects(levelsHolder.GetChild(0).GetComponent<LevelBtn>().Data.id);
    }
    public void ShowSubjects(int id)
    {
        //split into 3section
        foreach (Transform child in subjectsHolder.transform) 
        {
            // Destroy(child.gameObject);
            //  subjectBtn.gameObject.SetActive(false);
            ObjectPooler.Instance.ReleasePooledObject(child.GetComponent<PoolableObject>());
        }
        
        foreach (var subject in infoListSubjects.subjects)
        {
            if (subject.levelId != id) continue;
           
            var subjectBtn = ObjectPooler.Instance.GetPooledObject(subjectPrefab);
            if (subjectBtn == null) continue;
            
            var newBtn = subjectBtn.GetComponent<SubjectsBtn>();
            newBtn.Initialize();
            newBtn.BindData(subject);
            
            subjectBtn.transform.SetParent(subjectsHolder);

            /*
            subjectBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log($"Select Subject[{subject.id}], from level[{id}]");
                ShowChapter(id);
            });
            */
        }

        var subjectToSelect = infoListSubjects.subjects.FirstOrDefault(subject => subject.levelId == id);
        if (subjectToSelect != null)
        {
            Debug.Log($"Select Subject[{subjectToSelect.id}], from level[{id}]");
            ShowChapter(subjectToSelect.id);
        }
    }

    public void ShowChapter(int id)
    {
        // GameObject chapterbtn = ObjectPooler.instance.GetPooledObject(chaptersPrefab);

        foreach (Transform child in chaptersHolder.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (var t in infoListChapters.chapters)
        {
            if (t.levelId != id) continue;
             
            var chapterBtn = Instantiate(chaptersPrefab, chaptersHolder.transform, false);

                        
            var newBtn = chapterBtn.GetComponent<ChaptersBtn>();
            newBtn.Initialize();
            newBtn.BindData(t);
            chapterBtn.GetComponent<Button>().onClick.AddListener(() => OnClickChapter(t.id));
        }
    }
    private void OnClickChapter(int chapterId)
    {
        Debug.Log($"Click Chapter: {chapterId}");
    }

  /*
    public void UpdateData(Text oldname, string newname)
    {
        foreach (var lvl in infoListLevels.levels)
        {
            if(lvl.name!=oldname.text) continue;
            lvl.name = newname;
            SaveToJson();
        }
    }
*/
  /*   public void UpdateDataSubect(Text oldname, string newname)
     {
         foreach (var lvl in infoListSubjects.subjects)
         {
             if(lvl.name!=oldname.text) continue;
             lvl.name = newname;
             SaveToJson();
         }
     }
     */
  /* 
 public void UpdateDataChapter(Text oldname, string newname)
    {
        foreach (var lvl in infoListChapters.chapters)
        {
            if(lvl.name!=oldname.text) continue;
            lvl.name = newname;
            SaveToJson();
        }
    }
  
    
   */ 
    public void SaveToJson()
    {
        var jsonString = JsonUtility.ToJson(_data);
    }
    public void LogJson()
    {
        Debug.Log(JsonUtility.ToJson(infoListLevels));
    }
    
    


}
