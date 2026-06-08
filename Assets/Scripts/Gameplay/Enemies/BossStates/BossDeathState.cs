using System.Collections;
using UnityEngine;

namespace Gameplay.Enemies.BossStates
{
    public class BossDeathState : StatesBossConst
    {
        private Canvas _hpCanvas;

        public BossDeathState(AIController controller, BossAnimator animator, EnemyMover mover, Canvas hpCanvas) : base(controller, animator, mover)
        {
            _hpCanvas = hpCanvas;
        }

        protected override void EnterAction()
        {
            if (_hpCanvas != null) _hpCanvas.enabled = false;
            Controller.GetComponent<Collider>().enabled = false;
            BossAnimator.DeathEvent();
            Mover.Stop();
            
            Controller.StartCoroutine(DestroyCoroutine());
        }

        protected override void ExecuteAction()
        {
        }

        protected override void ExitAction()
        {
        }

        private IEnumerator DestroyCoroutine()
        {
            yield return new WaitUntil(() => BossAnimator.CheckAnimationState(0, 0.99f, "BossDeath"));
            Object.Destroy(Controller.gameObject);
        }
    }
}