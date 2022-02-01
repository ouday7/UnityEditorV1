using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public AssetReference objectToLoad;
    public AssetReference accessoryObjectToLoad;
    public Canvas canv;
    private GameObject instantiatedObject;
    private GameObject instantiatedAccessoryObject;
    
    private void Awake()
    {
        Addressables.LoadAssetAsync <GameObject>(objectToLoad).Completed += ObjectLoadDone;
    }

    private void ObjectLoadDone(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        var loadedObject = obj.Result;
        Debug.Log("Successfully loaded object.");
        instantiatedObject = Instantiate(loadedObject,canv.transform);
        Debug.Log("Successfully instantiated object.");
        if (accessoryObjectToLoad != null)
        {
            accessoryObjectToLoad.InstantiateAsync(instantiatedObject.transform).Completed += op =>
            {
                if (op.Status != AsyncOperationStatus.Succeeded) return;
                
                instantiatedAccessoryObject = op.Result;
                Debug.Log("Successfully loaded and instantiated accessory object.");
            };
        }
    }
}
