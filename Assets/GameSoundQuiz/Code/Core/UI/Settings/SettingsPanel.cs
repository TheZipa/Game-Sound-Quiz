using System;
using GameSoundQuiz.Core.UI.Base;
using GameSoundQuiz.Services.EntityContainer;
using GameSoundQuiz.Services.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace GameSoundQuiz.Core.UI.Settings
{
    public class SettingsPanel : FadeBaseWindow, IFactoryEntity
    {
        public event Action<bool> OnSoundEnableToggled;

        [SerializeField] private Animator _animator;
        [SerializeField] private Toggle _soundToggle;
        [Space(10)]
        [Header("Sprites")]
        [SerializeField] private Sprite _onView;
        [SerializeField] private Sprite _offView;

        private readonly int _toggleAnimHash = Animator.StringToHash("Toggle");
        private ISoundService _soundService;

        protected override void OnAwake()
        {
            base.OnAwake();
            _animator.SetBool(_toggleAnimHash, false);
            _soundToggle.onValueChanged.AddListener(isEnabled =>
            {
                //_soundService.PlayEffectSound(SoundId.Click);
                ToggleView(isEnabled);
                OnSoundEnableToggled?.Invoke(isEnabled);
            });
        }

        public void Construct(ISoundService soundService, bool isSoundEnabled)
        {
            _soundService = soundService;
            _soundToggle.isOn = isSoundEnabled;
            ToggleView(isSoundEnabled);
        }

        public override void Show()
        {
            base.Show();
            _animator.SetBool(_toggleAnimHash, true);
        }

        public override void Hide()
        {
            base.Hide();
            _animator.SetBool(_toggleAnimHash, false);
        }

        private void ToggleView(bool isSoundEnabled) =>
            _soundToggle.image.sprite = isSoundEnabled ? _onView : _offView;
    }
}