using GameSoundQuiz.Services.Assets;
using GameSoundQuiz.Services.EntityContainer;
using GameSoundQuiz.Services.SaveLoad;
using GameSoundQuiz.Services.Sound;
using GameSoundQuiz.Services.StaticData;

namespace GameSoundQuiz.Services.Factories.GameFactory
{
    public class GameFactory : BaseFactory.BaseFactory, IGameFactory
    {
        private readonly ISoundService _soundService;
        private readonly IStaticData _staticData;
        private readonly ISaveLoad _saveLoad;

        public GameFactory(IAssets assets, IEntityContainer entityContainer, ISoundService soundService, 
            IStaticData staticData, ISaveLoad saveLoad) : base(assets, entityContainer)
        {
            _soundService = soundService;
            _staticData = staticData;
            _saveLoad = saveLoad;
        }
    }
}