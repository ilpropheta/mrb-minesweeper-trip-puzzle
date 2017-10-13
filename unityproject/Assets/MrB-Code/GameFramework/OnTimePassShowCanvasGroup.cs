using UnityEngine;
using System.Collections;
using Prime31.ZestKit;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class OnTimePassShowCanvasGroup : MonoBehaviour {

    public float SecondsToWait = 1;

    void Start ()
	{
        var canvasGroup = gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.ZKalphaTo(1, 0.3f)
            .setDelay(SecondsToWait)
            .start();

        gameObject.GetComponent<RectTransform>()
            .ZKlocalScaleTo(new Vector3(3,3), 1.0f)
            .setEaseType(EaseType.Punch)
            .setDelay(SecondsToWait)
            .start();

    }
}
