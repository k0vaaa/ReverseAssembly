using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Views.Components
{
    public class LoadButton : MonoBehaviour
    {
        [SerializeField] private Button _main;
        [SerializeField] private Button _delete;

        public event Action OnClick;
        public event Action OnDeleteClick;

        private void Awake()
        {
            _main.onClick.AddListener(() => OnClick?.Invoke());
            _delete.onClick.AddListener(() => OnDeleteClick?.Invoke());
        }
    }
}