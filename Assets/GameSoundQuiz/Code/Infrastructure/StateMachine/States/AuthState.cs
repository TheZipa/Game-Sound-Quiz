using System;
using GameSoundQuiz.Core.UI.Auth;
using GameSoundQuiz.Services.LoadingCurtain;
using GameSoundQuiz.Services.Multiplayer;
using GameSoundQuiz.Services.SaveLoad;
using GameSoundQuiz.Services.Windows;

namespace GameSoundQuiz.Infrastructure.StateMachine.States
{
    public class AuthState : IState
    {
        private readonly IApplicationStateMachine _stateMachine;
        private readonly IMultiplayerCommon _multiplayerCommon;
        private readonly IWindowsService _windowsService;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly ISaveLoad _saveLoad;

        private AuthScreen _authScreen;

        public AuthState(IApplicationStateMachine stateMachine, IWindowsService windowsService,
            ILoadingCurtain loadingCurtain, ISaveLoad saveLoad, IMultiplayerCommon multiplayerCommon)
        {
            _multiplayerCommon = multiplayerCommon;
            _windowsService = windowsService;
            _loadingCurtain = loadingCurtain;
            _stateMachine = stateMachine;
            _saveLoad = saveLoad;
        }

        public void Enter()
        {
            bool isAuthComplete = TryAuthFromCache();
            if (isAuthComplete)
            {
                CompleteAuth();
                return;
            }

            StartLogin();
        }

        public void Exit()
        {
            if (_authScreen is not null)
            {
                _authScreen.OnStartButtonClick -= TryAuthFromView;
                _authScreen.Hide();
            }
            
            _loadingCurtain.Show();
        }

        private bool TryAuthFromCache()
        {
            string userName = _saveLoad.Progress.Name;

            if (String.IsNullOrWhiteSpace(userName)) return false;
            
            _multiplayerCommon.SetNickname(userName);
            return true;
        }

        private void StartLogin()
        {
            _authScreen = _windowsService.GetWindow<AuthScreen>();
            _authScreen.OnStartButtonClick += TryAuthFromView;
            _authScreen.Show();

            _loadingCurtain.Hide();
        }

        private void TryAuthFromView(string name)
        {
            if (String.IsNullOrWhiteSpace(name) || name.Length < 3)
            {
                _authScreen.Show();
                return;
            }

            _saveLoad.Progress.Name = name;
            _multiplayerCommon.SetNickname(name);
            
            CompleteAuth();
        }

        private void CompleteAuth() => _stateMachine.Enter<MenuState>();
    }
}