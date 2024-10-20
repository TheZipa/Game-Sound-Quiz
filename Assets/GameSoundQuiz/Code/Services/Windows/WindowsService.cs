﻿using System;
using System.Collections.Generic;

using GameSoundQuiz.Core.UI.Base;
using GameSoundQuiz.Services.WindowsFactory;
using Object = UnityEngine.Object;

namespace GameSoundQuiz.Services.Windows
{
    public class WindowsService : IWindowsService
    {
        private readonly Dictionary<Type, BaseWindow> _windows;
        private readonly Queue<Type> _removeWindowsQueue;
        private readonly IWindowsFactory _windowsFactory;
        
        public WindowsService(IWindowsFactory windowsFactory)
        {
            _windowsFactory = windowsFactory;
            _windows = new Dictionary<Type, BaseWindow>(8);
            _removeWindowsQueue = new Queue<Type>(4);
        }

        public TWindow AddPersistentWindow<TWindow>() where TWindow : BaseWindow
        {
            if (_windows.TryGetValue(typeof(TWindow), out BaseWindow window))
                return window as TWindow;

            TWindow persistentWindow = _windowsFactory.CreatePersistentWindow<TWindow>();
            persistentWindow.IsPersistent = true;
            persistentWindow.Hide();
            _windows.Add(typeof(TWindow), persistentWindow);
            
            return persistentWindow;
        }
        
        public TWindow GetWindow<TWindow>() where TWindow : BaseWindow
        {
            if (_windows.TryGetValue(typeof(TWindow), out BaseWindow window))
                return window as TWindow;

            TWindow newWindow = _windowsFactory.CreateSceneWindow<TWindow>();
            _windows.Add(typeof(TWindow), newWindow);
            return newWindow;
        }

        public void Clear()
        {
            foreach (KeyValuePair<Type, BaseWindow> windowPair in _windows)
            {
                if(windowPair.Value.IsPersistent) continue;
                _removeWindowsQueue.Enqueue(windowPair.Key);
            }

            while (_removeWindowsQueue.Count > 0)
            {
                Type removeWindowType = _removeWindowsQueue.Dequeue();
                BaseWindow removeWindow = _windows[removeWindowType];
                Object.Destroy(removeWindow);
                _windows.Remove(removeWindowType);
            }
            
            _windowsFactory.Clear();
        }
    }
}