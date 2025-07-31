using UnityEngine;

namespace DynamicUpdater
{
    public static class WebGLBuildValidator
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void ValidateWebGLBuild()
        {
            Debug.Log("[DynamicUpdater] WebGL Build Validator - Checking if package is loaded in WebGL build.");
            
            // Check if our types are available
            var applyTestType = typeof(ApplyTest);
            var bootstrapType = typeof(LibraryBootstrap);
            
            Debug.Log($"[DynamicUpdater] ApplyTest type available: {applyTestType != null}");
            Debug.Log($"[DynamicUpdater] LibraryBootstrap type available: {bootstrapType != null}");
            
            // Check if the host was created by LibraryBootstrap
            GameObject host = GameObject.Find("DynamicUpdaterHost");
            if (host != null)
            {
                Debug.Log("[DynamicUpdater] DynamicUpdaterHost found in WebGL build.");
                if (host.GetComponent<ApplyTest>() != null)
                {
                    Debug.Log("[DynamicUpdater] ApplyTest component found in WebGL build.");
                }
                else
                {
                    Debug.Log("[DynamicUpdater] ApplyTest component NOT found in WebGL build.");
                }
            }
            else
            {
                Debug.Log("[DynamicUpdater] DynamicUpdaterHost NOT found in WebGL build.");
            }
            
            Debug.Log("[DynamicUpdater] WebGL build validation complete.");
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void EarlyWebGLInit()
        {
            Debug.Log("[DynamicUpdater] Early WebGL initialization.");
        }
    }
} 