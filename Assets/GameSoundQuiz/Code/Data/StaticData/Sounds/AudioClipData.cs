using System;
using GameSoundQuiz.Data.Enums;
using UnityEngine;

namespace GameSoundQuiz.Data.StaticData.Sounds
{
    [Serializable]
    public class AudioClipData
    {
        public AudioClip Clip;
        public SoundId Id;
    }
}