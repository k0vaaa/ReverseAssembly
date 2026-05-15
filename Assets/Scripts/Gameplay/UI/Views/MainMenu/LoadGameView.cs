using System;
using System.Collections.Generic;
using Core.UI;
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
        
        public void ShowLoadGameMenu(List<string> timestamps, Action<string> callback)
        {
            // Очищаем старые кнопки
            foreach (Transform child in _saveContainer)
                Destroy(child.gameObject);

            // Создаем кнопку для каждого сохранения
            foreach (string timestamp in timestamps)
            {
                Button saveButton = Instantiate(_buttonPrefab, _saveContainer);
                string formattedTime = FormatTimestamp(timestamp); // Форматируем для читаемости
                saveButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Save {formattedTime}";
                saveButton.onClick.AddListener(() => callback.Invoke(timestamp));
            }
        }
        
        private string FormatTimestamp(string timestamp)
        {
            DateTime dateTime = DateTime.ParseExact(timestamp, "yyyyMMddHHmmss", null);
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}