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
            if (damageable == null) return;
            if (damageable == _self) return;
            DoDamage(damageable);
            Destroy(gameObject);
        }

        public IEnumerator Ttl()
        {
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }
        
    }
}