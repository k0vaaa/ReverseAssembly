using Gameplay.Combat.Offensive.ScriptableObjects;
using UnityEngine;

namespace Gameplay.Combat.Offensive.Base
{
    public abstract class Skill : ISkill
    {
        protected Damage Damage { get; private set; }
        private float _cooldownTime;
        private float timeUntilReady { get; set; } = 0f;
        public bool IsReady { get; private set; }

        protected Skill(AbilityDefinition abilityDefinition)
        {
            Damage = abilityDefinition.damage;
            _cooldownTime = abilityDefinition.cooldownTime;
        }

        public virtual void Init()
        {
            
        }

        private void CheckCooldown()
        {
            if (timeUntilReady <= 0)
            {
                IsReady = true;
                timeUntilReady = 0;
            }
            else
            {
                IsReady = false;
                timeUntilReady -= Time.deltaTime;
            }
        }

        public float GetReadyPercent() => Mathf.Clamp(timeUntilReady / _cooldownTime,0f,1f);
       

        public void Tick()
        {
            CheckCooldown();
            OnTick();
        }

        protected abstract void OnTick();


        public void TryCast()
        {
            if(!IsReady) return;
            
            if (CastAction())
            {
                timeUntilReady = _cooldownTime;
            }

            
        }

        protected abstract bool CastAction();
    }
}