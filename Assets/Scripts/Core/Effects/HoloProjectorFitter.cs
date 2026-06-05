using UnityEngine;
using UnityEngine.VFX;

namespace Core.Effects
{
    [ExecuteAlways]
    [RequireComponent(typeof(VisualEffect))]
    public class HoloProjectorFitter : MonoBehaviour
    {
        [Header("Настройки")]
        [Tooltip("Перетащите сюда ваш экран (Canvas или Image)")]
        public RectTransform targetScreen; 
    
        private VisualEffect _vfx;

        private void OnEnable()
        {
            _vfx = GetComponent<VisualEffect>();
        }

        private void Update()
        {
        
            if (targetScreen == null || _vfx == null || _vfx.visualEffectAsset.name != "ProjectionTargetable") return;

            // 1. Находим мировые векторы нашего UI-экрана (с учетом его поворота и скейла)
            // TransformVector превращает локальную ширину/высоту в реальное 3D-направление в мире
            Vector3 worldCenter = targetScreen.position;
            Vector3 worldRight = targetScreen.TransformVector(new Vector3(targetScreen.rect.width, 0, 0));
            Vector3 worldUp = targetScreen.TransformVector(new Vector3(0, targetScreen.rect.height, 0));

            // 2. Так как VFX Graph работает в своих локальных координатах, 
            // переводим эти мировые векторы в локальную систему координат проектора
            Vector3 localCenter = transform.InverseTransformPoint(worldCenter);
            Vector3 localRight = transform.InverseTransformVector(worldRight);
            Vector3 localUp = transform.InverseTransformVector(worldUp);

            // 3. Отправляем эти 3 вектора в VFX Graph
            _vfx.SetVector3("TargetCenter", localCenter);
            _vfx.SetVector3("TargetRight", localRight);
            _vfx.SetVector3("TargetUp", localUp);
        }
    }
}