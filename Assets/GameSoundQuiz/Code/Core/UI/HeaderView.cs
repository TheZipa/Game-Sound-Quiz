using System;
using GameSoundQuiz.Core.UI.Base;

using UnityEngine;
using UnityEngine.UI;

namespace GameSoundQuiz.Core.UI
{
    public class HeaderView : BaseWindow
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _backButton;

        public event Action OnSettingsClick;

        private Action _backClickAction;

        public void SetBackAction(Action backClickAction) => _backClickAction = backClickAction;

        public void ToggleBackButton(bool isEnabled) => _backButton.gameObject.SetActive(isEnabled);

        private void Awake()
        {
            _backButton.onClick.AddListener(() => _backClickAction?.Invoke());
            _settingsButton.onClick.AddListener(() => OnSettingsClick?.Invoke());
        }
    }
}