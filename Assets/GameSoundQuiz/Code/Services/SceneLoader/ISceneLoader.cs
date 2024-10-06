using System;

namespace GameSoundQuiz.Services.SceneLoader
{
    public interface ISceneLoader : IGlobalService
    {
        void LoadScene(string sceneName, Action onLoaded = null);
    }
}