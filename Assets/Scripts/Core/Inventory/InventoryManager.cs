using System;
using Core.Events;
using Core.Extensions;
using Core.Input;
using Core.SaveLoad.PlayerSaves;
using Core.UI;
using Gameplay.Events;
using Gameplay.UI;
using Reflex.Attributes;
using UnityEngine;

namespace Core.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [Inject] private PlayerDataInteractor _playerData;
        [Inject] private ViewManager _viewManager;
        [Inject] private InputManager _input;
        
        
        
        private PlayerHUDView _hudView;
        private int _codeBlocks;

        public void Awake()
        {
            EventBus.Subscribe<CodeBlockCollectedEvent>(OnBlockCollected).AddTo(gameObject);
            
            // Получаем HUD и обновляем сразу
            _hudView = _viewManager.GetView<PlayerHUDView>();
            if (_hudView != null)
            {
                // Показываем HUD если он скрыт
                _hudView.Show();
                UpdateHUD();
            }
        }

       

        private void OnBlockCollected(CodeBlockCollectedEvent data)
        {
            _codeBlocks++;
            //_playerData.CurrentSave.CodeBlocks++;
            //Debug.Log($"Collected! Total blocks: {_playerData.CurrentSave.CodeBlocks}");
            
            UpdateHUD();
        }

        private void UpdateHUD()
        {
            if (_hudView != null)
            {
                _hudView.UpdateBlocksCount(_codeBlocks);
            }
        }

        
    }
}
