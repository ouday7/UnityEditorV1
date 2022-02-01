using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Templates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TemplatesHandler : EntryPointSystemBase
{
    [System.Serializable]
    private struct TemplateCatalog
    {
        public TemplatesNames type;
        public AssetReference prefab;
    }

    public static TemplatesHandler Instance;

    [SerializeField] private List<TemplateData> templatesData;
    [SerializeField] private List<TemplateCategory> categoriesData;
    [SerializeField] private List<TemplateCatalog> templatesCatalog;

    private Dictionary<TemplateCategory, List<TemplateData>> _mapByCategory;
    private Dictionary<int, TemplateData> _mapById;
    private Dictionary<TemplatesNames, AssetReference> _mapByName;

    public List<TemplateCategory> Categories => categoriesData;
    public List<TemplateData> Templates => templatesData;



    public override void Begin()
    {
        if (Instance != null) return;
        Instance = this;
        _mapByCategory = new Dictionary<TemplateCategory, List<TemplateData>>();
        _mapById = new Dictionary<int, TemplateData>();
        _mapByName = new Dictionary<TemplatesNames, AssetReference>();

        foreach (var category in categoriesData)
        {
            if (_mapByCategory.ContainsKey(category)) continue;
            _mapByCategory.Add(category,
                templatesData.Where(template => template.category == category.category).ToList());
        }

        foreach (var template in templatesData)
        {
            if (_mapById.ContainsKey(template.id)) continue;
            _mapById.Add(template.id, template);
        }

        foreach (var catalog in templatesCatalog)
        {
            if (_mapByName.ContainsKey(catalog.type)) continue;
            _mapByName.Add(catalog.type, catalog.prefab);
        }

   
    }

    public void GetTemplateById(int Id, Action<TemplateBase> OnComplete)
    {
        if (!Instance._mapById.ContainsKey(Id)) throw Exception($"No Template With id == {Id}");
        var name = Instance._mapById[Id].templateName;
        if (!Instance._mapByName.ContainsKey(name)) throw Exception($"No Template With Name == {name}");
        // return Instantiate(Instance._mapByName[name]);
        Addressables.InstantiateAsync(Instance._mapByName[name]).Completed += (operation) =>
        {
            OnComplete?.Invoke(operation.Result.GetComponent<TemplateBase>());
        };
    }
    
    
    public static TemplateData GetTemplateDataById(int Id)
    {
        if(!Instance._mapById.ContainsKey(Id)) 
            throw Exception($"No TemplateData With Id == {Id}");
        return Instance._mapById[Id];
    }

    public static Exception Exception(string message) => new Exception(message);


    //just for test 
    //
    // #region Test Addressable
    //
    // [System.Serializable]
    // public struct TempCat
    // {
    //     public string name;
    //     public AssetReference AssetReference;
    // }
    //
    // public  Dictionary<TemplateCategory, List<TempCat>> _mapByTypeCategory;
    // [SerializeField] private List<TempCat> tempCats;
    //
    //
    // public AssetReference objectToLoad;
    // public AssetReference accessoryObjectToLoad;
    // private GameObject _instantiatedObject;
    // private GameObject _instantiatedAccessoryObject;
    //
    //
    // [Button]
    // public void ObjectLoadDone(AsyncOperationHandle<GameObject> obj)
    // {
    //     if (obj.Status == AsyncOperationStatus.Succeeded)
    //     {
    //         GameObject loadedObject = obj.Result;
    //         Debug.Log("Successfully loaded object.");
    //         _instantiatedObject = Instantiate(loadedObject);
    //         Debug.Log("Successfully instantiated object.");
    //         if (accessoryObjectToLoad != null)
    //         {
    //             accessoryObjectToLoad.InstantiateAsync(_instantiatedObject.transform).Completed += op =>
    //             {
    //                 if (op.Status == AsyncOperationStatus.Succeeded)
    //                 {
    //                     _instantiatedAccessoryObject = op.Result;
    //                     Debug.Log("Successfully loaded and instantiated accessory object.");
    //                 }
    //             };
    //         }
    //     }
    //     else if (obj.Status == AsyncOperationStatus.Failed)
    //     {
    //         Debug.Log("Operation Failed !: ");
    //     }
    //     else
    //     {
    //         Debug.Log("Operation None !: ");
    //     }
    // }
    // public void ObjectLoad(AsyncOperationHandle<GameObject> obj)
    // {
    //     
    // }
    //    #endregion
    }