using System;
using System.Collections.Generic;
using System.Linq;

using VContainer;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

using GameSoundQuiz.Core.UI.Base;
using GameSoundQuiz.Services.Multiplayer;
using GameSoundQuiz.Services.WindowsFactory;

namespace GameSoundQuiz.Core.UI.Rooms
{
    public class RoomListScreen : FadeBaseWindow
    {
        public event Action OnRoomListClose;
        public event Action<RoomInfo> OnRoomConnect;
        public event Action OnRoomCreateClick;
        
        [SerializeField] Transform _roomsContent;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _createRoomButton;

        private readonly Dictionary<string, RoomConnectField> _rooms = new(10);
        
        private IMultiplayerRooms _multiplayerRooms;
        private IObjectPool<RoomConnectField> _roomFieldsPool;

        protected override void OnAwake()
        {
            base.OnAwake();
            _createRoomButton.onClick.AddListener(() => OnRoomCreateClick?.Invoke());
            _closeButton.onClick.AddListener(() =>
            {
                Hide();
                OnRoomListClose?.Invoke();
            });
        }

        [Inject]
        public void Construct(IMultiplayerRooms multiplayerRooms, IWindowsFactory windowsFactory)
        {
            _roomFieldsPool = CreateRoomConnectPool(windowsFactory);
            _multiplayerRooms = multiplayerRooms;
            _multiplayerRooms.OnRoomsUpdated += RefreshRoomList;
        }

        public void Clear()
        {
            foreach (RoomConnectField room in _rooms.Values)
            {
                _roomFieldsPool.Release(room);
                room.OnRoomConnectPressed -= SendRoomConnect;
            }
            _rooms.Clear();
        }

        private IObjectPool<RoomConnectField> CreateRoomConnectPool(IWindowsFactory windowsFactory) =>
            new ObjectPool<RoomConnectField>(() => windowsFactory.CreateWindow<RoomConnectField>(_roomsContent), 
                roomField => roomField.Show(), roomField => roomField.Hide());

        private void RefreshRoomList(List<RoomInfo> roomInfos)
        {
            foreach (string roomName in _rooms.Keys.ToArray())
            {
                if(roomInfos.All(r => r.Name != roomName)) RemoveRoomFromList(roomName);
            }
            
            foreach (RoomInfo roomInfo in roomInfos)
            {
                if (_rooms.TryGetValue(roomInfo.Name, out RoomConnectField roomField))
                    roomField.UpdateRoomData(roomInfo);
                else
                    AddRoomToList(roomInfo);
            }
        }

        private void AddRoomToList(RoomInfo roomInfo)
        {
            RoomConnectField roomConnectField = _rooms.TryGetValue(roomInfo.Name, out RoomConnectField roomField) 
                ? roomField : _roomFieldsPool.Get();
            roomConnectField.UpdateRoomData(roomInfo);
            roomConnectField.OnRoomConnectPressed += SendRoomConnect;
            _rooms.Add(roomInfo.Name, roomConnectField);
        }

        private void RemoveRoomFromList(string roomName)
        {
            if (!_rooms.TryGetValue(roomName, out RoomConnectField roomConnectField)) return;
            roomConnectField.OnRoomConnectPressed -= SendRoomConnect;
            _roomFieldsPool.Release(roomConnectField);
            _rooms.Remove(roomName);
        }

        private void SendRoomConnect(RoomInfo roomInfo) => OnRoomConnect?.Invoke(roomInfo);

        private void OnDestroy()
        {
            _multiplayerRooms.OnRoomsUpdated -= RefreshRoomList;
            Clear();
        }
    }
}