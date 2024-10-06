using System;

namespace GameSoundQuiz.Data.Progress
{
    [Serializable]
    public class UserProgress : IPropertyChanged
    {
        public event Action OnPropertyChanged;
        
        public SettingsData Settings { get; set; }

        public int Balance 
        { 
            get => _balance;
            set { _balance = value; OnPropertyChanged?.Invoke(); }
        }

        private int _balance;

        public UserProgress()
        {
            Settings = new SettingsData();
        }
        
        public void Prepare() => Settings.OnPropertyChanged += SendPropertyChanged;

        private void SendPropertyChanged() => OnPropertyChanged?.Invoke();
    }
}