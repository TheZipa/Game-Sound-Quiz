using GameSoundQuiz.Core.UI;
using GameSoundQuiz.Services.Factories.GameFactory;
using GameSoundQuiz.Services.Factories.UIFactory;
using GameSoundQuiz.Services.LoadingCurtain;
using GameSoundQuiz.Services.SaveLoad;
using GameSoundQuiz.Services.Sound;
using GameSoundQuiz.Services.StaticData;
using UnityEngine;

namespace GameSoundQuiz.Infrastructure.StateMachine.States
{
    public class LoadApplicationState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticData _staticData;
        private readonly ISaveLoad _saveLoad;
        private readonly IGameFactory _gameFactory;
        private readonly IUIFactory _uiFactory;
        private readonly ISoundService _soundService;
        private readonly ILoadingCurtain _loadingCurtain;

        public LoadApplicationState(IGameStateMachine gameStateMachine, IStaticData staticData, ISaveLoad saveLoad,
            IGameFactory gameFactory, IUIFactory uiFactory, ISoundService soundService, ILoadingCurtain loadingCurtain)
        {
            _staticData = staticData;
            _saveLoad = saveLoad;
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
            _soundService = soundService;
            _loadingCurtain = loadingCurtain;
            _gameStateMachine = gameStateMachine;
        }
        
        public void Enter()
        {
            _saveLoad.Load();
            _soundService.Construct(_saveLoad, _staticData.SoundData);
            CreatePersistentEntities();
            _gameStateMachine.Enter<MenuState>();
        }

        public void Exit()
        {
        }

        private void CreatePersistentEntities()
        {
            GameObject persistentCanvas = CreatePersistentCanvas();
            _uiFactory.CreateSettingsPanel(persistentCanvas.transform);
            TopPanel topPanel = _uiFactory.CreateTopPanel(persistentCanvas.transform);
            topPanel.SetBackAction(() =>
            {
                _loadingCurtain.Show();
                _gameStateMachine.Enter<MenuState>();
            });
        }

        private GameObject CreatePersistentCanvas()
        {
            GameObject persistentCanvas = _uiFactory.CreateRootCanvas();
            persistentCanvas.GetComponent<Canvas>().sortingOrder = 10;
            persistentCanvas.name = "PersistentCanvas";
            Object.DontDestroyOnLoad(persistentCanvas);
            return persistentCanvas;
        }
    }
}