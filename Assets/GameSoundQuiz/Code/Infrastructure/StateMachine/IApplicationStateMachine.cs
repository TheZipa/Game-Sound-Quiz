using System;
using GameSoundQuiz.Infrastructure.StateMachine.States;
using GameSoundQuiz.Services;

namespace GameSoundQuiz.Infrastructure.StateMachine
{
    public interface IApplicationStateMachine : IGlobalService
    {
        void Enter<TState>() where TState : class, IState;
        void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>;
        void AddState<TState>(TState instance) where TState : class, IState;
        void AddState<TState, TPayload>(TState instance) where TState : class, IPayloadedState<TPayload>;
        void AddState(Type type, IExitableState instance);
    }
}