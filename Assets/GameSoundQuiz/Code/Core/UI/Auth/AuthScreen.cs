using System;
using System.Collections;
using GameSoundQuiz.Core.UI.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSoundQuiz.Core.UI.Auth
{
    public class AuthScreen : BaseWindow
    {
        public event Action<string> OnStartButtonClick;
        
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private CanvasGroup _wrongNameHint;
        [SerializeField] private Button _startButton;
        
        [Space(12)]
        [SerializeField] private float _wrongNameHintTime;
        [SerializeField] private float _wrongNameDisappearSpeed;

        private Coroutine _wrongNameCoroutine;

        private void Awake()
        {
            _startButton.onClick.AddListener(() => OnStartButtonClick?.Invoke(_nameInputField.text));
        }

        public void ShowWrongName()
        {
            if(_wrongNameCoroutine is not null) StopCoroutine(_wrongNameCoroutine);
            _wrongNameCoroutine = StartCoroutine(ShowWrongNameRoutine());
        }

        private IEnumerator ShowWrongNameRoutine()
        {
            _wrongNameHint.alpha = 1;

            yield return new WaitForSeconds(_wrongNameHintTime);

            while (_wrongNameHint.alpha > 0)
            {
                _wrongNameHint.alpha -= _wrongNameDisappearSpeed * Time.deltaTime;
                yield return null;
            }

            _wrongNameHint.alpha = 0;
            _wrongNameCoroutine = null;
        }
    }
}