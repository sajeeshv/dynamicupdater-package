using UnityEngine;
using System.Collections.Generic;
using System;

namespace DynamicUpdater
{
    public class ApplyTest : MonoBehaviour
    {
        void Start()
        {
            Debug.Log("[DynamicUpdater] ApplyTest attached and running.");
        }

        /// <summary>
        /// Receives configuration data from React and applies it to the target GameObject
        /// </summary>
        /// <param name="jsonData">JSON string containing targetName and property-value pairs</param>
        public void ReceiveConfigFromReact(string jsonData)
        {
            Debug.Log($"[DynamicUpdater] Received config data: {jsonData}");
            
            // Parse JSON data
            var parsedData = ParseFullConfigData(jsonData);
            if (parsedData == null)
            {
                Debug.LogError("[DynamicUpdater] Failed to parse config data!");
                return;
            }

            string targetName = parsedData.Item1;
            Dictionary<string, object> config = parsedData.Item2;

            // Find the target GameObject
            GameObject target = GameObject.Find(targetName);
            if (target == null)
            {
                Debug.LogError($"[DynamicUpdater] Target GameObject '{targetName}' not found!");
                return;
            }

            // Apply each property individually
            foreach (var kvp in config)
            {
                string propertyName = kvp.Key;
                object propertyValue = kvp.Value;
                
                // Create single property config
                var singleConfig = new Dictionary<string, object> { { propertyName, propertyValue } };
                
                // Apply this single property
                UniversalPropertySetter.ApplyConfig(target, singleConfig);
                
                Debug.Log($"[DynamicUpdater] Applied {propertyName} = {propertyValue} to '{targetName}'");
            }
            
            Debug.Log($"[DynamicUpdater] Successfully applied all config to '{targetName}'");
        }

        private System.Tuple<string, Dictionary<string, object>> ParseFullConfigData(string jsonData)
        {
            try
            {
                // Use Unity's JsonUtility for simple parsing
                var configData = JsonUtility.FromJson<ConfigData>(jsonData);
                if (configData == null)
                {
                    Debug.LogError("[DynamicUpdater] Failed to parse JSON data!");
                    return null;
                }

                if (string.IsNullOrEmpty(configData.targetName))
                {
                    Debug.LogError("[DynamicUpdater] targetName not found in config data!");
                    return null;
                }

                // Convert to Dictionary for UniversalPropertySetter
                var config = new Dictionary<string, object>();
                if (configData.position != null) config["position"] = configData.position;
                if (configData.scale != null) config["scale"] = configData.scale;
                if (configData.rotation != null) config["rotation"] = configData.rotation;
                if (configData.color != null) config["color"] = configData.color;
                if (configData.intensity != null) config["intensity"] = configData.intensity;
                if (configData.enabled != null) config["enabled"] = configData.enabled;

                return new System.Tuple<string, Dictionary<string, object>>(configData.targetName, config);
            }
            catch (Exception e)
            {
                Debug.LogError($"[DynamicUpdater] Error parsing full config data: {e.Message}");
                return null;
            }
        }

        [Serializable]
        private class ConfigData
        {
            public string targetName;
            public float[] position;
            public float[] scale;
            public float[] rotation;
            public string color;
            public float? intensity;
            public bool? enabled;
        }
    }
}

