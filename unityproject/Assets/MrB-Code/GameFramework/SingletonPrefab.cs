using UnityEngine;
using UnityEngine.Assertions;

// Based on http://wiki.unity3d.com/index.php/Singleton

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public abstract class SingletonPrefab<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static bool _applicationIsQuitting;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var existingInstances = FindObjectsOfType<T>();
                if (existingInstances.Length == 1)
                    return existingInstances[0];

                Assert.IsFalse(existingInstances.Length > 1, string.Format("[Singleton] found more than one {0} instance!", typeof(T).Name));
                Assert.IsFalse(_applicationIsQuitting, string.Format("[Singleton] Instance {0} already destroyed on application quit.", typeof(T).Name));
                if (_applicationIsQuitting)
                    return null;

                GameObject singleton;

                // Check if exists a singleton prefab on Resources Folder.
                // -- Prefab must be named as TPrefab
                var prefabName = string.Format("{0}Prefab", typeof(T).Name);
                var singletonPrefab = Resources.Load<GameObject>(prefabName);

                // Create singleton as new or from prefab
                if (singletonPrefab != null)
                {
                    singleton = Instantiate(singletonPrefab);
                    _instance = singleton.GetComponent<T>();
                }
                else
                {
                    singleton = new GameObject();
                    _instance = singleton.AddComponent<T>();
                }
                singleton.name = typeof(T).Name + "_SingletonPrefab";
                DontDestroyOnLoad(singleton);
            }
            return _instance;
        }
    }

    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy()
    {
        _applicationIsQuitting = true;
    }
}