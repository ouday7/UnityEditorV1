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
    
    public GameObject levelbtn;
    [SerializeField] private ChaptersBtn _chaptersBtn;
    [SerializeField] private SubjectsBtn _subjectsBtn;

    
    [System.Serializable] public class Levels
    {
        public int id;
        public string name;
        public int order;
    }
    [System.Serializable] public class Subjects
    {
        public int id;
        public string name;
        public int order;
        public int levelId;
    }
    [System.Serializable] public class Chapters
    {
        public int id;
        public string name;
        public int order;
         public int subjectId;
         public int levelId;

    }

    
    [System.Serializable] public class GetDatalevels
    {
        public Levels[] levels;
    }
    [System.Serializable] public class GetDatachapters
    {
        public Chapters[] chapters;
    }
    [System.Serializable] public class GetDatasubjects
    {
        public Subjects[] subjects;
    }
    
    
    public GetDatalevels infoListlevels = new GetDatalevels();
    public GetDatachapters infoListchapters = new GetDatachapters();
    public GetDatasubjects infoListsubjects = new GetDatasubjects();
    

    

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }
    private void Start()
    {
        infoListlevels = JsonUtility.FromJson<GetDatalevels>(textJson.text);
        infoListchapters = JsonUtility.FromJson<GetDatachapters>(textJson.text);
        infoListsubjects = JsonUtility.FromJson<GetDatasubjects>(textJson.text);
        SpawnLevels();
        
        
    }
    public void SpawnLevels()
    {
        var list = infoListlevels.levels.OrderBy(tab => tab.order);
        foreach (var tab in list)
        {
            

           levelbtn = ObjectPooler.instance.GetPooledObject(levelsPrefab);
           
            if (levelbtn != null)
            {
                
                levelbtn.transform.SetParent(levelsHolder);
                levelbtn.SetActive(true);
                levelbtn.name = tab.name;
                levelbtn.GetComponentInChildren<RtlText>().text = tab.name;
                levelbtn.GetComponent<Button>().onClick.AddListener(() => ShowSubjects(tab.id));
                
            }

        }
    }
    public void ShowSubjects(int i)
    {

        GameObject subjectBtn = ObjectPooler.instance.GetPooledObject(subjectPrefab);



        foreach (Transform child in subjectsHolder.transform) 
        {
            GameObject.Destroy(child.gameObject);
          //  subjectBtn.gameObject.SetActive(false);

        }
        var list = infoListsubjects.subjects.OrderBy(tab => tab.order);

        foreach (var t in list)
        {

            
            if (t.levelId != i) continue;
            if (subjectBtn != null)
            {
                
                subjectBtn.transform.SetParent(subjectsHolder);
                subjectBtn.SetActive(true);
                subjectBtn.GetComponentInChildren<RtlText>().text = t.name;
               subjectBtn.GetComponent<Button>().onClick.AddListener(() => ShowChaperte(i));
                
            }
            
        }
    }
    private void ShowChaperte(int i)
    {
       // GameObject chapterbtn = ObjectPooler.instance.GetPooledObject(chaptersPrefab);

        foreach (Transform child in chaptersHolder.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        for (var j = 0; j < infoListchapters.chapters.Length; j++)
        {
            if (infoListchapters.chapters[j].levelId != i) continue;
             
           var chapterbtn = Instantiate(chaptersPrefab, chaptersHolder.transform, false);

            //chapterbtn.transform.SetParent(chaptersHolder);
            chapterbtn.GetComponentInChildren<RtlText>().text = infoListchapters.chapters[j].name;
            var j1 = j;
            chapterbtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log(infoListchapters.chapters[j1].name);
            });
        }
    }

    public void Updatedata(Text oldname, string newname)
    {
        foreach (var lvl in infoListlevels.levels)
        {
            if(lvl.name!=oldname.text) continue;
            lvl.name = newname;
            SaveToJson();
        }
        
    }

    private void SaveToJson()
    {
        
        
    }
}
