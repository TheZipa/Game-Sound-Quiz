using GameSoundQuiz.Data.StaticData;
using GameSoundQuiz.Data.StaticData.Sounds;

namespace GameSoundQuiz.Services.StaticData
{
    public interface IStaticData : IGlobalService
    {
        SoundData SoundData { get; }
        ApplicationConfig ApplicationConfig { get; }
        void LoadStaticData();
    }
}