using UnityEditor.SceneManagement;
using UnityEngine;  
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private Button configBtn;
    [SerializeField] private string newScene;
    
    private void Start()
    {
        configBtn.onClick.AddListener(OpenConfigScene);
    }

    private void OpenConfigScene()
    {
        var y = SceneManager. GetActiveScene(). buildIndex;
        SceneManager.UnloadSceneAsync(y);
        
        SceneManager.LoadScene(newScene);
    }
    
}
