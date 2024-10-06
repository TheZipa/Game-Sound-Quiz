using System;
using GameSoundQuiz.Core.UI.Base;
using GameSoundQuiz.Core.UI.Settings;
using GameSoundQuiz.Services.EntityContainer;
using GameSoundQuiz.Services.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace GameSoundQuiz.Core.UI
{
    public class TopPanel : BaseWindow, IFactoryEntity
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _backButton;
        
        private ISoundService _soundService;
        private SettingsPanel _settingsPanel;
        
        private Action _backClickAction;

        public void Construct(ISoundService soundService, SettingsPanel settingsPanel)
        {
            _soundService = soundService;
            _settingsPanel = settingsPanel;
            _settingsPanel.OnSoundEnableToggled += isSoundEnabled => _soundService.IsSoundMuted = isSoundEnabled;
        }

        public void SetBackAction(Action backClickAction) => _backClickAction = backClickAction;

        public void ToggleMainMenuStateView()
        {
            _backButton.gameObject.SetActive(false);
        }

        public void ToggleGameplayStateView()
        {
            _backButton.gameObject.SetActive(true);
        }

        private void Awake()
        {
            _settingsButton.onClick.AddListener(ToggleSettingsPanel);
            _backButton.onClick.AddListener(() => _backClickAction?.Invoke());
        }

        private void ToggleSettingsPanel()
        {
            if(_settingsPanel.gameObject.activeSelf) _settingsPanel.Hide();
            else _settingsPanel.Show();
        }
    }
}