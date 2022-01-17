using System;
using System.Collections.Generic;
using System.Linq;
using Templates;
using UnityEngine;

public class TemplatesHandler : EntryPointSystemBase
{
    [System.Serializable]
    private struct TemplateCatalog
    {
        public TemplatesNames type;
        public TemplateBase prefab;
    }
    
    public static TemplatesHandler Instance;
    
    [SerializeField] private List<TemplateData> templatesData;
    [SerializeField] private List<TemplateCategory> categoriesData;
    [SerializeField] private List<TemplateCatalog> templatesCatalog;


    private Dictionary<TemplateCategory, List<TemplateData>> _mapByCategory;
    private Dictionary<int, TemplateData> _mapById;
    private Dictionary<TemplatesNames, TemplateBase> _mapByName;
    
    public List<TemplateCategory> Categories => categoriesData;
    public List<TemplateData> Templates => templatesData;
    
    public override void Begin()
    {
        if (Instance != null) return;
        Instance = this;
        _mapByCategory = new Dictionary<TemplateCategory, List<TemplateData>>();
        _mapById = new Dictionary<int, TemplateData>();
        _mapByName = new Dictionary<TemplatesNames,TemplateBase>();

        foreach (var category in categoriesData)
        {
            if (_mapByCategory.ContainsKey(category)) continue; 
            _mapByCategory.Add(category, templatesData.Where(template => template.category == category.category).ToList());
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

    public static TemplateBase GetTemplateById(int templateId)
    {
        if (!Instance._mapById.ContainsKey(templateId)) throw Exception($"No Template With id == {templateId}");
        var name = Instance._mapById[templateId].templateName;
        if(!Instance._mapByName.ContainsKey(name)) 
            throw Exception($"No Template With Name == {name}");
        return Instantiate(Instance._mapByName[name]);
    }

    public static TemplateData GetTemplateDataById(int Id)
    {
        if(!Instance._mapById.ContainsKey(Id)) 
            throw Exception($"No TemplateData With Id == {Id}");
        return Instance._mapById[Id];
    }

    public static Exception Exception(string message) => new Exception(message);
}

 
