using System.Collections;
using Gameplay.Controllers.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemies.BossStates
{
    public class BossDeathState : StatesBossConst
    {
        private SkillsController _skillsController;
        private readonly Canvas hpCanvas;
 
        public BossDeathState(BossController bossController, BossAnimator animator, NavMeshAgent navMeshAgent, Canvas hpCanvas) : base(bossController, animator, navMeshAgent)
        {
            this.hpCanvas = hpCanvas;
        }

        public override void Enter()
        {
            
            hpCanvas.enabled = false;
            BossController.GetComponent<Collider>().enabled = false;
            BossAnimator.DeathEvent();
            NavMeshAgent.ResetPath();
            NavMeshAgent.isStopped = true;
            NavMeshAgent.angularSpeed = 0f;
            BossController.StartCoroutine(Destroy());
        }

        public override void Execute()
        {
            BossController.RotateToPlayer();
        }

        public override void Exit()
        {
            
        }
        
        public IEnumerator Destroy()
        {
            yield return new WaitUntil(()=>BossAnimator.CheckAnimationState(0, 0.99f, "BossDeath"));
            Object.Destroy(BossController.gameObject);
        }
    }
}