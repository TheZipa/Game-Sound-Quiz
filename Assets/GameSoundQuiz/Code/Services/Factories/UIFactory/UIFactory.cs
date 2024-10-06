using GameSoundQuiz.Core.UI;
using GameSoundQuiz.Core.UI.Settings;
using GameSoundQuiz.Services.Assets;
using GameSoundQuiz.Services.EntityContainer;
using GameSoundQuiz.Services.SaveLoad;
using GameSoundQuiz.Services.Sound;
using GameSoundQuiz.Services.StaticData;
using UnityEngine;

namespace GameSoundQuiz.Services.Factories.UIFactory
{
    public class UIFactory : BaseFactory.BaseFactory, IUIFactory
    {
        private const string RootCanvasPrefabName = "RootCanvas";
        
        private readonly IStaticData _staticData;
        private readonly ISoundService _soundService;
        private readonly ISaveLoad _saveLoad;

        public UIFactory(
            IAssets assets,
            ISaveLoad saveLoad,
            IStaticData staticData,
            IEntityContainer entityContainer, 
            ISoundService soundService)
        : base(assets, entityContainer)
        {
            _staticData = staticData;
            _soundService = soundService;
            _saveLoad = saveLoad;
        }
        
        public GameObject CreateRootCanvas() => _assets.Instantiate(RootCanvasPrefabName);

        public SettingsPanel CreateSettingsPanel(Transform parent)
        {
            SettingsPanel settingsPanel = InstantiateAsRegistered<SettingsPanel>(parent);
            settingsPanel.Construct(_soundService, _saveLoad.Progress.Settings.IsSoundEnabled);
            return settingsPanel;
        }

        public TopPanel CreateTopPanel(Transform parent)
        {
            TopPanel topPanel = InstantiateAsRegistered<TopPanel>(parent);
            topPanel.Construct(_soundService, _entityContainer.GetEntity<SettingsPanel>());
            return topPanel;
        }
    }
}