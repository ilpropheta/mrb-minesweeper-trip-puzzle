using System;
using System.Collections.Generic;
using System.Linq;
using FlipWebApps.GameFramework.Scripts.Integrations.Preferences;
using UnityEngine;

namespace Assets.GameFramework.Localisation
{
    public static class LocalisationManager
    {
        public static event EventHandler OnLocalisationChanged;

        private static IDictionary<string, string> _words;
        private static bool _initialized;
        private static string _currentLanguage;

        private static void Initialize()
        {
            if (_initialized)
                return;

            _initialized = true;

            var allowedLanguages = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                {SystemLanguage.English.ToString(), "localisation/values/strings"},
                {SystemLanguage.Italian.ToString(), "localisation/values-it/strings"},
            };

            // from settings
            var languageFromSettings = PreferencesFactory.GetString(SettingsKeys.Language, useSecurePrefs: false);
            if (TrySetCurrentLanguage(allowedLanguages, languageFromSettings))
                return;

            // from device
            var languageFromSystem = Application.systemLanguage.ToString();
            if (TrySetCurrentLanguage(allowedLanguages, languageFromSystem))
                return;

            // first one allowed (default)
            TrySetCurrentLanguage(allowedLanguages, allowedLanguages.Keys.First());
        }

        private static bool TrySetCurrentLanguage(IDictionary<string, string> allowedLanguages, string languageName)
        {
            if (!allowedLanguages.ContainsKey(languageName))
                return false;

            var assetName = allowedLanguages[languageName];
            var textAsset = Resources.Load<TextAsset>(assetName);
            _words = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            new XmlStringResourceReader(_words).Read(textAsset.text);
            Resources.UnloadAsset(textAsset);
            _currentLanguage = languageName;
            return true;
        }

        public static string Get(string key)
        {
            Initialize();
            string value;
            if (!_words.TryGetValue(key, out value))
            {
                value = string.Format("['{0}'@{1}] missing", key, _currentLanguage);
#if UNITY_EDITOR
                // write the missing xml
                _words.Add(key, "");
                new XmlStringResourceWriter(_currentLanguage, _words).Write();
#endif
            }
            
            return value;
        }

        public static void SetLanguage(SystemLanguage language)
        {
            if (language == Application.systemLanguage)
                PreferencesFactory.DeleteKey(SettingsKeys.Language);
            else
                PreferencesFactory.SetString(SettingsKeys.Language, language.ToString());

            _initialized = false;
            if (OnLocalisationChanged != null)
                OnLocalisationChanged(null, null);
        }

        public static string GetCurrentLanguage()
        {
            Initialize();
            return _currentLanguage;
        }

        internal static string GetDeviceLanguage()
        {
            return Application.systemLanguage.ToString();
        }

    }
}
