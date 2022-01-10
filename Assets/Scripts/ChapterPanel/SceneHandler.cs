using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChapterPanel
{
    public enum SceneNames
    {
        GameEditor,
        ChapterConfig
    }
    
    public class SceneHandler : MonoBehaviour
    {
        public static SceneHandler instance;

        private void Awake()
        {
            if(instance != null) return;
            instance = this;
        }

        public void LoadScene(SceneNames sceneName)
        {
            //todo: implement LoadSceneAsync
            SceneManager.LoadScene((int) sceneName);
        }
    }
}