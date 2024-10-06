using System;
using GameSoundQuiz.Core.UI.Base;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSoundQuiz.Core.UI.Rooms
{
    public class RoomConnectField : BaseWindow
    {
        public event Action<RoomInfo> OnRoomConnectPressed;
        
        [SerializeField] private TextMeshProUGUI _roomName;
        [SerializeField] private TextMeshProUGUI _roomPlayersCount;
        [SerializeField] private Button _connectButton;
        private RoomInfo _roomInfo;

        private void Awake() => _connectButton.onClick.AddListener(() => OnRoomConnectPressed?.Invoke(_roomInfo));

        public void UpdateRoomData(RoomInfo roomInfo)
        {
            bool isRoomFulled = roomInfo.PlayerCount == roomInfo.MaxPlayers;
            _roomInfo = roomInfo;
            _roomName.text = roomInfo.Name;
            _roomPlayersCount.color = isRoomFulled ? Color.red : Color.green;
            _roomPlayersCount.text = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
            _connectButton.interactable = !isRoomFulled;
        }
    }
}