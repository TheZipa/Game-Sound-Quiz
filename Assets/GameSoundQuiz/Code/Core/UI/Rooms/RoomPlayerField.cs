using System;
using GameSoundQuiz.Core.UI.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSoundQuiz.Core.UI.Rooms
{
    public class RoomPlayerField : BaseWindow
    {
        public event Action<RoomPlayerField> OnAdditionalMenuToggled;
        public event Action OnSetMasterButtonClick;
        
        [SerializeField] private Image _personIcon;
        [SerializeField] private TextMeshProUGUI _playerName;
        [SerializeField] private GameObject _additionalMenuView;
        [SerializeField] private Button _additionalMenuButton;
        [SerializeField] private Button _setMasterButton;

        private void Awake()
        {
            _additionalMenuButton.onClick.AddListener(ToggleAdditionalMenu);
            _setMasterButton.onClick.AddListener(() => OnSetMasterButtonClick?.Invoke());
        }

        public void ShowAndSetup(string playerName)
        {
            _playerName.text = playerName;
            Show();
        }

        public void SetVisibleAdditionalMenuButton(bool isVisible) =>
            _additionalMenuButton.gameObject.SetActive(isVisible);

        public void HideAdditionalMenu() => _additionalMenuView.SetActive(false);

        private void ToggleAdditionalMenu()
        {
            _additionalMenuView.SetActive(!_additionalMenuView.activeSelf);
            OnAdditionalMenuToggled?.Invoke(this);
        }
    }
}