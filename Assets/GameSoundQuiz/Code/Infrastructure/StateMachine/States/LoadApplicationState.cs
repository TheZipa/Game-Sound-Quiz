using GameSoundQuiz.Core.UI;
using GameSoundQuiz.Core.UI.Settings;
using GameSoundQuiz.Services.LoadingCurtain;
using GameSoundQuiz.Services.Multiplayer;
using GameSoundQuiz.Services.SaveLoad;
using GameSoundQuiz.Services.Sound;
using GameSoundQuiz.Services.StaticData;
using GameSoundQuiz.Services.Windows;
using GameSoundQuiz.Services.WindowsFactory;

namespace GameSoundQuiz.Infrastructure.StateMachine.States
{
    public class LoadApplicationState : IState
    {
        private readonly IApplicationStateMachine _applicationStateMachine;
        private readonly IMultiplayerConnection _multiplayerConnection;
        private readonly IWindowsService _windowsService;
        private readonly IWindowsFactory _windowsFactory;
        private readonly ISoundService _soundService;
        private readonly IStaticData _staticData;
        private readonly ISaveLoad _saveLoad;

        public LoadApplicationState(
            IApplicationStateMachine applicationStateMachine,
            IMultiplayerConnection multiplayerConnection,
            IWindowsService windowsService,
            IWindowsFactory windowsFactory,
            ISoundService soundService,
            IStaticData staticData,
            ISaveLoad saveLoad)
        {
            _applicationStateMachine = applicationStateMachine;
            _multiplayerConnection = multiplayerConnection;
            _windowsService = windowsService;
            _soundService = soundService;
            _staticData = staticData;
            _windowsFactory = windowsFactory;
            _saveLoad = saveLoad;
        }
        
        public void Enter()
        {
            _saveLoad.Load();
            _soundService.Construct(_saveLoad, _staticData.SoundData);
            
            CreatePersistentWindows();

            _multiplayerConnection.OnConnectingSuccess += SwitchToAuthState;
            _multiplayerConnection.Connect();
        }

        public void Exit()
        {
            _multiplayerConnection.OnConnectingSuccess -= SwitchToAuthState;
        }

        private void SwitchToAuthState() => _applicationStateMachine.Enter<AuthState>();

        private void CreatePersistentWindows()
        {
            _windowsFactory.CreatePersistentCanvas();

            SettingsView settingsView = _windowsService.AddPersistentWindow<SettingsView>();
            HeaderView headerView = _windowsService.AddPersistentWindow<HeaderView>();

            headerView.OnSettingsClick += settingsView.Toggle;
        }
    }
}