using System;

namespace Core.SaveLoad.Saveables
{
    [Serializable]
    public class GameSettings
    {
        public float EnemiesPower = 1f;
        public bool PeaceMode;
    }
}