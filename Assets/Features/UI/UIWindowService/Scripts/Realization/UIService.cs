using Assets.Features.UI.Scripts.Realization;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIService : IUIService
{
    private const string UIWindowsResourcesPath = "UIWindows";
    private const string DeactiveContainerName = "PoolContainer";

    private readonly Transform _deactiveContainer;
    private Transform _activeContainer;
    private Dictionary<Type, UIWindow> _windows = new Dictionary<Type, UIWindow>();

    public UIService(RectTransform activeContainer)
    {
        _activeContainer = activeContainer;
        _deactiveContainer = new GameObject(DeactiveContainerName).transform;
        _deactiveContainer.gameObject.SetActive(false);
        Init();
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(_deactiveContainer.gameObject);

        _activeContainer = null;
        foreach(var window in _windows.Values)
        {
            UnityEngine.Object.Destroy(window.gameObject);
        }
        _windows.Clear();
        _windows = null;
    }

    public void Init()
    {
        var windows = GetWindows(UIWindowsResourcesPath);
        foreach (var item in windows)
        {
            var window = UnityEngine.Object.Instantiate(item);
            _windows.Add(item.GetType(), window);
            window.RectTransform.SetParent(_deactiveContainer);
        }
    }

    public T Show<T>() where T : UIWindow
    {
        var window = _windows[typeof(T)];

        Show(window);

        var component = window.GetComponent<T>();
        return component;
    }

    public T Get<T>() where T : UIWindow
    {
        return _windows[typeof(T)].GetComponent<T>();
    }

    public void Hide<T>() where T : UIWindow
    {
        Hide(_windows[typeof(T)]);        
    }
    
    private IEnumerable<UIWindow> GetWindows(string windowsPoolNames)
    {
        return Resources.LoadAll<UIWindow>(windowsPoolNames);
    }

    private void Show(UIWindow window)
    {
        window.RectTransform.SetParent(_activeContainer);
        window.RectTransform.localScale = Vector3.one;
        window.RectTransform.localRotation = Quaternion.identity;
        window.RectTransform.localPosition = Vector3.zero;

        window.RectTransform.offsetMin = Vector2.zero;
        window.RectTransform.offsetMax = Vector2.zero;
        window.Show();
    }

    private void Hide(UIWindow window)
    {
        window.RectTransform.SetParent(_deactiveContainer);
        window.Hide();
    }
}
