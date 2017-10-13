using System;
using System.Linq;
using UnityEngine;

namespace Assets.GameFramework.GameObjects
{
    /// <summary>
    /// Helper functions for quickly and simply finding and manipulating game objects
    /// </summary>
    internal static class GameObjectHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="parentGameObject"></param>
        /// <param name="name"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static TComponent GetChildComponentByName<TComponent>(this GameObject parentGameObject, string name, bool includeInactive = false) where TComponent : Component
        {
            TComponent[] components = parentGameObject.GetComponentsInChildren<TComponent>(includeInactive);
            return components.FirstOrDefault(component => component.gameObject.name == name);
        }

        /// <summary>
        /// Note uses transform
        /// </summary>
        /// <param name="parentGameObject"></param>
        /// <param name="name"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static GameObject GetChildByName(this GameObject parentGameObject, string name, bool includeInactive = false)
        {
            Transform[] components = parentGameObject.GetComponentsInChildren<Transform>(includeInactive);
            return (from component in components where component.gameObject.name == name select component.gameObject).FirstOrDefault();
        }

        /// <summary>
        /// Note uses transform
        /// </summary>
        /// <param name="thisGameObject"></param>
        /// <param name="name"></param>
        /// <param name="includeInactive"></param>
        /// <returns></returns>
        public static GameObject GetParentNamedGameObject(GameObject thisGameObject, string name)
        {
            while (true)
            {
                if (thisGameObject.name == name) return thisGameObject;
                if (thisGameObject.transform.parent == null) return null;
                thisGameObject = thisGameObject.transform.parent.gameObject;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        public static void DisableGameObjectForRemoval(GameObject gameObject)
        {
            if (gameObject.GetComponent<Collider2D>() != null)
                gameObject.GetComponent<Collider2D>().enabled = false;
            if (gameObject.GetComponent<Rigidbody2D>() != null)
                gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            gameObject.transform.position = new Vector3(0, -20); // place out of view
        }


        public static void SetLayerRecursive(GameObject gameObject, string layerName)
        {
            SetLayerRecursive(gameObject, LayerMask.NameToLayer(layerName));
        }

        public static void SetLayerRecursive(GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                SetLayerRecursive(gameObject.transform.GetChild(i).gameObject, layer);
            }
        }

        public static void DestroyChildren(Transform root)
        {
            int childCount = root.childCount;
            for (int i = 0; i < childCount; i++)
            {
                UnityEngine.Object.Destroy(root.GetChild(i).gameObject);
            }
        }

        public static bool SafeSetActive(GameObject gameObject, bool value)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(value);
                return true;
            }
            return false;
        }
    }

}