using System;
using UnityEngine;

namespace Assets.GameFramework.Localisation.Components
{
    /// <summary>
    /// Localises a Text field based upon the given Key
    /// </summary>
    [RequireComponent(typeof(UnityEngine.UI.Text))]
//[ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class LocaliseText : MonoBehaviour 
    {
        private UnityEngine.UI.Text _textComponent;

        [Tooltip("Localization key")]
        public string Key = "insert_key_here";

        void Awake()
        {
            _textComponent = GetComponent<UnityEngine.UI.Text>();

            DoLocalise(null, null);
            LocalisationManager.OnLocalisationChanged += DoLocalise;
        }


        void OnDestroy()
        {
            LocalisationManager.OnLocalisationChanged -= DoLocalise;
        }

        private void DoLocalise(object sender, EventArgs eventArgs)
        {
            if (_textComponent == null)
                return;

            //var localisedText = Localisation.LocaliseText.Get(Key);
            var localisedText = LocalisationManager.Get(Key);
            if (!string.IsNullOrEmpty(localisedText))
                _textComponent.text = localisedText;
        }
    }
}
