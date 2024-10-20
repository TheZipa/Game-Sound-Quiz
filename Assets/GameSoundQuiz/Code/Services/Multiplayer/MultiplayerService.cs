using System;
using System.Collections.Generic;
using System.Linq;

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace GameSoundQuiz.Services.Multiplayer
{
    public class MultiplayerService : MonoBehaviourPunCallbacks, IMultiplayerCommon, IMultiplayerConnection, IMultiplayerRooms
    {
        public event Action<EventData> OnEventReceived;
        public event Action OnRoomJoined;
        public event Action OnConnectingSuccess;
        
        public event Action<int, string> OnRoomJoinFailed;
        public event Action<Player> OnPlayerRoomJoin;
        public event Action<Player> OnPlayerRoomLeft;
        public event Action<Player> OnMasterPlayerChanged;
        public event Action<DisconnectCause> OnConnectionClosed;
        public event Action<List<RoomInfo>> OnRoomsUpdated;
        
        private readonly RaiseEventOptions _eventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        private readonly TypedLobby _mainLobby = new TypedLobby("mainLobby", LobbyType.Default);

        private void Awake() => PhotonNetwork.NetworkingClient
            .EventReceived += eventData => OnEventReceived?.Invoke(eventData);

        public void Connect()
        {
            PhotonNetwork.GameVersion = Application.version;
            PhotonNetwork.ConnectUsingSettings();
        }

        public void SetNickname(string nickname) => PhotonNetwork.NickName = nickname;

        public void SetMasterPlayer(string playerNickname)
        {
            Player[] playersInRoom = GetPlayersInRoom();
            if (playersInRoom.Length is 0 || String.IsNullOrWhiteSpace(playerNickname)) return;

            Player player = playersInRoom.First(v => v.NickName == playerNickname);
            PhotonNetwork.SetMasterClient(player);
        }

        public void JoinToRoom(string roomName)
        {
            if (!PhotonNetwork.IsConnected) OnRoomJoinFailed?.Invoke(-1, "You are not connected");
            else if (PhotonNetwork.NetworkClientState != ClientState.JoinedLobby) OnRoomJoinFailed?.Invoke(-1, "Wrong client state");
            else PhotonNetwork.JoinRoom(roomName);
        }

        public void CreateAndJoinRoom(string roomName, int maxPlayers, bool isVisible)
        {
            RoomOptions roomOptions = new()
            {
                IsVisible = isVisible,
                MaxPlayers = maxPlayers
            };
            PhotonNetwork.CreateRoom(roomName, roomOptions, _mainLobby);
        }

        public Player[] GetPlayersInRoom() => PhotonNetwork.CurrentRoom.Players.Values.ToArray();

        public bool IsMasterPlayer() => PhotonNetwork.IsMasterClient;

        public int GetCurrentPlayerId() => PhotonNetwork.LocalPlayer.ActorNumber;

        public void LeaveRoom() => PhotonNetwork.LeaveRoom();

        public void SendEvent(byte eventCode) => PhotonNetwork
            .RaiseEvent(eventCode, null, _eventOptions, SendOptions.SendReliable);

        public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby(_mainLobby);

        public override void OnJoinedLobby() => OnConnectingSuccess?.Invoke();

        public override void OnDisconnected(DisconnectCause cause) => OnConnectionClosed?.Invoke(cause);

        public override void OnJoinedRoom() => OnRoomJoined?.Invoke();

        public override void OnPlayerEnteredRoom(Player newPlayer) => OnPlayerRoomJoin?.Invoke(newPlayer);

        public override void OnMasterClientSwitched(Player newMasterClient) => OnMasterPlayerChanged?.Invoke(newMasterClient);

        public override void OnPlayerLeftRoom(Player otherPlayer) => OnPlayerRoomLeft?.Invoke(otherPlayer);

        public override void OnJoinRoomFailed(short returnCode, string message) => OnRoomJoinFailed?.Invoke(returnCode, message);

        public override void OnRoomListUpdate(List<RoomInfo> roomList) => OnRoomsUpdated?.Invoke(roomList);
    }
}