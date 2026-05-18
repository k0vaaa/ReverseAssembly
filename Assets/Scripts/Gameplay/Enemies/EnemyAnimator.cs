using System.Collections;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class EnemyAnimator : MonoBehaviour
    {
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int Walk = Animator.StringToHash("IsWalking");
        private static readonly int Attack = Animator.StringToHash("Punch");
        private static readonly int Hitted = Animator.StringToHash("Hit");

        private static readonly int AnimSpeed = Animator.StringToHash("AnimSpeed");
        //private static readonly int Spell = Animator.StringToHash("Spell");
        
        public Animator _animator { get; private set; }
        [Header("Glitch Effect")]
        [SerializeField] private Transform _visualModel;
        
        private Coroutine _glitchCoroutine;
        private Vector3 _originalModelPosition;
        
        public bool CheckAnimationState(int layerIndex, float time, string stateName) => 
            _animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime >= time && 
            _animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);

        public void Init()
        {
            _animator = GetComponent<Animator>();
        }
        
        public void DeathEvent()
        {
            _animator.SetTrigger(Death);
        }
        
        public void WalkEvent()
        {
            _animator.SetBool(Walk, true);
        }

        public void IdleEvent()
        {
            _animator.SetBool(Walk, false);
        }
        
        public void DoAttack()
        {
            _animator.SetTrigger(Attack);
        }
        
        public void DoHitEvent()
        {
            _animator.SetTrigger(Hitted);
        }

        public void StartGlitchStun()
        {
            if (_visualModel != null) _originalModelPosition = _visualModel.localPosition;
            
            if (_glitchCoroutine != null) StopCoroutine(_glitchCoroutine);
            _glitchCoroutine = StartCoroutine(GlitchRoutine());
        }

        public void StopGlitchStun()
        {
            if (_glitchCoroutine != null)
            {
                StopCoroutine(_glitchCoroutine);
                _glitchCoroutine = null;
            }

            // Возвращаем всё в норму
            _animator.SetFloat(AnimSpeed, 1);
            if (_visualModel != null) _visualModel.localPosition = _originalModelPosition;
        }

        private IEnumerator GlitchRoutine()
        {
            while (true)
            {
                // 1. Анимационный глитч (заедание времени)
                // С вероятностью 50% ставим на паузу, иначе - проигрываем рывком вперед или даже назад (скорость от -2 до +2)
                _animator.SetFloat(AnimSpeed, Random.value > 0.5f ? 0f : Random.Range(-4f, 4f));

                // 2. Пространственный глитч (тряска модельки)
                if (_visualModel != null)
                {
                    Vector3 randomOffset = new Vector3(
                        Random.Range(-0.05f, 0.05f),
                        0f, // по Y лучше не трясти, чтобы не отрывало от земли
                        Random.Range(-0.05f, 0.05f)
                    );
                    _visualModel.localPosition = _originalModelPosition + randomOffset;
                }

                // Ждем 1-4 кадра (очень быстрое обновление)
                yield return new WaitForSeconds(Random.Range(0.02f, 0.08f));
            }
        }
        
        
        
        
        
    }
}