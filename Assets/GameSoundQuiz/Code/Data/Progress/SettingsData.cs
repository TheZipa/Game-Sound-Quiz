using System;

namespace GameSoundQuiz.Data.Progress
{
    [Serializable]
    public class SettingsData : IPropertyChanged
    {
        public event Action OnPropertyChanged;

        public bool IsSoundEnabled
        {
            get => _isSoundEnabled;
            set { _isSoundEnabled = value; OnPropertyChanged?.Invoke(); }
        }
        
        private bool _isSoundEnabled;


        public SettingsData() => IsSoundEnabled = true;

    }
}