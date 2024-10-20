using System;
using System.Collections.Generic;

using TMPro;
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

        [Header("Master User")] 
        [SerializeField] private TextMeshProUGUI _masterUserName;

        private IMultiplayerRooms _multiplayerRooms;
        private Dictionary<string, RoomPlayerField> _roomPlayerFields;
        private Stack<RoomPlayerField> _fields;

        private PhotonPlayer _masterPlayer;
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
            _multiplayerRooms.OnMasterPlayerChanged += SetMasterPlayer;
        }

        public void SetupRoom()
        {
            foreach (PhotonPlayer player in _multiplayerRooms.GetPlayersInRoom()) AddPlayerToRoom(player);
            SetVisibleStartButton();
        }
        
        private void SetVisibleStartButton() => _startGameButton.gameObject.SetActive(_multiplayerRooms.IsMasterPlayer());

        private void TryStartGame()
        {
            if (_multiplayerRooms.GetPlayersInRoom().Length < _minPlayers) return;
            OnStartGame?.Invoke();
        }

        private void AddPlayerToRoom(PhotonPlayer player)
        {
            if(player.IsMasterClient) SetMasterPlayer(player);
            else AddPlayerField(player);
        }

        private void AddPlayerField(PhotonPlayer player)
        {
            RoomPlayerField roomPlayerField = _fields.Pop();
            roomPlayerField.ShowAndSetup(player.NickName);
            roomPlayerField.OnSetMasterButtonClick += () => SetMasterPlayer(player);
            roomPlayerField.OnAdditionalMenuToggled += CloseAdditionalRemoveForAllField;
            _roomPlayerFields.Add(player.NickName, roomPlayerField);
        }

        private void RemovePlayerFromRoom(PhotonPlayer player)
        {
            RoomPlayerField roomPlayerField = _roomPlayerFields[player.NickName];
            roomPlayerField.Hide();
            roomPlayerField.OnSetMasterButtonClick -= () => SetMasterPlayer(player);
            roomPlayerField.OnAdditionalMenuToggled -= CloseAdditionalRemoveForAllField;
            _fields.Push(roomPlayerField);
            _roomPlayerFields.Remove(player.NickName);
        }

        private void SetMasterPlayer(PhotonPlayer player)
        {
            if (_masterPlayer is not null) AddPlayerField(player);
            _masterPlayer = player;
            _masterUserName.text = _masterPlayer.NickName;
            SetVisibleStartButton();
        }

        private void CloseAdditionalRemoveForAllField(RoomPlayerField sender)
        {
            foreach (RoomPlayerField roomPlayerField in _fields)
            {
                if(roomPlayerField == sender) continue;
                roomPlayerField.HideAdditionalMenu();
            }
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
            _multiplayerRooms.OnMasterPlayerChanged -= SetMasterPlayer;
        }
    }
}