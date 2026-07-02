using System;

public class Hud : IDisposable
{
    private readonly IUIService _uiService;
    private readonly Collectables _collectables;

    private bool _isShowed;
    public Hud(IUIService uiService, Collectables collectables)
    {
        _uiService = uiService;
        _collectables = collectables;

        _uiService.Get<UIHudWindow>().Shown += OnShown;
        _uiService.Get<UIHudWindow>().Hidden += OnHidden;        
    }
    public void Dispose()
    {
        _uiService.Get<UIHudWindow>().Shown -= OnShown;
        _uiService.Get<UIHudWindow>().Hidden -= OnHidden;
        if (_isShowed)
        {
            _collectables.OnCountChanged -= OnCountChanged;
        }
    }

    private void OnCountChanged(int obj)
    {
        _uiService.Get<UIHudWindow>().ScoreText.text = obj.ToString();
    }

    private void OnShown(object sender, EventArgs e)
    {
        _isShowed = true;        
        _collectables.OnCountChanged += OnCountChanged;
    }

    private void OnHidden(object sender, EventArgs e)
    {
        _isShowed = false;
        _collectables.OnCountChanged -= OnCountChanged;
    }        
}