using GameSoundQuiz.Infrastructure.StateMachine;
using GameSoundQuiz.Infrastructure.StateMachine.States;
using GameSoundQuiz.Services.StateFactory;
using VContainer.Unity;

namespace GameSoundQuiz.Infrastructure.Entry
{
    public class ApplicationEntry : IStartable
    {
        private readonly IApplicationStateMachine _applicationStateMachine;
        private readonly IStateFactory _stateFactory;

        public ApplicationEntry(IApplicationStateMachine applicationStateMachine, IStateFactory stateFactory)
        {
            _applicationStateMachine = applicationStateMachine;
            _stateFactory = stateFactory;
        }
        
        public void Start()
        {
            _stateFactory.CreateAllStates();
            _applicationStateMachine.Enter<LoadApplicationState>(); 
        }
    }
}