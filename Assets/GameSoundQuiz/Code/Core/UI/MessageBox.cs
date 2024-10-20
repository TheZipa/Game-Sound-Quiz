using GameSoundQuiz.Core.UI.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSoundQuiz.Core.UI
{
    public class MessageBox : FadeBaseWindow
    {
        [SerializeField] private Button _okButton;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Image _windowBackground;
        
        [Space(12)] 
        [SerializeField] private Color _infoWindowColor;
        [SerializeField] private Color _errorWindowColor;

        protected override void OnAwake()
        {
            base.OnAwake();
            _okButton.onClick.AddListener(Hide);
        }

        public void ShowInfo(string description) => SetupAndShow(description, _infoWindowColor);

        public void ShowError(string description) => SetupAndShow(description, _errorWindowColor);

        private void SetupAndShow(string description, Color windowColor)
        {
            _windowBackground.color = windowColor;
            _description.text = description;
        }
    }
}