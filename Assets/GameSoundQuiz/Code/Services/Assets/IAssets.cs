using UnityEngine;

namespace GameSoundQuiz.Services.Assets
{
    public interface IAssets : IGlobalService
    {
        T ProvideAsset<T>() where T : Object;
        T Instantiate<T>() where T : Object;
        T Instantiate<T>(Transform parent) where T : Object;
        T Instantiate<T>(Vector3 at, Vector3 rotation, Transform parent = null) where T : Object;
        T Instantiate<T>(Vector3 at, Quaternion rotation, Transform parent = null) where T : Object;
        GameObject ProvideAsset(string prefabName);
        GameObject Instantiate(string prefabName);
    }
}