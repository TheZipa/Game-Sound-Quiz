using UnityEngine;

namespace GameSoundQuiz.Services.Assets
{
    public class AssetProvider : IAssets
    {
        private const string GamePrefabsFolder = "Game Sound Quiz Prefabs/";
        
        public T ProvidePrefab<T>() where T : Object =>
            Resources.Load<T>(GamePrefabsFolder + typeof(T).Name);
        
        public T ProvidePrefab<T>(string prefabName) where T : Object =>
            Resources.Load<T>(GamePrefabsFolder + prefabName);
    }
}