namespace GameSoundQuiz.Services.StateFactory
{
    public interface IStateFactory : IGlobalService
    {
        void CreateAllStates();
    }
}