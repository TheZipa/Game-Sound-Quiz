using System;
using System.Collections.Generic;
using GameSoundQuiz.Core.UI.Base;
using GameSoundQuiz.Services.Multiplayer;
using UnityEngine;
using UnityEngine.UI;
using PhotonPlayer = Photon.Realtime.Player;

namespace GameSoundQuiz.Core.UI.Rooms
{
    public class RoomScreen : FadeBaseWindow
    {
        public event Action OnRoomLeft;
        public event Action OnStartGame;
        
        public Transform PlayerFieldContent;
        [SerializeField] private Button _leaveRoomButton;
        [SerializeField] private Button _startGameButton;

        private IMultiplayerRooms _multiplayerRooms;
        private Dictionary<string, RoomPlayerField> _roomPlayerFields;
        private Stack<RoomPlayerField> _fields;
        private int _minPlayers;

        protected override void OnAwake()
        {
            base.OnAwake();
            _startGameButton.onClick.AddListener(TryStartGame);
            _leaveRoomButton.onClick.AddListener(() =>
            {
                OnRoomLeft?.Invoke();
                ClearPlayersList();
                Hide();
            });
        }

        public void Construct(IMultiplayerRooms multiplayerRooms, Stack<RoomPlayerField> roomPlayerFields, 
            int maxPlayers, int minPlayers)
        {
            _roomPlayerFields = new Dictionary<string, RoomPlayerField>(maxPlayers);
            _fields = roomPlayerFields;
            _multiplayerRooms = multiplayerRooms;
            _multiplayerRooms.OnPlayerRoomJoin += AddPlayerToRoom;
            _multiplayerRooms.OnPlayerRoomLeft += RemovePlayerFromRoom;
            _minPlayers = minPlayers;
        }

        public void SetupRoom()
        {
            foreach (PhotonPlayer player in _multiplayerRooms.GetPlayersInRoom()) AddPlayerToRoom(player);
            _startGameButton.gameObject.SetActive(_multiplayerRooms.IsMasterPlayer());
        }

        private void TryStartGame()
        {
            if (_multiplayerRooms.GetPlayersInRoom().Length < _minPlayers) return;
            OnStartGame?.Invoke();
        }

        private void AddPlayerToRoom(PhotonPlayer player)
        {
            RoomPlayerField roomPlayerField = _fields.Pop();
            roomPlayerField.ShowAndSetup(player.NickName, player.IsMasterClient);
            _roomPlayerFields.Add(player.NickName, roomPlayerField);
        }

        private void RemovePlayerFromRoom(PhotonPlayer player)
        {
            RoomPlayerField roomPlayerField = _roomPlayerFields[player.NickName];
            roomPlayerField.Hide();
            _fields.Push(roomPlayerField);
            _roomPlayerFields.Remove(player.NickName);
        }

        private void ClearPlayersList()
        {
            foreach (RoomPlayerField playerField in _roomPlayerFields.Values)
            {
                playerField.Hide();
                _fields.Push(playerField);
            }
            _roomPlayerFields.Clear();
        }

        private void OnDestroy()
        {
            _multiplayerRooms.OnPlayerRoomJoin -= AddPlayerToRoom;
            _multiplayerRooms.OnPlayerRoomLeft -= RemovePlayerFromRoom;
        }
    }
}