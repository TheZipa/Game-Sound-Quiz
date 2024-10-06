using GameSoundQuiz.Core.UI;
using GameSoundQuiz.Core.UI.MainMenu;
using GameSoundQuiz.Services.EntityContainer;
using GameSoundQuiz.Services.Factories.UIFactory;
using GameSoundQuiz.Services.LoadingCurtain;
using GameSoundQuiz.Services.SceneLoader;
using UnityEngine;

namespace GameSoundQuiz.Infrastructure.StateMachine.States
{
    public class MenuState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IUIFactory _uiFactory;
        private readonly ISceneLoader _sceneLoader;
        private readonly IEntityContainer _entityContainer;
        private readonly ILoadingCurtain _loadingCurtain;

        private MainMenu _mainMenu;
        private TopPanel _topPanel;
        private const string MenuScene = "Menu";

        public MenuState(IGameStateMachine stateMachine, IUIFactory uiFactory,
            ISceneLoader sceneLoader, IEntityContainer entityContainer, ILoadingCurtain loadingCurtain)
        {
            _stateMachine = stateMachine;
            _uiFactory = uiFactory;
            _sceneLoader = sceneLoader;
            _entityContainer = entityContainer;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter() => _sceneLoader.LoadScene(MenuScene, PrepareMenu);

        public void Exit()
        {
            _mainMenu.OnPlayClick.RemoveListener(LoadGame);
        }

        private void PrepareMenu()
        {
            CreateMenuElements();
            Subscribe();
            _loadingCurtain.Hide();
        }

        private void CreateMenuElements()
        {
            GameObject rootCanvas = _uiFactory.CreateRootCanvas();
            _topPanel = _entityContainer.GetEntity<TopPanel>();
            _topPanel.ToggleMainMenuStateView();
            _mainMenu = _uiFactory.InstantiateAsRegistered<MainMenu>(rootCanvas.transform);
        }

        private void Subscribe()
        {
            _mainMenu.OnPlayClick.AddListener(LoadGame);
        }

        private void LoadGame() => _stateMachine.Enter<LoadGameState>();
    }
}