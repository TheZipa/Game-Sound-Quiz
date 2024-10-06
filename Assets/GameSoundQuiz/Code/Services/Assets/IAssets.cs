using UnityEngine;

namespace GameSoundQuiz.Services.Assets
{
    public interface IAssets : IGlobalService
    {
        T ProvidePrefab<T>() where T : Object;
        T ProvidePrefab<T>(string prefabName) where T : Object;
    }
}