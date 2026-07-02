using Assets.Features.UI.Scripts.Realization;
using TMPro;
using UnityEngine;

public class UIHudWindow : UIWindow
{
    [SerializeField] private TMP_Text _scoreText;
    public TMP_Text ScoreText => _scoreText;
    protected override void OnShow()
    {
        
    }
    protected override void OnHide()
    {
        
    }    
}
