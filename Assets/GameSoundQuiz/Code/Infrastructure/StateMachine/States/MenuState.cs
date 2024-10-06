using GameSoundQuiz.Core.UI;
using GameSoundQuiz.Core.UI.MainMenu;
using GameSoundQuiz.Services.LoadingCurtain;
using GameSoundQuiz.Services.SceneLoader;
using GameSoundQuiz.Services.Windows;

namespace GameSoundQuiz.Infrastructure.StateMachine.States
{
    public class MenuState : IState
    {
        private const string MenuScene = "Menu";
        
        private readonly IApplicationStateMachine _stateMachine;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IWindowsService _windowsService;
        private readonly ISceneLoader _sceneLoader;

        private MainMenuScreen _mainMenuScreen;
        private HeaderView _headerView;

        public MenuState(
            IApplicationStateMachine stateMachine,
            ILoadingCurtain loadingCurtain,
            IWindowsService windowsService,
            ISceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _loadingCurtain = loadingCurtain;
            _windowsService = windowsService;
            _sceneLoader = sceneLoader;
        }

        public void Enter() => _sceneLoader.LoadScene(MenuScene, EnableMenu);

        private void EnableMenu()
        {
            GetViews();
            Subscribe();
            
            _headerView.Show();
            _headerView.ToggleBackButton(false);

            _loadingCurtain.Hide();
        }

        public void Exit()
        {
            _mainMenuScreen.OnRoomsClick.RemoveListener(SwitchToRoomList);
        }

        private void Subscribe()
        {
            _mainMenuScreen.OnRoomsClick.AddListener(SwitchToRoomList);
        }

        private void GetViews()
        {
            _mainMenuScreen = _windowsService.GetWindow<MainMenuScreen>();
            _headerView = _windowsService.GetWindow<HeaderView>();
        }

        private void SwitchToRoomList() => _stateMachine.Enter<RoomState>();
    }
}