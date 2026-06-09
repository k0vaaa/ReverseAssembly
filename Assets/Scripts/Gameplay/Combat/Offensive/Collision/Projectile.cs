using System.Collections;
using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Base;
using UnityEngine;

namespace Gameplay.Combat.Offensive.Collision
{
    public class Projectile : MonoBehaviour, IDamaging
    {
        private Damage _damage;
        private IDamageable _self;

        public void Init(Damage damage, IDamageable self)
        {
            _damage = damage;
            _self = self;
        }

        public void DoDamage(IDamageable damageable)
        {
            damageable.TakeDamage(_damage);
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null && damageable != _self)
            {
                DoDamage(damageable);
                
                var glitchable = other.gameObject.GetComponent<IGlitchable>();
                glitchable?.ApplyGlitchStun(3f);
            }
            
            // Снаряд должен уничтожаться в любом случае при столкновении с чем-либо (если это не сам стрелок)
            if (damageable != _self && other.gameObject.layer != LayerMask.GetMask("Player","Weapon","Triggers"))
            {
                Destroy(gameObject);
            }
        }

        public IEnumerator Ttl()
        {
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }
        
    }
}