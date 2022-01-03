using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct TemplateCatalog
{
    public TemplatesNames type;
    public GameObject prefab;
}

public class TemplatesHandler : MonoBehaviour
{
    public static TemplatesHandler Instance;
    public List<TemplateDataInformation> templatesData;
    public List<TemplateCategory> categoriesData;
    public List<TemplateCatalog> templatesCatalog;
    

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}

 
