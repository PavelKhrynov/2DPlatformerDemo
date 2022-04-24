using System.Collections;
using UnityEngine;

namespace WhereIAm.Scripts.Utils
{
    public static class GameObjectExtensions
    {
        public static bool IsInLayer(this GameObject go, LayerMask layerMask)
        {
            return layerMask == (layerMask | 1 << go.layer);
        }
    }
}