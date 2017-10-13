using Prime31.ZestKit;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameFramework
{
    internal static class ColorHelpers
    {
        public static void AlphaTo(this Image image, float alpha)
        {
            var color = image.color;
            image.color = new Color(color.r, color.g, color.b, alpha);
        }
    }

    internal static class ZestKitExtensions
    {
        public static void EnsureZestKitIsProperlyConfigured(this ZestKit instance)
        {
            ZestKit.removeAllTweensOnLevelLoad = true;
        }
    }
}
