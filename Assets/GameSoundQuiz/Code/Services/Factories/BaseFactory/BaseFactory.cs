using GameSoundQuiz.Services.Assets;
using GameSoundQuiz.Services.EntityContainer;
using UnityEngine;

namespace GameSoundQuiz.Services.Factories.BaseFactory
{
    public abstract class BaseFactory : IBaseFactory
    {
        protected readonly IAssets _assets;
        protected readonly IEntityContainer _entityContainer;

        protected BaseFactory(IAssets assets, IEntityContainer entityContainer)
        {
            _assets = assets;
            _entityContainer = entityContainer;
        }

        public T InstantiateAsRegistered<T>(Vector3 at, Quaternion rotation, Transform parent = null) where T : Object, IFactoryEntity
        {
            T gameObject = _assets.Instantiate<T>(at, rotation, parent);
            _entityContainer.RegisterEntity(gameObject);
            return gameObject;
        }
        
        public T InstantiateAsRegistered<T>(Transform parent = null) where T : Object, IFactoryEntity
        {
            T gameObject = _assets.Instantiate<T>(parent);
            _entityContainer.RegisterEntity(gameObject);
            return gameObject;
        }

        public T Instantiate<T>(Vector3 at, Vector3 rotation, Transform parent = null) where T : Object =>
            _assets.Instantiate<T>(at, rotation, parent);

        public T Instantiate<T>(Transform parent = null) where T : Object =>
            _assets.Instantiate<T>(parent);
    }
}