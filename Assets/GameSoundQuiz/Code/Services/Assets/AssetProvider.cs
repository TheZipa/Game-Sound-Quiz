using UnityEngine;

namespace GameSoundQuiz.Services.Assets
{
    public class AssetProvider : IAssets
    {
        private const string GamePrefabsFolder = "Game Sound Quiz Prefabs/";
        
        public T ProvideAsset<T>() where T : Object =>
            Resources.Load<T>(GamePrefabsFolder + typeof(T).Name);
        
        public GameObject ProvideAsset(string prefabName) =>
            Resources.Load<GameObject>(GamePrefabsFolder + prefabName);

        public GameObject Instantiate(string prefabName)
        {
            GameObject prefab = ProvideAsset(prefabName);
            return Object.Instantiate(prefab);
        }

        public T Instantiate<T>() where T : Object
        {
            T prefab = ProvideAsset<T>();
            return Object.Instantiate(prefab);
        }

        public T Instantiate<T>(Transform parent) where T : Object
        {
            T prefab = ProvideAsset<T>();
            return Object.Instantiate(prefab, parent);
        }

        public T Instantiate<T>(Vector3 at, Vector3 rotation, Transform parent = null) where T : Object
        {
            T prefab = ProvideAsset<T>();
            return Object.Instantiate(prefab, at, Quaternion.Euler(rotation), parent);
        }
        
        public T Instantiate<T>(Vector3 at, Quaternion rotation, Transform parent = null) where T : Object
        {
            T prefab = ProvideAsset<T>();
            return Object.Instantiate(prefab, at, rotation, parent);
        }
    }
}