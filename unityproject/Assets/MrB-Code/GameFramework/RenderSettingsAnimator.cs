using UnityEngine;
using System.Collections;

public class RenderSettingsAnimator : MonoBehaviour
{
    public bool EnableAnimation = false;
    public Color AmbientLight = new Color(0.21f, 0.23f, 0.26f); //363942FF - 54  58 66

    void Update()
    {
        if (EnableAnimation)
            RenderSettings.ambientLight = AmbientLight;
    }   
}
