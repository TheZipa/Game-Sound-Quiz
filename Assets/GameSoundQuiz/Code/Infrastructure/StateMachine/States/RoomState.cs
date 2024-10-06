using System.Threading.Tasks;

using GameSoundQuiz.Core.UI.Rooms;
using GameSoundQuiz.Services.LoadingCurtain;
using GameSoundQuiz.Services.Multiplayer;
using GameSoundQuiz.Services.StaticData;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace GameSoundQuiz.Infrastructure.StateMachine.States
{
    public class RoomState : IState
    {
        private const byte StartGameEventCode = 1;
        
        private readonly IApplicationStateMachine _stateMachine;
        private readonly IMultiplayerRooms _multiplayerRooms;
        private readonly IMultiplayerCommon _multiplayerCommon;
        private readonly IStaticData _staticData;
        private readonly ILoadingCurtain _loadingCurtain;

        private RoomListScreen _roomListScreen;
        private RoomScreen _roomScreen;
        private RoomCreateScreen _roomCreateScreen;

        public RoomState(
            IApplicationStateMachine stateMachine,
            IMultiplayerRooms multiplayerRooms,
            IMultiplayerCommon multiplayerCommon,
            ILoadingCurtain loadingCurtain,
            IStaticData staticData)
        {
            _stateMachine = stateMachine;
            _multiplayerRooms = multiplayerRooms;
            _multiplayerCommon = multiplayerCommon;
            _staticData = staticData;
            _loadingCurtain = loadingCurtain;
        }
        
        public void Enter()
        {
            CacheEntities();
            Subscribe();
            _roomListScreen.Show();
        }

        public void Exit()
        {
            Unsubscribe();
            _roomListScreen.Hide();
        }

        private void CacheEntities()
        {
            //_roomListScreen = _entityContainer.GetEntity<RoomListScreen>();
            //_roomScreen = _entityContainer.GetEntity<RoomScreen>();
            //_roomCreateScreen = _entityContainer.GetEntity<RoomCreateScreen>();
        }

        private void Subscribe()
        {
            _multiplayerRooms.OnRoomJoinFailed += DisplayRoomJoinFail;
            _multiplayerRooms.OnRoomJoined += SwitchToRoomScreen;
            _multiplayerCommon.OnEventReceived += HandleStartGameEvent;
            _roomScreen.OnRoomLeft += _multiplayerRooms.LeaveRoom;
            _roomScreen.OnStartGame += StartGame;
            _roomListScreen.OnRoomConnect += JoinToRoom;
            _roomListScreen.OnRoomCreateClick += _roomCreateScreen.Show;
            _roomListScreen.OnRoomListClose += ReturnToMainMenu;
            _roomCreateScreen.OnRoomCreated += CreateNewRoom;
        }
        
        private void Unsubscribe()
        {
            _multiplayerRooms.OnRoomJoinFailed -= DisplayRoomJoinFail;
            _multiplayerRooms.OnRoomJoined -= SwitchToRoomScreen;
            _multiplayerCommon.OnEventReceived -= HandleStartGameEvent;
            _roomScreen.OnRoomLeft -= _multiplayerRooms.LeaveRoom;;
            _roomScreen.OnStartGame -= StartGame;
            _roomListScreen.OnRoomConnect -= JoinToRoom;
            _roomListScreen.OnRoomCreateClick -= _roomCreateScreen.Show;
            _roomListScreen.OnRoomListClose -= ReturnToMainMenu;
            _roomCreateScreen.OnRoomCreated -= CreateNewRoom;
        }

        private async void DisplayRoomJoinFail(string message)
        {
            await Task.Delay(1000);
            _loadingCurtain.Hide();
            //_entityContainer.GetEntity<ErrorScreen>().ShowError(message);
        }

        private void SwitchToRoomScreen()
        {
            _roomCreateScreen.Hide();
            _roomScreen.SetupRoom();
            _roomScreen.Show();
            _loadingCurtain.Hide();
        }

        private void CreateNewRoom(string roomName)
        {
            _loadingCurtain.Show();
            _multiplayerRooms.CreateAndJoinRoom(roomName, _staticData.ApplicationConfig.MaxPlayersInLobby, true);
        }

        private void StartGame()
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
            _multiplayerCommon.SendEvent(StartGameEventCode);
            _stateMachine.Enter<LoadGameState>();
        }

        private void HandleStartGameEvent(EventData eventData)
        {
            if (eventData.Code != StartGameEventCode) return;
            _stateMachine.Enter<LoadGameState>();
        }

        private void JoinToRoom(RoomInfo roomInfo)
        {
            _multiplayerRooms.JoinToRoom(roomInfo.Name);
            _loadingCurtain.Show();
        }

        private void ReturnToMainMenu() => _stateMachine.Enter<MenuState>();
    }
}