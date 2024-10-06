using GameSoundQuiz.Services.LoadingCurtain;

namespace GameSoundQuiz.Infrastructure.StateMachine.States
{
    public class GameplayState : IState
    {
        private readonly IApplicationStateMachine _applicationStateMachine;
        private readonly ILoadingCurtain _loadingCurtain;

        public GameplayState(IApplicationStateMachine applicationStateMachine, ILoadingCurtain loadingCurtain)
        {
            _applicationStateMachine = applicationStateMachine;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter()
        {
            _loadingCurtain.Hide();
        }

        public void Exit()
        {
            
        }
    }
}