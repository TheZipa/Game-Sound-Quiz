using GameSoundQuiz.Services.EntityContainer;
using UnityEngine;

namespace GameSoundQuiz.Services.Factories.BaseFactory
{
    public interface IBaseFactory
    {
        T InstantiateAsRegistered<T>(Vector3 at, Quaternion rotation, Transform parent = null) where T : Object, IFactoryEntity;
        T InstantiateAsRegistered<T>(Transform parent = null) where T : Object, IFactoryEntity;
        T Instantiate<T>(Transform parent = null) where T : Object;
        T Instantiate<T>(Vector3 at, Vector3 rotation, Transform parent = null) where T : Object;
    }
}