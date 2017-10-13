using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prime31.ZestKit;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OnButtonClickHideCanvas : MonoBehaviour
{
    public float AlphaFadeInSeconds = 0.3f;
    public float ScaleFadeInSeconds = 0.4f;
    public float ScaleSize = 0f;
    public CanvasGroup TargetCanvas;
    public EaseType FadeOutEaseType = EaseType.BackIn;
    public string SceneName;

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private bool _ongoingFade;

    void OnClick()
    {
        if (_ongoingFade)
            return;
        
        _ongoingFade = true;
        ActionTask.afterDelay(
            Mathf.Max(AlphaFadeInSeconds, ScaleFadeInSeconds), this, 
            task => { _ongoingFade = false; });

        TargetCanvas.GetComponent<RectTransform>()
            .ZKlocalScaleTo(new Vector3(ScaleSize, ScaleSize), ScaleFadeInSeconds)
            .setFrom(new Vector3(1, 1))
            .setEaseType(FadeOutEaseType)
            .start();
       
        TargetCanvas.ZKalphaTo(0, AlphaFadeInSeconds)
            .setFrom(1)
            .setCompletionHandler(tween =>
            {
                if (!string.IsNullOrEmpty(SceneName))
                    FindObjectOfType<FadeLevelManager>().LoadScene(SceneName);                 
            })
            .start();
    }
}
