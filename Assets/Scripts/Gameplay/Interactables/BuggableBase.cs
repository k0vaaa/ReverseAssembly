using System;
using Core.UI;
using DG.Tweening;
using ExternalAssets.QuickOutline.Scripts;
using Gameplay.UI.Views.Gameplay.Terminal;
using Gameplay.UI.Windows;
using Reflex.Attributes;
using UnityEngine;
using static ExternalAssets.QuickOutline.Scripts.Outline;

namespace Gameplay.Interactables
{
    public class BuggableBase : MonoBehaviour, IBuggable
    {
        [SerializeField, HideInInspector] private string _id;
        public string Id => _id;

        [Inject] protected WindowManager _windowManager;
        
        [SerializeField] private PuzzleViewBase _puzzleType;
        protected PuzzleViewBase _puzzleView;
        
        [SerializeField] protected Events.WorldBranch _requiredBranch = Events.WorldBranch.Alpha;
        [SerializeField] protected float _scannerVisibilityTime = 2f;
        [SerializeField, TextArea] private string _bugInfo;
        [SerializeField] private MeshFilter _mesh;

        protected Outline _outline;
        protected Sequence _sequence;
        private TerminalScannerView _terminalScannerView;

        private void Start()
        {
            if(!_mesh) _mesh = GetComponent<MeshFilter>();
            
            _puzzleView = _windowManager.GetWindow<TerminalWindow>().GetView(_puzzleType.GetType()) as PuzzleViewBase;
            _terminalScannerView = _windowManager.GetWindow<TerminalWindow>().GetView<TerminalScannerView>();
            OnStart();
        }

        protected virtual void OnStart()
        {
        }

        public bool IsBugged { get; protected set; } = true;
        public virtual bool IsInteractableInCurrentBranch(Events.WorldBranch branch)
        {
            return branch == _requiredBranch;
        }
        public void Scan(bool isScanning)
        {
            if (isScanning)
            {
                _terminalScannerView.SetNameText(GetName());
                _terminalScannerView.SetInfoText(GetInfo());
            }
            else
            {
                _terminalScannerView.SetNameText("-");
                _terminalScannerView.SetInfoText("-");
            }
            OnScanned(isScanning);
        }

        public virtual void OnScanned(bool isScanning)
        {
            
        }

        public virtual void OnInteract()
        {
            
        }

        public virtual void FixBug()
        {
            
        }

        public virtual void LoadState(bool isBugged)
        {
            IsBugged = isBugged;
            if (!IsBugged)
            {
                FixBug(); // По умолчанию просто вызываем FixBug, если он был починен
            }
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_id))
            {
                _id = Guid.NewGuid().ToString();
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
#endif
        

        public void Visualize()
        {
            if (_outline != null)
            {
                var mode = _outline.OutlineMode;
                _outline.OutlineColor = IsBugged ? Color.red : Color.green;
                _outline.OutlineMode = Mode.OutlineAll;
                _outline.enabled = true;
                _sequence?.Kill();
                _sequence = DOTween.Sequence();

                _sequence.AppendInterval(_scannerVisibilityTime)
                    .OnKill(() =>
                    {
                        _outline.enabled = false;
                        _outline.OutlineMode = mode;
                    });
                _sequence.Play();
            }
        }

        public string GetName() => gameObject.name;

        public string GetInfo() => _bugInfo;
        public MeshFilter GetMesh() => _mesh;
    }
}