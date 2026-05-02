using System.Collections;
using Gameplay.Combat.Interfaces;
using Gameplay.Combat.Offensive.Base;
using UnityEngine;

namespace Gameplay.Combat.Offensive.Collision
{
    public class AOESpell : MonoBehaviour, IDamaging
    {
        private Damage _damage;
        private IDamageable _self;
        private IDamageable _damageable;

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
            _damageable = other.gameObject.GetComponent<IDamageable>();
            if (_damageable == null) return;
            if (_damageable == _self) return;
            
            
            
            InvokeRepeating(nameof(DamageRepeating), 1f, 1f);

        }
        
        private void DamageRepeating() => DoDamage(_damageable);

        private void OnTriggerExit(Collider other)
        {
            _damageable = other.gameObject.GetComponent<IDamageable>();
            if (_damageable == null) return;
            if (_damageable == _self) return;
            
            CancelInvoke(nameof(DamageRepeating));
        }
        
        public IEnumerator Ttl()
        {
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }
    }
}