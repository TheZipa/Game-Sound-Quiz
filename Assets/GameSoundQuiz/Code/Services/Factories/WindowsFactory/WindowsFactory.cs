using GameSoundQuiz.Core.UI.Base;
using GameSoundQuiz.Services.Assets;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace GameSoundQuiz.Services.WindowsFactory
{
    public class WindowsFactory : IWindowsFactory
    {
        private const string RootCanvasPrefabName = "RootCanvas";

        private readonly IAssets _assets;
        private readonly IObjectResolver _objectResolver;

        private IContainerBuilder _builder;
        private GameObject _sceneCanvas;
        private GameObject _persistentCanvas;

        public WindowsFactory(IAssets assets, IObjectResolver objectResolver)
        {
            _assets = assets;
            _objectResolver = objectResolver;
        }
        
        public GameObject CreateCanvas()
        {
            GameObject canvasPrefab = _assets.ProvidePrefab<GameObject>(RootCanvasPrefabName);
            return Object.Instantiate(canvasPrefab);
        }

        public TWindow CreatePersistentWindow<TWindow>() where TWindow : BaseWindow =>
            CreateWindow<TWindow>(_persistentCanvas.transform);

        public TWindow CreateSceneWindow<TWindow>() where TWindow : BaseWindow
        {
            _sceneCanvas ??= CreateCanvas();
            return CreateWindow<TWindow>(_sceneCanvas.transform);
        }

        public void CreatePersistentCanvas()
        {
            _persistentCanvas = CreateCanvas();
            _persistentCanvas.GetComponent<Canvas>().sortingOrder = 52;
            _persistentCanvas.name = "PersistentCanvas";
            Object.DontDestroyOnLoad(_persistentCanvas);
        }
        
        public void Clear()
        {
            if(_sceneCanvas is not null) Object.Destroy(_sceneCanvas);
            _sceneCanvas = null;
        }

        private TWindow CreateWindow<TWindow>(Transform canvas) where TWindow : BaseWindow
        {
            TWindow windowPrefab = _assets.ProvidePrefab<TWindow>();
            TWindow window = _objectResolver.Instantiate(windowPrefab, canvas);

            return window;
        }
    }
}