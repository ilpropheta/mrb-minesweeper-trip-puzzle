using Assets.GameFramework;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Load the specified scene when the button is clicked.
/// 
/// This automatically hooks up the button onClick listener
/// </summary>
[RequireComponent(typeof(Button))]
public class OnButtonClickLoadScene : MonoBehaviour
{
    public string SceneName;

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        AudioManager.Instance.PlayFx(AudioManager.FxType.Click);
        LevelNavigator.NavigateTo(SceneName);        
    }
}
