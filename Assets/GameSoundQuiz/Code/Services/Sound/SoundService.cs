using System.Collections.Generic;
using System.Linq;
using GameSoundQuiz.Data.Enums;
using GameSoundQuiz.Data.StaticData.Sounds;
using GameSoundQuiz.Services.CoroutineRunner;
using GameSoundQuiz.Services.SaveLoad;
using UnityEngine;

namespace GameSoundQuiz.Services.Sound
{
    public class SoundService : MonoBehaviour, ISoundService, ICoroutineRunner
    {
        public bool IsSoundEnabled
        {
            get => _effectsSource.mute;
            set
            {
                AudioListener.volume = value ? 1f : 0f;
                _saveLoad.Progress.Settings.IsSoundEnabled = value;
            }
        }

        public float Volume => _effectsSource.volume;

        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _effectsSource;
        
        private Dictionary<SoundId, AudioClipData> _sounds;
        private ISaveLoad _saveLoad;

        public void Construct(ISaveLoad saveLoad, SoundData soundData)
        {
            _saveLoad = saveLoad;
            _sounds = soundData.AudioEffectClips.ToDictionary(s => s.Id);
            _musicSource.clip = soundData.BackgroundMusic;
            IsSoundEnabled = _saveLoad.Progress.Settings.IsSoundEnabled;
        }

        public void PlayBackgroundMusic() => _musicSource.Play();

        public void PlayEffectSound(SoundId soundId) => _effectsSource.PlayOneShot(_sounds[soundId].Clip);

        public void SetEffectsVolume(float volume) => AudioListener.volume = volume;
    }
}