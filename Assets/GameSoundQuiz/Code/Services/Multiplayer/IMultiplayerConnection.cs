using System;
using Photon.Realtime;

namespace GameSoundQuiz.Services.Multiplayer
{
    public interface IMultiplayerConnection
    {
        event Action OnConnectingSuccess;
        void Connect();
        event Action<DisconnectCause> OnConnectionClosed;
    }
}