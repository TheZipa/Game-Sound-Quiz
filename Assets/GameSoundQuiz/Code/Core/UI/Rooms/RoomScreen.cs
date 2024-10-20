using System;
using System.Collections.Generic;

using VContainer;
using UnityEngine;
using UnityEngine.UI;

using GameSoundQuiz.Core.UI.Base;
using GameSoundQuiz.Services.Multiplayer;
using GameSoundQuiz.Services.StaticData;
using GameSoundQuiz.Services.WindowsFactory;
using PhotonPlayer = Photon.Realtime.Player;

namespace GameSoundQuiz.Core.UI.Rooms
{
    public class RoomScreen : FadeBaseWindow
    {
        public event Action OnRoomLeft;
        public event Action OnStartGame;
        
        [SerializeField] private Transform _playerFieldContent;
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

        [Inject]
        public void Construct(IMultiplayerRooms multiplayerRooms, IWindowsFactory windowsFactory, IStaticData staticData)
        {
            _minPlayers = staticData.ApplicationConfig.MinPlayersInLobby;
            int maxPlayersInLobby = staticData.ApplicationConfig.MaxPlayersInLobby;
            _roomPlayerFields = new Dictionary<string, RoomPlayerField>(maxPlayersInLobby);
            
            _fields = CreateRoomPlayerFields(windowsFactory, maxPlayersInLobby);
            _multiplayerRooms = multiplayerRooms;
            _multiplayerRooms.OnPlayerRoomJoin += AddPlayerToRoom;
            _multiplayerRooms.OnPlayerRoomLeft += RemovePlayerFromRoom;
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
        
        private Stack<RoomPlayerField> CreateRoomPlayerFields(IWindowsFactory windowsFactory, int maxPlayers)
        {
            Stack<RoomPlayerField> roomPlayerFields = new Stack<RoomPlayerField>(maxPlayers);
            for (int i = 0; i < maxPlayers; i++)
            {
                RoomPlayerField roomPlayerField = windowsFactory.CreateWindow<RoomPlayerField>(_playerFieldContent);
                roomPlayerField.Hide();
                roomPlayerFields.Push(roomPlayerField);
            }
            return roomPlayerFields;
        }

        private void OnDestroy()
        {
            _multiplayerRooms.OnPlayerRoomJoin -= AddPlayerToRoom;
            _multiplayerRooms.OnPlayerRoomLeft -= RemovePlayerFromRoom;
        }
    }
}