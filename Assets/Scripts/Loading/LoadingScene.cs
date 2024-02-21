using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Loading
{
    public class LoadingScene : MonoBehaviour
    {
        public GameObject LoadingScreen;
        public Image LoadingBarFill;

        public void LoadScene(int sceneId)
        {
            StartCoroutine(LoadSceneAsync(sceneId));
        }
        
        private IEnumerator LoadSceneAsync(int sceneId)
        {
            var operation = SceneManager.LoadSceneAsync(sceneId);
            
            LoadingScreen.SetActive(true);
            
            while (!operation.isDone)
            {
                var progress = Mathf.Clamp01(operation.progress / 0.9f);
                LoadingBarFill.fillAmount = progress;
                yield return null;
            }
        }

    }
}
