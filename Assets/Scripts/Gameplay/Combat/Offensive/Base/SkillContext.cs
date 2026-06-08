using Gameplay.Controllers.Player;
using Gameplay.Enemies;
using UnityEngine;

namespace Gameplay.Combat.Offensive.Base
{
    public class SkillContext
    {
        public FightController FightController;
        public VFXController VFXController;
        public SFXController SFXController;
        public GameObject Caster;
        public AIController EnemyController;
    }
}