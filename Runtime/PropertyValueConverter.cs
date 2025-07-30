using System;
using System.Collections;
using UnityEngine;

namespace DynamicUpdater
{
    public static class PropertyValueConverter
    {
        public static object ConvertTo(Type type, object value)
        {
            if (type == typeof(Vector3))
            {
                var list = value as IList;
                return new Vector3(
                    Convert.ToSingle(list[0]),
                    Convert.ToSingle(list[1]),
                    Convert.ToSingle(list[2])
                );
            }

            if (type == typeof(Quaternion))
            {
                var list = value as IList;
                return Quaternion.Euler(
                    Convert.ToSingle(list[0]),
                    Convert.ToSingle(list[1]),
                    Convert.ToSingle(list[2])
                );
            }

            if (type == typeof(Color))
            {
                if (ColorUtility.TryParseHtmlString(value.ToString(), out var col))
                    return col;
            }

            return Convert.ChangeType(value, type);
        }
    }
}
