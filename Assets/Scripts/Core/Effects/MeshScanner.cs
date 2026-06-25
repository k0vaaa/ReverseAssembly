using UnityEngine;
using UnityEngine.VFX;

namespace Core.Effects
{
    [ExecuteAlways]
    [RequireComponent(typeof(VisualEffect))]
    public class MeshScanner : MonoBehaviour
    {
        [Header("Что сканируем?")]
        public MeshFilter targetObject; 

        [Header("Настройки сканера")]
        public float scanSpeed = 1f;       // Скорость (метров в секунду)
        public float scanThickness = 0.1f;

        private VisualEffect _vfx;
        private Renderer _targetRenderer;

        private void OnEnable()
        {
            _vfx = GetComponent<VisualEffect>();
        }

        public void UpdateRenderer()
        {
            if (targetObject == null) return;
            _targetRenderer = targetObject.GetComponent<Renderer>();
        }
        private void Update()
        {
            if (targetObject == null || targetObject.sharedMesh == null || _vfx == null)
            {
                return;
            }




            _vfx.SetMesh("TargetMesh", targetObject.sharedMesh);

            // 1. Матрица для правильного прилипания лучей (оставляем как было)
            Matrix4x4 matrix = transform.worldToLocalMatrix * targetObject.transform.localToWorldMatrix;
            _vfx.SetMatrix4x4("TargetMatrix", matrix);

            // 2. Отправляем в VFX мировую матрицу объекта (чтобы найти его верх и низ)
            _vfx.SetMatrix4x4("ObjectWorldMatrix", targetObject.transform.localToWorldMatrix);

            // --- МИРОВЫЕ КООРДИНАТЫ СКАНЕРА ---
        
            // Renderer.bounds дает идеальную коробку вокруг объекта в Мировых координатах
            Bounds worldBounds = _targetRenderer.bounds;
            float minY = worldBounds.min.y;
            float height = worldBounds.size.y;

            // Линия ездит в мировых координатах
            float currentScanY = Mathf.PingPong(Time.time * scanSpeed, height) + minY;
        
            _vfx.SetFloat("ScanLineY", currentScanY);
        
            // Больше не нужно делить толщину на Scale! В мировых координатах 0.1 это всегда 10 см.
            _vfx.SetFloat("ScanLineThickness", scanThickness); 
        }
    }
}