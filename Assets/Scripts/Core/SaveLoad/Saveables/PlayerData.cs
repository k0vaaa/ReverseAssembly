using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.SaveLoad.Saveables
{
    [Serializable]
    public class PlayerData
    {
        public float Health;
        public Vector3 Position;
        public List<EnemyData> Enemies; 
    }

    [Serializable]
    public class EnemyData
    {
        public string Id;
        public float Health;
        public Vector3 Position;
        public int PrefabIndex;
        public bool IsBoss;
    }
    
}