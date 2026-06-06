using Core.Effects;
using UnityEngine;
using UnityEngine.VFX;

namespace Gameplay.Controllers.Player
{
    public class VFXController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _scannerEffect;
        [SerializeField] private VisualEffect _projectionVFX;
        [SerializeField] private MeshScanner _scannerProjection;

        public void PlayScanner(bool play = true)
        {
            if (play)
            {
                _scannerEffect.Play();
            }
            else
            {
                _scannerEffect.Stop();
            }
        }
        public void SetProjection(MeshFilter mesh)
        {
            if (_scannerProjection.targetObject == mesh) return;
            _scannerProjection.targetObject = mesh;
            _scannerProjection.enabled = mesh != null;
            _scannerProjection.UpdateRenderer();
            _projectionVFX.enabled = mesh != null;
        }
        
    }
}