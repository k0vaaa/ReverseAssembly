using Core.Bootstrap;
using Core.DI;
using Core.Events;
using Gameplay.Events;
using UnityEngine;

namespace Gameplay.Core
{
    public class BranchManager : MonoBehaviour, IInitializable
    {
        public WorldBranch CurrentBranch { get; private set; }

        [Header("Environments")]
        [SerializeField] private GameObject _mainBranchEnv;
        [SerializeField] private GameObject _alphaBranchEnv;

        public void Init()
        {
            // По умолчанию начинаем в Main
            SwitchBranch(WorldBranch.Main);
        }

        public void SwitchBranch(WorldBranch branch)
        {
            CurrentBranch = branch;

            // Жестко переключаем объекты окружения
            if (_mainBranchEnv != null) _mainBranchEnv.SetActive(CurrentBranch == WorldBranch.Main);
            if (_alphaBranchEnv != null) _alphaBranchEnv.SetActive(CurrentBranch == WorldBranch.Alpha);

            // Рассылаем глобальное событие (для звуков, партиклов или ИИ)
            EventBus.Raise(new BranchSwitchedEvent(CurrentBranch));
        }

        // Метод для вызова из Терминала
        public void ToggleBranch()
        {
            SwitchBranch(CurrentBranch == WorldBranch.Main ? WorldBranch.Alpha : WorldBranch.Main);
        }
    }
}