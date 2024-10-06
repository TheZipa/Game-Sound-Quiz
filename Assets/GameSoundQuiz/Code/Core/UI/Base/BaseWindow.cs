using UnityEngine;

namespace GameSoundQuiz.Core.UI.Base
{
    public abstract class BaseWindow : MonoBehaviour
    {
        public virtual void Show() => gameObject.SetActive(true);
        
        public virtual void Hide() => gameObject.SetActive(false);
    }
}