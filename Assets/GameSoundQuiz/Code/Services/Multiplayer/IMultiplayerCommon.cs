using System;
using ExitGames.Client.Photon;

namespace GameSoundQuiz.Services.Multiplayer
{
    public interface IMultiplayerCommon
    {
        event Action<EventData> OnEventReceived;
        void SetNickname(string nickname);
        void SendEvent(byte eventCode);
        int GetCurrentPlayerId();
    }
}