using GameSoundQuiz.Core.UI;
using GameSoundQuiz.Core.UI.Settings;
using GameSoundQuiz.Services.Factories.BaseFactory;
using UnityEngine;

namespace GameSoundQuiz.Services.Factories.UIFactory
{
    public interface IUIFactory : IBaseFactory, IGlobalService
    {
        GameObject CreateRootCanvas();
        SettingsPanel CreateSettingsPanel(Transform parent);
        TopPanel CreateTopPanel(Transform parent);
    }
}