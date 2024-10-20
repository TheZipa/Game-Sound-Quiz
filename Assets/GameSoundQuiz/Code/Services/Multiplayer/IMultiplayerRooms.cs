using System;
using System.Collections.Generic;
using Photon.Realtime;

namespace GameSoundQuiz.Services.Multiplayer
{
    public interface IMultiplayerRooms
    {
        event Action OnRoomJoined;
        event Action<int, string> OnRoomJoinFailed;
        event Action<Player> OnPlayerRoomJoin;
        event Action<Player> OnPlayerRoomLeft;
        event Action<List<RoomInfo>> OnRoomsUpdated;
        void JoinToRoom(string roomName);
        void CreateAndJoinRoom(string roomName, int maxPlayers, bool isVisible);
        Player[] GetPlayersInRoom();
        bool IsMasterPlayer();
        void LeaveRoom();
        event Action<Player> OnMasterPlayerChanged;
        void SetMasterPlayer(string playerNickname);
    }
}