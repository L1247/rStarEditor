#region

using UnityEngine;

#endregion

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "MonsterData" , menuName = "MonsterData" , order = 0)]
    public class MonsterData : ScriptableObject
    {
    #region Public Variables

        public int      b;
        public ItemData itemData;
        public string   a;

    #endregion
    }
}