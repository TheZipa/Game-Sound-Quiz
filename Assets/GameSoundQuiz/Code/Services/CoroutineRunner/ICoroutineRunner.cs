using System.Collections;
using UnityEngine;

namespace GameSoundQuiz.Services.CoroutineRunner
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator routine);
    }
}