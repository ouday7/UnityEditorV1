using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[System.Serializable]
public struct QuizFieldMap
{
    public FieldTypes name;
    public AssetReference prefab;
}

public class QuizFieldsHandler : EntryPointSystemBase
{
    public static QuizFieldsHandler Instance;

    [SerializeField] private List<QuizFieldMap> quizFieldsList;
    private Dictionary<FieldTypes, AssetReference> _map;

    public override void Begin()
    {
        if (Instance != null) return;
        Instance = this;
        _map = new Dictionary<FieldTypes, AssetReference>();
        foreach (var quizFieldMap in quizFieldsList)
        {
            if (_map.ContainsKey(quizFieldMap.name)) continue;
            _map.Add(quizFieldMap.name, quizFieldMap.prefab);
        }
    }

    public void GetQuizField(FieldTypes inName, Action<QuizFieldBase> onCompleteCallback)
    {
        if (!Instance._map.ContainsKey(inName))
            throw new Exception($"No QuizField found with name {inName}");

        Addressables.InstantiateAsync(Instance._map[inName]).Completed += (operation) =>
        {
            if (operation.Status != AsyncOperationStatus.Succeeded) Debug.Log("Fail to load assets.");
            Debug.Log("Asset Loaded Successfully");
            onCompleteCallback?.Invoke(operation.Result.GetComponent<QuizFieldBase>());
        };

    }
    
}