using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UPersian.Components;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public Button butt;
    [SerializeField] private TextAsset textJson;
    
    [SerializeField] private GameObject levelsPrefab;
    [SerializeField] private GameObject subjectPrefab;
    [SerializeField] private GameObject chaptersPrefab;
    
    [SerializeField] private RectTransform levelsHolder;
    [SerializeField] private RectTransform subjectsHolder;
    [SerializeField] private RectTransform chaptersHolder;
    
    public GameObject selectedLevelBtn;
    [SerializeField] private ChaptersBtn selectedChaptersBtn;
    [SerializeField] private SubjectsBtn selectedSubjectsBtn;

    public LevelsData infoListLevels = new LevelsData();
    public ChaptersData infoListChapters = new ChaptersData();
    public SubjectsData infoListSubjects = new SubjectsData();

    private JsonData _data;//added

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
        _data = JsonUtility.FromJson<JsonData>(textJson.text);//added
        SpawnLevels();
    }

    public void SpawnLevels()
    {
        var levels = infoListLevels.levels.OrderBy(tab => tab.order);
        foreach (var level in levels)
        {
            selectedLevelBtn = ObjectPooler.Instance.GetPooledObject(levelsPrefab);

            if (selectedLevelBtn == null) continue;
            var newBtn = selectedLevelBtn.GetComponent<LevelBtn>();
            newBtn.Initialize();
            newBtn.BindData(level);
            selectedLevelBtn.transform.SetParent(levelsHolder);
            selectedLevelBtn.SetActive(true);//initialize
            selectedLevelBtn.name = level.name;//initialize
            selectedLevelBtn.GetComponentInChildren<RtlText>().text = level.name;//initialize
            selectedLevelBtn.GetComponent<Button>().onClick.AddListener(() => ShowSubjects(level.id));//initialize
        }
        ShowSubjects(levelsHolder.GetChild(0).GetComponent<LevelBtn>().Data.id);
    }

    private void ShowSubjects(int id)
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
            subjectBtn.transform.SetParent(subjectsHolder);
            subjectBtn.SetActive(true);
            subjectBtn.GetComponentInChildren<RtlText>().text = subject.name;
            subjectBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log($"Select Subject[{subject.id}], from level[{id}]");
                ShowChapter(id);
            });
        }

        var subjectToSelect = infoListSubjects.subjects.FirstOrDefault(subject => subject.levelId == id);
        if (subjectToSelect != null)
        {
            Debug.Log($"Select Subject[{subjectToSelect.id}], from level[{id}]");
            ShowChapter(subjectToSelect.id);
        }
    }
    private void ShowChapter(int id)
    {
       // GameObject chapterbtn = ObjectPooler.instance.GetPooledObject(chaptersPrefab);

        foreach (Transform child in chaptersHolder.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        
        for (var i = 0; i < infoListChapters.chapters.Length; i++)
        {
            if (infoListChapters.chapters[i].levelId != id) continue;
             
            var chapterBtn = Instantiate(chaptersPrefab, chaptersHolder.transform, false);

            //chapterbtn.transform.SetParent(chaptersHolder);
            chapterBtn.GetComponentInChildren<RtlText>().text = infoListChapters.chapters[i].name;
            chapterBtn.GetComponent<Button>().onClick.AddListener(() => OnClickChapter(infoListChapters.chapters[i].id));
        }
    }

    private void OnClickChapter(int chapterId)
    {
        Debug.Log($"Click Chapter: {chapterId}");
    }

    public void UpdateData(Text oldname, string newname)
    {
        foreach (var lvl in infoListLevels.levels)
        {
            if(lvl.name!=oldname.text) continue;
            lvl.name = newname;
            SaveToJson();
        }
    }

    private void SaveToJson()
    {
        var jsonString = JsonUtility.ToJson(_data);
    }

    public void LogJson()
    {
        Debug.Log(JsonUtility.ToJson(infoListLevels));
    }
}
