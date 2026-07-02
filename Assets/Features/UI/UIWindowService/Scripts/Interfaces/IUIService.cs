using Assets.Features.UI.Scripts.Realization;
using System;

public interface IUIService : IDisposable
{
    public void Init();
    public T Show<T>() where T : UIWindow;
    public T Get<T>() where T : UIWindow;
    public void Hide<T>() where T : UIWindow;
}
