using System.Collections.Generic;
using System.Linq;
using Core.Bootstrap;
using Core.Inventory;
using Core.SaveLoad.Interactors;
using Core.SaveLoad.PlayerSaves;
using Core.SaveLoad.Saveables;
using Gameplay.Combat.Health;
using Gameplay.Controllers.Player;
using Gameplay.Core;
using Gameplay.Enemies;
using Gameplay.Interactables;
using Reflex.Attributes;
using UnityEngine;
using Logger = Core.Utilities.Logger;
using ExternalAssets.Mini_First_Person_Controller.Scripts;

namespace Gameplay.Core
{
    public class SaveManager
    {
        [Inject] private PlayerDataInteractor _playerDataInteractor;
        [Inject] private SyncEnergyManager _energyManager;
        [Inject] private InventoryManager _inventory;
        [Inject] private BranchManager _branchManager;
        
        private StabilitySystem _playerStabilitySystem;
        private MovementController _movementController;
        private EnemyManager _enemyManager;

        public void SetPlayerSystems(StabilitySystem playerStabilitySystem, MovementController movementController)
        {
            _playerStabilitySystem = playerStabilitySystem;
            _movementController = movementController;
        }

        public void SetEnemyManager(EnemyManager enemyManager)
        {
            _enemyManager = enemyManager;
        }

        public void SaveGame()
        {
            if (_playerStabilitySystem == null || _movementController == null)
            {
                Debug.LogWarning("SaveManager is not fully initialized. Cannot save.");
                return;
            }

            var enemiesData = _enemyManager != null 
                ? _enemyManager.GetEnemyData() 
                : _playerDataInteractor.CurrentSave?.Enemies ?? new List<EnemyData>();

            var playerData = new PlayerData
            {
                Position = _movementController.transform.position,
                Rotation = _movementController.transform.rotation,
                Health = _playerStabilitySystem.Stability,
                Energy = _energyManager.CurrentEnergy,
                Enemies = enemiesData,
                CodeBlocks = _inventory.CodeBlocks,
                CurrentBranch = BranchManager.CurrentBranch,
                Puzzles = Object.FindObjectsByType<BuggableBase>(FindObjectsSortMode.None)
                    .Select(p => new PuzzleData { Id = p.Id, IsBugged = p.IsBugged })
                    .ToList()
            };
            

            _playerDataInteractor.SavePlayerData(playerData);
            Debug.Log("Game Saved Successfully!");
        }

        public void LoadPlayerAndPuzzles(StabilitySystem playerStabilitySystem, MovementController movementController)
        {
            var save = _playerDataInteractor.CurrentSave;
            if (save == null || save.Position == default)
            {
                Debug.LogWarning("No valid save found, starting as new scene.");
                return;
            }

            // Загружаем состояние игрока
            var cc = movementController.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            movementController.transform.position = save.Position;
            if (save.Rotation != default(Quaternion))
            {
                Debug.Log($"[SaveManager] Applying rotation from save: {save.Rotation.eulerAngles}");
                var fpLook = movementController.GetComponentInChildren<FirstPersonLook>();
                if (fpLook != null)
                {
                    fpLook.SetRotation(save.Rotation);
                }
                else
                {
                    movementController.transform.rotation = save.Rotation;
                }
            }
            else
            {
                Debug.Log($"[SaveManager] Rotation is default, not applying.");
            }
            Physics.SyncTransforms();
            if (cc != null) cc.enabled = true;
            
            playerStabilitySystem.SetStability(save.Health);
            _energyManager.SetEnergy(save.Energy);
            _inventory.CodeBlocks = save.CodeBlocks;
            _inventory.UpdateHUD();
            
            if (_branchManager != null)
            {
                _branchManager.SwitchBranch(save.CurrentBranch, instant: true);
            }

            // Загружаем состояние головоломок
            var puzzles = Object.FindObjectsByType<BuggableBase>(FindObjectsSortMode.None);
            if (save.Puzzles != null)
            {
                foreach (var puzzle in puzzles)
                {
                    var savedPuzzle = save.Puzzles.FirstOrDefault(p => p.Id == puzzle.Id);
                    if (savedPuzzle != null)
                    {
                        puzzle.LoadState(savedPuzzle.IsBugged);
                    }
                }
            }

            Debug.Log($"Save Loaded Successfully");
        }
    }
}
