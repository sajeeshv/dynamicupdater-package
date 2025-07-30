using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DynamicUpdater
{
    public static class UniversalPropertySetter
    {
        public static void ApplyConfig(GameObject target, Dictionary<string, object> config)
        {
            foreach (var kvp in config)
            {
                string property = kvp.Key;
                object value = kvp.Value;

                if (!TrySetPropertyOnAny(target, property, value))
                    Debug.LogWarning($"[DynamicUpdater] Failed to set '{property}' on {target.name}");
            }
        }

        private static bool TrySetPropertyOnAny(GameObject go, string property, object value)
        {
            object[] candidates = new object[]
            {
                go.transform,
                go.GetComponent<Renderer>()?.material,
                go.GetComponent<Renderer>()
            };

            // All attached components (excluding nulls)
            candidates = candidates
                .Concat(go.GetComponents<Component>())
                .Where(x => x != null)
                .ToArray();

            foreach (var obj in candidates)
            {
                if (TrySetProperty(obj, property, value))
                    return true;
            }

            return false;
        }

        private static bool TrySetProperty(object targetObj, string propertyName, object value)
        {
            var type = targetObj.GetType();

            var prop = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            var field = type.GetField(propertyName, BindingFlags.Public | BindingFlags.Instance);

            Type targetType = prop?.PropertyType ?? field?.FieldType;
            if (targetType == null) return false;

            object converted = PropertyValueConverter.ConvertTo(targetType, value);
            try
            {
                if (prop != null)
                    prop.SetValue(targetObj, converted);
                else if (field != null)
                    field.SetValue(targetObj, converted);
                else
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

