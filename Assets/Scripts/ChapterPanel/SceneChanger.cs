using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChapterPanel
{
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
}
