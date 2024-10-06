using System;
using UnityEngine;

namespace GameSoundQuiz.Core.UI.Base
{
    public abstract class BaseWindow : MonoBehaviour
    {
        public event Action<BaseWindow> OnShow;
        public event Action<BaseWindow> OnHide;

        public bool IsPersistent { get; set; }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            OnShow?.Invoke(this);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            OnHide?.Invoke(this);
        }
    }
}