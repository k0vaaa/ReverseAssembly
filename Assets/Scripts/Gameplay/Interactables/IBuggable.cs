using UnityEngine;

namespace Gameplay.Interactables
{
    public interface IBuggable
    {
        bool IsBugged { get; }
        bool IsInteractableInCurrentBranch(Gameplay.Events.WorldBranch branch);

        void Scan(bool isScanning); // Вызывается, когда сканер включен/выключен
        void OnInteract();               // Вызывается при нажатии [E] на объект
        void FixBug();                   // Вызывается после прохождения мини-игры
        void Visualize();
        string GetName();
        string GetInfo();
        MeshFilter GetMesh();

    }
}