using System;
using System.Collections.Generic;
using System.Linq;
using GameSoundQuiz.Core.UI.Base;
using GameSoundQuiz.Services.Multiplayer;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace GameSoundQuiz.Core.UI.Rooms
{
    public class RoomListScreen : FadeBaseWindow
    {
        public event Action OnRoomListClose;
        public event Action<RoomInfo> OnRoomConnect;
        public event Action OnRoomCreateClick;
        
        public Transform RoomsContent;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _createRoomButton;

        private readonly Dictionary<string, RoomConnectField> _rooms = new(10);
        private IMultiplayerRooms _multiplayerRooms;
        private IObjectPool<RoomConnectField> _roomFieldsPool;
        private Coroutine _refreshRoomListRoutine;

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

        public void Construct(IMultiplayerRooms multiplayerRooms, IObjectPool<RoomConnectField> roomFieldsPool)
        {
            _roomFieldsPool = roomFieldsPool;
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