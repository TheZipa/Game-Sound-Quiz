using System.Threading.Tasks;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

using GameSoundQuiz.Core.UI;
using GameSoundQuiz.Core.UI.Rooms;
using GameSoundQuiz.Services.LoadingCurtain;
using GameSoundQuiz.Services.Multiplayer;
using GameSoundQuiz.Services.StaticData;
using GameSoundQuiz.Services.Windows;

namespace GameSoundQuiz.Infrastructure.StateMachine.States
{
    public class RoomState : IState
    {
        private const byte StartGameEventCode = 1;
        
        private readonly IApplicationStateMachine _stateMachine;
        private readonly IMultiplayerRooms _multiplayerRooms;
        private readonly IMultiplayerCommon _multiplayerCommon;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IWindowsService _windowsService;
        private readonly IStaticData _staticData;

        private RoomListScreen _roomListScreen;
        private RoomScreen _roomScreen;
        private RoomCreateScreen _roomCreateScreen;

        public RoomState(
            IApplicationStateMachine stateMachine,
            IMultiplayerRooms multiplayerRooms,
            IMultiplayerCommon multiplayerCommon,
            ILoadingCurtain loadingCurtain,
            IWindowsService windowsService,
            IStaticData staticData)
        {
            _stateMachine = stateMachine;
            _multiplayerRooms = multiplayerRooms;
            _multiplayerCommon = multiplayerCommon;
            _staticData = staticData;
            _loadingCurtain = loadingCurtain;
            _windowsService = windowsService;
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
            _roomListScreen = _windowsService.GetWindow<RoomListScreen>();
            _roomScreen = _windowsService.GetWindow<RoomScreen>();
            _roomCreateScreen = _windowsService.GetWindow<RoomCreateScreen>();
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
            _windowsService.GetWindow<MessageBox>().ShowError(message);
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

        private void ReturnToMainMenu()
        {
            _loadingCurtain.Show();
            _stateMachine.Enter<MenuState>();
        }
    }
}