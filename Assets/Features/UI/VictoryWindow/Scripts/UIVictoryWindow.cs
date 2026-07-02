using Assets.Features.UI.Scripts.Realization;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIVictoryWindow : UIWindow
{
    public event EventHandler ButtonPressed;

    [SerializeField] private Button _button;

    protected override void OnShow()
    {
        _button.onClick.AddListener(OnButtonPressed);
    }

    protected override void OnHide()
    {
        _button.onClick.RemoveListener(OnButtonPressed);
    }

    private void OnButtonPressed()
    {
        ButtonPressed?.Invoke(this, EventArgs.Empty);
    }
}
