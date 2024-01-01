#region

using UnityEngine;

#endregion

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "ItemData" , menuName = "ItemData" , order = 0)]
    public class ItemData : ScriptableObject
    {
    #region Public Variables

        public Texture2D texture2D;

    #endregion
    }
}