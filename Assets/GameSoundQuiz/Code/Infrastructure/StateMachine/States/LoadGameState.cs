using GameSoundQuiz.Services.LoadingCurtain;
using GameSoundQuiz.Services.SceneLoader;
using GameSoundQuiz.Services.WindowsFactory;
using UnityEngine;

namespace GameSoundQuiz.Infrastructure.StateMachine.States
{
    public class LoadGameState : IState
    {
        private readonly IApplicationStateMachine _applicationStateMachine;
        private readonly IWindowsFactory _windowsFactory;
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingCurtain _loadingCurtain;
        private const string GameScene = "Game";

        public LoadGameState(
            IApplicationStateMachine applicationStateMachine,
            ILoadingCurtain loadingCurtain,
            ISceneLoader sceneLoader,
            IWindowsFactory windowsFactory)
        {
            _applicationStateMachine = applicationStateMachine;
            _loadingCurtain = loadingCurtain;
            _sceneLoader = sceneLoader;
            _windowsFactory = windowsFactory;
        }

        public void Enter()
        {
            _loadingCurtain.Show();
            _sceneLoader.LoadScene(GameScene, CreateGame);
        }

        public void Exit()
        {
        }

        private void CreateGame()
        {
            InitializeUI();
            InitializeGameplay();
            FinishLoad();
        }

        private void InitializeUI()
        {
            GameObject rootCanvas = _windowsFactory.CreateCanvas();
        }

        private void InitializeGameplay()
        {
            
        }

        private void FinishLoad() => _applicationStateMachine.Enter<GameplayState>();
    }
}