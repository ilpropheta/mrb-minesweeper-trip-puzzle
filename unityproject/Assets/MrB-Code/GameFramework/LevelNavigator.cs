using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.GameFramework
{
    public static class LevelNavigator
    {
        public static object Args;
        private static string _lastScene;

        public static void NavigateTo(string sceneName, object args = null)
        {
            Args = args;
            _lastScene = SceneManager.GetActiveScene().name;
            var fadeManager = Object.FindObjectOfType<FadeLevelManager>();
            if (fadeManager != null)
            {
                Object.FindObjectOfType<FadeLevelManager>().LoadScene(sceneName);
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }

        public static void GoBack()
        {
            SceneManager.LoadScene(_lastScene);
            _lastScene = SceneManager.GetActiveScene().name;
        }


    }
}
