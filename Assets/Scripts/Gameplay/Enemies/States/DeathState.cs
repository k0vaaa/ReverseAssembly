using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies.States

{  
    
    public class DeathState : StatesEnemyConst
    {   
        private readonly Canvas hpCanvas;
        public DeathState(EnemyController enemyController, EnemyAnimator animator, NavMeshAgent navMeshAgent, Canvas hpCanvas) : base(enemyController, animator, navMeshAgent)
        {
            this.hpCanvas = hpCanvas;
            
        }
        
        public override void Enter()
        {
            // EnemyController.isDead = true;
            hpCanvas.enabled = false;
            EnemyController.GetComponent<Collider>().enabled = false;
            EnemyAnimator.DeathEvent();
            NavMeshAgent.ResetPath();
            NavMeshAgent.isStopped = true;
            NavMeshAgent.angularSpeed = 0f;
            // Debug.Log(EnemyController.isDead);
            EnemyController.StartCoroutine(Destroy());
            
        }

        public override void Execute()
        {
        }

        public override void Exit()
        {
        }

        public IEnumerator Destroy()
        {
            yield return new WaitUntil(()=>EnemyAnimator.CheckAnimationState(0, 0.99f, "deathTest"));
            Object.Destroy(EnemyController.gameObject);
            
            
        }
    }
}