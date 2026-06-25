using Core.Events;
using Gameplay.Combat.Health;
using Gameplay.Events;
using UnityEngine;

namespace Gameplay.Controllers.Player
{
    public class PlayerDeathHandler : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<StabilitySystem>().OnDeath.AddListener(HandleDeath);
        }

        private void HandleDeath()
        {
            EventBus.Raise(new PlayerDeathEvent());
        }
    }
}