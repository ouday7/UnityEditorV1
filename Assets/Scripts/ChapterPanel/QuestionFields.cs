using ChapterPanel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestionFields : MonoBehaviour
{
    private EditManager _editManager;
    public JsonData data;

    public void Begin()
    {
        GameDataManager.instance.SaveToJson();
       
    }
    public void SaveData()
    {
        GameDataManager.instance.SaveToJson();
    }
    
}