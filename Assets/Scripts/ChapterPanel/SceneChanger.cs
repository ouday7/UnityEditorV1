using UnityEditor.SceneManagement;
using UnityEngine;  
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string configScene;
    [SerializeField] private string mainScene;

    public void OpenConfigScene()
    {
        SceneManager.LoadScene(configScene);
    }
    public void OpenMainScene()
    {
        SceneManager.LoadScene(mainScene);
    }
}
