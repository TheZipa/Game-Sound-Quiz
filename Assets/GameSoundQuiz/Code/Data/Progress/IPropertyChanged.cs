using System;

namespace GameSoundQuiz.Data.Progress
{
    public interface IPropertyChanged
    {
        event Action OnPropertyChanged;
    }
}