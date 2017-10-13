using System;
using UnityEngine;
using System.Linq;
using Assets.GameFramework;
using UnityEngine.UI;

public class ControlPanelManager : MonoBehaviour
{    
    public event EventHandler OnSubmit;

    void Awake ()
	{
	    var buttons = transform.GetComponentsInChildren<Button>();
	    
	    var quitButton = buttons.Single(x => x.name.Equals("ButtonQuit", StringComparison.OrdinalIgnoreCase));
        quitButton.onClick.AddListener(OnClickButtonQuit);

        var submitButton = buttons.Single(x => x.name.Equals("ButtonSubmit", StringComparison.OrdinalIgnoreCase));
        submitButton.onClick.AddListener(OnClickSubmit);
    }

    private void OnClickSubmit()
    {
        if (OnSubmit == null)
            return;
        OnSubmit(this, null);
    }

    private void OnClickButtonQuit()
    {
        DialogManager.Instance.AskOkOrCancel(OnDialogOkClicked);
    }

    private void OnDialogOkClicked(DialogInstance dialogInstance)
    {
        LevelNavigator.NavigateTo("01_Title");
    }
}
