using UnityEngine;
using System.Collections;
using Assets.GameFramework;
using Assets.GameFramework.Localisation;

public class OnButtonClickSetLanguage : OnButtonClick
{
    [Tooltip("The langauge that we want to set")]
    public SystemLanguage Language;

    public override void OnClick()
    {
        LocalisationManager.SetLanguage(Language);
    }

}
