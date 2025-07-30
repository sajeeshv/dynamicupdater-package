using UnityEngine;

namespace DynamicUpdater
{
    public static class LibraryBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Init()
        {
            Debug.Log("[DynamicUpdater] LibraryBootstrap auto-initializing.");

            GameObject host = GameObject.Find("DynamicUpdaterHost");
            if (host == null)
            {
                host = new GameObject("DynamicUpdaterHost");
                Object.DontDestroyOnLoad(host);
            }

            if (host.GetComponent<ApplyTest>() == null)
            {
                host.AddComponent<ApplyTest>();
            }
        }
    }
} 