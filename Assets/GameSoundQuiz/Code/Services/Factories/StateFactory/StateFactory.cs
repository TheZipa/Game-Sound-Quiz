using System;
using GameSoundQuiz.Extensions;
using GameSoundQuiz.Infrastructure.StateMachine;
using GameSoundQuiz.Infrastructure.StateMachine.States;
using VContainer;

namespace GameSoundQuiz.Services.Factories.StateFactory
{
    public class StateFactory : IStateFactory
    {
        private readonly IObjectResolver _objectResolver;
        private readonly IGameStateMachine _stateMachine;

        public StateFactory(IObjectResolver objectResolver, IGameStateMachine stateMachine)
        {
            _objectResolver = objectResolver;
            _stateMachine = stateMachine;
        }
        
        public void CreateAllStates()
        {
            foreach (Type stateType in TypeExtensions.GetAllStatesTypes())
                _stateMachine.AddState(stateType, _objectResolver.Resolve(stateType) as IExitableState);
        }
    }
}