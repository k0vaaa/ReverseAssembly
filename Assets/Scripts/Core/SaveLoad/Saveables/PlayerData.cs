using System;
using System.Collections.Generic;
using Gameplay.Events;
using UnityEngine;

namespace Core.SaveLoad.Saveables
{
    [Serializable]
    public class PlayerData
    {
        public float Health;
        public float Energy;
        public Vector3 Position;
        public Quaternion Rotation;
        public WorldBranch CurrentBranch;
        public List<EnemyData> Enemies; 
        public List<PuzzleData> Puzzles;
        public int CodeBlocks;
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

    [Serializable]
    public class PuzzleData
    {
        public string Id;
        public bool IsBugged;
    }
}