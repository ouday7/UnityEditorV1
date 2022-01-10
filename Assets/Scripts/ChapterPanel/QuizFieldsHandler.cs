using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct QuizFieldMap
{
    public FieldTypes name;
    public QuizFieldBase prefab;
}

public class QuizFieldsHandler : MonoBehaviour
{
    public static QuizFieldsHandler Instance;
    [SerializeField] private List<QuizFieldMap> quizFieldsList;
    private Dictionary<FieldTypes, QuizFieldBase> _map;
    
    public void Begin()
    {
        if(Instance != null) return;
        Instance = this;
        _map = new Dictionary<FieldTypes, QuizFieldBase>();
        foreach (var quizFieldMap in quizFieldsList)
        {
            if(_map.ContainsKey(quizFieldMap.name)) continue;
            _map.Add(quizFieldMap.name, quizFieldMap.prefab);
        }
    }
    public static QuizFieldBase GetQuizField(FieldTypes inName)
    {
        if (!Instance._map.ContainsKey(inName)) return null;
        return Instantiate(Instance._map[inName]);
    }
}