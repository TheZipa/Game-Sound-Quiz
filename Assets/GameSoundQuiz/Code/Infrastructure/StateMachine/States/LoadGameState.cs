using GameSoundQuiz.Core.UI;
using GameSoundQuiz.Services.EntityContainer;
using GameSoundQuiz.Services.Factories.GameFactory;
using GameSoundQuiz.Services.Factories.UIFactory;
using GameSoundQuiz.Services.LoadingCurtain;
using GameSoundQuiz.Services.SceneLoader;
using UnityEngine;

namespace GameSoundQuiz.Infrastructure.StateMachine.States
{
    public class LoadGameState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly IGameFactory _gameFactory;
        private readonly IEntityContainer _entityContainer;
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingCurtain _loadingCurtain;
        private const string GameScene = "Game";

        public LoadGameState(IGameStateMachine gameStateMachine, IUIFactory uiFactory, IGameFactory gameFactory,
            IEntityContainer entityContainer, ISceneLoader sceneLoader, ILoadingCurtain loadingCurtain)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            _gameFactory = gameFactory;
            _entityContainer = entityContainer;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
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
            GameObject rootCanvas = _uiFactory.CreateRootCanvas();
            _entityContainer.GetEntity<TopPanel>().ToggleGameplayStateView();
        }

        private void InitializeGameplay()
        {
            
        }

        private void FinishLoad() => _gameStateMachine.Enter<GameplayState>();
    }
}