using UnityEngine;

/// <summary>
/// Loads the specified scene when the escape key or android back button is pressed
/// </summary>
public class OnEscapeLoadLevel : MonoBehaviour
{
    public string SceneName;

    void Update()
    {
        if (!UnityEngine.Input.GetKeyDown(KeyCode.Escape)) return;

        //GameManager.LoadSceneWithTransitions(SceneName);
        //GetComponent<FadeLevelManager>().LoadScene(SceneName);
        FindObjectOfType<FadeLevelManager>().LoadScene(SceneName);
    }
}
