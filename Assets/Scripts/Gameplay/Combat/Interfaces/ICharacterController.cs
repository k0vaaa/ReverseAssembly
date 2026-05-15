using Gameplay.Combat.Health;
using UnityEngine;

namespace Gameplay.Combat.Interfaces
{
    public interface ICharacterController
    {
        
        bool isDead { get; set; }
        string UniqueId { get; set; } // Если UniqueId используется
        int PrefabIndex { get; set; } // Если PrefabIndex используется
        Transform transform { get; } // Для доступа к позиции
        GameObject gameObject { get; } // Для уничтожения объекта
        StabilitySystem GetComponent<T>() where T : Component; // Для доступа к компонентам
        T GetComponentInChildren<T>() where T : Component; // Для доступа к компонентам в дочерних объектах

        void Init(bool isPeaceful, Transform playerTransform);
    }
}