using GameSoundQuiz.Data.Progress;
using GameSoundQuiz.Extensions;
using UnityEngine;

namespace GameSoundQuiz.Services.SaveLoad
{
    public class SaveLoad : ISaveLoad
    {
        public UserProgress Progress { get; private set; }
        private const string ProgressKey = "Progress";

        public void Load()
        {
            string progressJson = PlayerPrefs.GetString(ProgressKey);
            Progress = progressJson.ToDeserialized<UserProgress>() ?? new UserProgress();
            Progress.Prepare();
            Progress.OnPropertyChanged += SaveProgress;
        }

        private void SaveProgress() => PlayerPrefs.SetString(ProgressKey, Progress.ToJson());
    }
}