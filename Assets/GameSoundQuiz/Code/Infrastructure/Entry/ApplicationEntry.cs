using GameSoundQuiz.Infrastructure.StateMachine;
using GameSoundQuiz.Infrastructure.StateMachine.States;
using GameSoundQuiz.Services.Factories.StateFactory;
using VContainer.Unity;

namespace GameSoundQuiz.Infrastructure.Entry
{
    public class ApplicationEntry : IStartable
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStateFactory _stateFactory;

        public ApplicationEntry(IGameStateMachine gameStateMachine, IStateFactory stateFactory)
        {
            _gameStateMachine = gameStateMachine;
            _stateFactory = stateFactory;
        }
        
        public void Start()
        {
            _stateFactory.CreateAllStates();
            _gameStateMachine.Enter<LoadApplicationState>(); 
        }
    }
}