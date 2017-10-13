using UnityEngine;
using System.Collections;

public class OnTimePassLoadLevel : MonoBehaviour
{
    public string SceneName;
    public int SecondsToWait=1;

    // Use this for initialization
    void Start ()
	{
        StartCoroutine(WaitForSeconds());
    }

    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(SecondsToWait);
        FindObjectOfType<FadeLevelManager>().LoadScene(SceneName);
    }    
}
