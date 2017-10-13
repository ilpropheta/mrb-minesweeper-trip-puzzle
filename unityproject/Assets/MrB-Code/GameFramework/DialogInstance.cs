using System;
using Assets.GameFramework.GameObjects;
using Prime31.ZestKit;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameFramework
{
    public class DialogInstance
    {
        private readonly GameObject _uiGameObject;
        private readonly Button _okButton;
        private readonly Button _cancelButton;
        public GameObject InnerGameObject { get { return _uiGameObject; } } 

        public DialogInstance(GameObject uiGameObject)
        {
            _uiGameObject = uiGameObject;
            _okButton = _uiGameObject.GetChildComponentByName<Button>("OkButton");
            _cancelButton = _uiGameObject.GetChildComponentByName<Button>("CancelButton");
        }

        public void WireEvents(Action<DialogInstance> okCallback, Action<DialogInstance> cancelCallback)
        {            
            _okButton.onClick.AddListener(() => Done(okCallback));
            _cancelButton.onClick.AddListener(() => Done(cancelCallback));
        }

        private void UnWireEvents()
        {
            _okButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
        }

        public void FadeIn()
        {
            var canvasGroup = _uiGameObject.GetComponentInChildren<CanvasGroup>();

            canvasGroup.alpha = 0;

            const float fadeInSeconds = 0.2f;
            const float scaleInSeconds = 0.4f;
            const float scaleSize = 1.5f;

            canvasGroup.ZKalphaTo(1, fadeInSeconds)
                .setFrom(0)
                .start();

            canvasGroup.GetComponent<RectTransform>()
                .ZKlocalScaleTo(new Vector3(scaleSize, scaleSize), scaleInSeconds)
                .setEaseType(EaseType.Punch)
                .start();
        }

        private void Done(Action<DialogInstance> doneCallback)
        {            
            UnWireEvents();
            FadeOutAndFinish(() =>
            {
                // calling the doneCallback first allows to access the inner GO before destrying it
                doneCallback(this);
                _uiGameObject.SetActive(false);
                UnityEngine.Object.Destroy(_uiGameObject);                
            });
        }

        private void FadeOutAndFinish(Action afterFadeOut)
        {
            const float fadeOutSeconds = 0.2f;

            var canvasGroup = _uiGameObject.GetComponentInChildren<CanvasGroup>();
            canvasGroup.ZKalphaTo(0, fadeOutSeconds)
                .setCompletionHandler(x => afterFadeOut())
                .start();
        }

    }
}