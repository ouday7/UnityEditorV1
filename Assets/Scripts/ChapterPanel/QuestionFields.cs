using ChapterPanel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestionFields : MonoBehaviour
{
    private EditManager _editManager;
    public JsonData data;

    public void Start()
    { ;
        //  saveBtn.onClick.AddListener(SaveData);
        GameDataManager.Instance.SaveToJson();
       
    }
   
    public void SaveData()
    {
        GameDataManager.Instance.SaveToJson();
    }
    
}