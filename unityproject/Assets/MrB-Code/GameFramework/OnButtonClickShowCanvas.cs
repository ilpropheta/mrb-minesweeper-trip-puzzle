using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prime31.ZestKit;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OnButtonClickShowCanvas : MonoBehaviour
{
    public float AlphaFadeInSeconds = 0.2f;
    public float ScaleFadeInSeconds = 0.4f;
    public float ScaleSize = 1.0f;
    public CanvasGroup TargetCanvas;
    public EaseType FadeInEaseType = EaseType.BackOut;

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        TargetCanvas.gameObject.SetActive(true);

        TargetCanvas.ZKalphaTo(1, AlphaFadeInSeconds)
            .setFrom(0)
            .start();

        TargetCanvas.GetComponent<RectTransform>()
            .ZKlocalScaleTo(new Vector3(ScaleSize, ScaleSize), ScaleFadeInSeconds)
            //.setFrom(new Vector3(1, 1))
            .setEaseType(FadeInEaseType)
            .start();
    }
}
