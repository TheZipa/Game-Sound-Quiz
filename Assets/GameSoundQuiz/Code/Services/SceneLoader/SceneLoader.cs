using System;
using GameSoundQuiz.Services.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSoundQuiz.Services.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        private readonly IWindowsService _windowsService;

        public SceneLoader(IWindowsService windowsService)
        {
            _windowsService = windowsService;
        }
        
        public void LoadScene(string sceneName, Action onLoaded = null)
        {
            _windowsService.Clear();
            
            AsyncOperation loadSceneAsyncOperation = SceneManager.LoadSceneAsync(sceneName);
            loadSceneAsyncOperation.completed += operation => onLoaded?.Invoke();
        }
    }
}