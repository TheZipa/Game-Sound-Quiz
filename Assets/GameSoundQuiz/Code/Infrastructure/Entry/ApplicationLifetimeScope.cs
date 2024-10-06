using System;
using GameSoundQuiz.Services.LoadingCurtain;
using GameSoundQuiz.Services.Multiplayer;
using GameSoundQuiz.Services.Sound;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using TypeExtensions = GameSoundQuiz.Extensions.TypeExtensions;

namespace GameSoundQuiz.Infrastructure.Entry
{
    public class ApplicationLifetimeScope : LifetimeScope
    {
        [SerializeField] private SoundService _soundService;
        [SerializeField] private LoadingCurtain _loadingCurtain;
        [SerializeField] private MultiplayerService _multiplayerService;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterEntryPoint(builder);
            RegisterInstanceServices(builder);
            RegisterServices(builder);
            RegisterStates(builder);
        }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }

        private static void RegisterEntryPoint(IContainerBuilder builder) => builder.RegisterEntryPoint<ApplicationEntry>();

        private void RegisterInstanceServices(IContainerBuilder builder)
        {
            builder.RegisterComponent(_soundService).AsImplementedInterfaces();
            builder.RegisterComponent(_loadingCurtain).AsImplementedInterfaces();
            builder.RegisterComponent(_multiplayerService).AsImplementedInterfaces();
        }

        private static void RegisterServices(IContainerBuilder builder)
        {
            foreach ((Type serviceImplementation, Type serviceInterface) in TypeExtensions.GetAllGlobalServiceTypes())
                builder.Register(serviceImplementation, Lifetime.Singleton).As(serviceInterface);
        }

        private static void RegisterStates(IContainerBuilder builder)
        {
            foreach (Type stateType in TypeExtensions.GetAllStatesTypes())
                builder.Register(stateType, Lifetime.Singleton);
        }
    }
}