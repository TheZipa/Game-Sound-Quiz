using GameSoundQuiz.Data.Progress;

namespace GameSoundQuiz.Services.SaveLoad
{
    public interface ISaveLoad : IGlobalService
    {
        UserProgress Progress { get; }
        void Load();
    }
}