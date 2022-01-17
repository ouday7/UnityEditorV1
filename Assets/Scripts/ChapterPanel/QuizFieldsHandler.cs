using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct QuizFieldMap
{
    public FieldTypes name;
    public QuizFieldBase prefab;
}

public class QuizFieldsHandler : EntryPointSystemBase
{
    public static QuizFieldsHandler Instance;
    
    [SerializeField] private List<QuizFieldMap> quizFieldsList;
    private Dictionary<FieldTypes, QuizFieldBase> _map;
    
    public override void Begin()
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
        if (!Instance._map.ContainsKey(inName))
            throw new Exception($"No QuizField with name {inName}");
        return Instantiate(Instance._map[inName]);
    }
}