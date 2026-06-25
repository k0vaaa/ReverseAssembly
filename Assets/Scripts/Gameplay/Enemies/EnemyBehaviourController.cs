using System;
using Core.Events;
using Core.Extensions;
using Gameplay.Enemies.Behaviors;
using Gameplay.Events;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyBehaviourController : MonoBehaviour
    {
        [SerializeField] private EnemyBehaviorStrategy _wander;
        [SerializeField] private EnemyBehaviorStrategy _melee;

        private void Awake()
        {
            if(!enabled) return;
            var controller = GetComponent<AIController>();
            EventBus.Subscribe<BranchSwitchedEvent>(e =>
            {
                controller.ChangeStrategy(e.NewBranch == WorldBranch.Alpha ? _melee : _wander);
            }).AddTo(gameObject);
        }
        
    }
}