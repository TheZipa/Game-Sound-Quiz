using GameSoundQuiz.Core.UI.Base;

namespace GameSoundQuiz.Services.Windows
{
    public interface IWindowsService : IGlobalService
    {
        TWindow AddPersistentWindow<TWindow>() where TWindow : BaseWindow;
        TWindow GetWindow<TWindow>() where TWindow : BaseWindow;
        void Clear();
    }
}