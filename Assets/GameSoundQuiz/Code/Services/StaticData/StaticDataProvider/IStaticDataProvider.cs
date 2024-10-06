using GameSoundQuiz.Data.StaticData;
using GameSoundQuiz.Data.StaticData.Sounds;

namespace GameSoundQuiz.Services.StaticData.StaticDataProvider
{
    public interface IStaticDataProvider : IGlobalService
    {
        SoundData LoadSoundData();
        ApplicationConfig LoadApplicationConfig();
    }
}