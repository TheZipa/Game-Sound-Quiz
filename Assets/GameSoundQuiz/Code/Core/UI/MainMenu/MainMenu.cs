using GameSoundQuiz.Services.EntityContainer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameSoundQuiz.Core.UI.MainMenu
{
    public class MainMenu : MonoBehaviour, IFactoryEntity
    {
        public UnityEvent OnPlayClick => _playButton.onClick;

        [SerializeField] private Button _playButton;
    }
}