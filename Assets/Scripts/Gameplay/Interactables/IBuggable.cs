namespace Gameplay.Interactables
{
    public interface IBuggable
    {
        bool IsBugged { get; }
        void OnScanned(bool isScanning); // Вызывается, когда сканер включен/выключен
        void OnInteract();               // Вызывается при нажатии [E] на объект
        void FixBug();                   // Вызывается после прохождения мини-игры
    }
}