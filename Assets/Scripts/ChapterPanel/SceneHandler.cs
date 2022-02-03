using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private Slider slider;

        private void Awake()
        {
            if(instance != null) return;
            instance = this;
        }

        public void LoadScene(SceneNames sceneName)
        {
            StartCoroutine(LoadAsync(sceneName));
        }

        private IEnumerator LoadAsync(SceneNames sceneName)
        {
            var operation=SceneManager.LoadSceneAsync((int) sceneName);
            loadingScreen.SetActive(true);
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                slider.value = progress;
                yield return null;
            }
        }
    }
}