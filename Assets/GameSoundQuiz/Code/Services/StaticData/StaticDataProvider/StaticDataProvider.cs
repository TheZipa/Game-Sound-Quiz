using GameSoundQuiz.Data.StaticData;
using GameSoundQuiz.Data.StaticData.Sounds;
using UnityEngine;

namespace GameSoundQuiz.Services.StaticData.StaticDataProvider
{
    public class StaticDataProvider : IStaticDataProvider
    {
        private const string SoundDataPath = "Static Data/Sound Data";
        private const string ApplicationConfigPath = "Static Data/Application Config";
        
        public SoundData LoadSoundData() => Resources.Load<SoundData>(SoundDataPath);

        public ApplicationConfig LoadApplicationConfig() => Resources.Load<ApplicationConfig>(ApplicationConfigPath);
    }
}