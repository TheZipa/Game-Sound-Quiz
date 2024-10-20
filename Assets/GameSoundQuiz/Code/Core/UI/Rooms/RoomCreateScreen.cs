using System;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

using GameSoundQuiz.Core.UI.Base;

namespace GameSoundQuiz.Core.UI.Rooms
{
    public class RoomCreateScreen : FadeBaseWindow
    {
        public event Action<string> OnRoomCreated;
        
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _createRoomButton;
        [SerializeField] private TMP_InputField _roomNameInputField;
        [SerializeField] private TextMeshProUGUI _createRoomErrorMessage;
        [SerializeField] private int _minRoomNameCharacters;

        protected override void OnAwake()
        {
            base.OnAwake();
            _closeButton.onClick.AddListener(Hide);
            _createRoomButton.onClick.AddListener(ValidateRoomName);
        }

        public override void Show()
        {
            _createRoomErrorMessage.gameObject.SetActive(false);
            base.Show();
        }

        private void ValidateRoomName()
        {
            if (_roomNameInputField.text.Length < _minRoomNameCharacters)
                ShowRoomErrorMessage("Invalid room name");
            else
                OnRoomCreated?.Invoke(_roomNameInputField.text);
        }

        private void ShowRoomErrorMessage(string message)
        {
            _createRoomErrorMessage.text = message;
            _createRoomErrorMessage.gameObject.SetActive(true);
        }
    }
}