using GameSoundQuiz.Data.StaticData;
using GameSoundQuiz.Data.StaticData.Sounds;
using GameSoundQuiz.Services.StaticData.StaticDataProvider;

namespace GameSoundQuiz.Services.StaticData
{
    public class StaticData : IStaticData
    {
        public SoundData SoundData { get; private set; }
        public ApplicationConfig ApplicationConfig { get; private set; }

        private readonly IStaticDataProvider _staticDataProvider;

        public StaticData(IStaticDataProvider staticDataProvider)
        {
            _staticDataProvider = staticDataProvider;
            LoadStaticData();
        }

        public void LoadStaticData()
        {
            SoundData = _staticDataProvider.LoadSoundData();
            ApplicationConfig = _staticDataProvider.LoadApplicationConfig();
        }
    }
}