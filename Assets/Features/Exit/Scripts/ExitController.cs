using System;

public class ExitController : IDisposable
{
    public event Action OnPlayerExited;

    private readonly Collectables _collectables;
    private readonly ExitView _exitView;
    private readonly int _playerInstanceId;

    private bool _isOpened;

    public ExitController(Collectables collectables, ExitView exitView, PlayerView playerView)
    {
        _collectables = collectables;
        _exitView = exitView;
        _playerInstanceId = playerView.gameObject.GetInstanceID();

        _collectables.OnCountChanged += HandleCountChanged;
        _exitView.OnPlayerEntered += HandlePlayerEntered;
        
        HandleCountChanged(_collectables.Count);
    }

    public void Dispose()
    {
        _collectables.OnCountChanged -= HandleCountChanged;
        _exitView.OnPlayerEntered -= HandlePlayerEntered;
    }

    private void HandleCountChanged(int count)
    {
        _isOpened = count == 0;
        UpdateView();
    }

    private void UpdateView()
    {
        _exitView.SetOpen(_isOpened);
    }

    private void HandlePlayerEntered(object sender, int instanceId)
    {
        if (!_isOpened || instanceId != _playerInstanceId)
            return;

        OnPlayerExited?.Invoke();
    }    
}