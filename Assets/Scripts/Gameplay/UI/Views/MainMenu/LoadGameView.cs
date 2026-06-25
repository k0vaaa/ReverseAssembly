using System;
using System.Collections.Generic;
using Core.UI;
using Gameplay.UI.Views.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Views.MainMenu
{
    public class LoadGameView : View
    {
        [SerializeField] private RectTransform _saveContainer;
        [SerializeField] private Button _buttonPrefab;
        [SerializeField] private Button _goBackButton;


        public void SetBackButtonListener(Action callback)
        {
            _goBackButton.onClick.AddListener(callback.Invoke);
        }

        public void ShowLoadGameMenu(List<string> timestamps, Action<string> loadAction, Action<string> deleteAction)
        {
            // Очищаем старые кнопки
            foreach (Transform child in _saveContainer)
                Destroy(child.gameObject);

            // Создаем кнопку для каждого сохранения
            foreach (string timestamp in timestamps)
            {
                LoadButton saveButton = Instantiate(_buttonPrefab, _saveContainer).GetComponent<LoadButton>();
                string formattedTime = FormatTimestamp(timestamp); // Форматируем для читаемости
                saveButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Save {formattedTime}";
                saveButton.OnClick += () => loadAction.Invoke(timestamp);
                saveButton.OnDeleteClick += () =>
                {
                    deleteAction.Invoke(timestamp);
                    Destroy(saveButton.gameObject);
                };
            }
        }
        
        private string FormatTimestamp(string timestamp)
        {
            DateTime dateTime = DateTime.ParseExact(timestamp, "dd.MM.yyyy-HH_mm_ss", null);
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}