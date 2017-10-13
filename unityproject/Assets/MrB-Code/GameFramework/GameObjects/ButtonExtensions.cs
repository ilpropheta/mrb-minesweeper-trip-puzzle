using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.GameFramework.GameObjects
{
    internal static class ButtonExtensions
    {
        internal static void WireClickEvent(this Button button, UnityAction action)
        {
            if (button != null)
                button.onClick.AddListener(action);
        }

    }
}
