using GameSoundQuiz.Core.UI.Base;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameSoundQuiz.Core.UI.MainMenu
{
    public class MainMenuScreen : BaseWindow
    {
        public UnityEvent OnRoomsClick => _roomsButton.onClick;

        [SerializeField] private Button _roomsButton;
    }
}