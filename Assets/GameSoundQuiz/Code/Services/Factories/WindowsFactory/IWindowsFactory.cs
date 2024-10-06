using GameSoundQuiz.Core.UI.Base;
using UnityEngine;

namespace GameSoundQuiz.Services.WindowsFactory
{
    public interface IWindowsFactory : IGlobalService
    {
        GameObject CreateCanvas();
        void CreatePersistentCanvas();
        TWindow CreatePersistentWindow<TWindow>() where TWindow : BaseWindow;
        TWindow CreateSceneWindow<TWindow>() where TWindow : BaseWindow;
    }
}