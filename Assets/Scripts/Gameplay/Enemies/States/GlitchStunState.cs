using UnityEngine.AI;

namespace Gameplay.Enemies.States
{
    public class GlitchStunState : EnemyState
    {
        public GlitchStunState(AIController controller, EnemyAnimator animator, EnemyMover mover) 
            : base(controller, animator, mover) { }

        protected override void EnterAction()
        {
           
            Mover.Stop();
            
            // Запускаем эффект глитча!
            if (EnemyAnimator != null)
            {
                EnemyAnimator.StartGlitchStun();
            }
        }

        protected  override void ExecuteAction()
        {
            
        }

        protected  override void ExitAction()
        {
            Mover.Resume();
            
            // Выключаем эффект и возвращаем анимации в норму
            if (EnemyAnimator != null)
            {
                EnemyAnimator.StopGlitchStun();
            }
        }
    }
}
