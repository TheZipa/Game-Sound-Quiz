using GameSoundQuiz.Data.Enums;
using GameSoundQuiz.Data.StaticData.Sounds;
using GameSoundQuiz.Services.SaveLoad;

namespace GameSoundQuiz.Services.Sound
{
    public interface ISoundService
    {
        bool IsSoundEnabled { get; set; }
        float Volume { get; }
        void Construct(ISaveLoad saveLoad, SoundData soundData);
        void PlayBackgroundMusic();
        void PlayEffectSound(SoundId soundId);
        void SetEffectsVolume(float volume);
    }
}