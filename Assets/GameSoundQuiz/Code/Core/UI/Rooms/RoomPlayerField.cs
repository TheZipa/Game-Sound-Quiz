using GameSoundQuiz.Core.UI.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSoundQuiz.Core.UI.Rooms
{
    public class RoomPlayerField : BaseWindow
    {
        [SerializeField] private Image _personIcon;
        [SerializeField] private TextMeshProUGUI _playerName;

        public void ShowAndSetup(string playerName, bool isMaster)
        {
            _playerName.text = playerName;
            _personIcon.gameObject.SetActive(isMaster);
            Show();
        } 
    }
}