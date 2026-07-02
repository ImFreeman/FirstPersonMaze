using System;
using UnityEngine;

namespace Assets.Features.UI.Scripts.Realization
{
    public abstract class UIWindow : MonoBehaviour
    {
        public event EventHandler Shown;
        public event EventHandler Hidden;
        public RectTransform RectTransform => _rectTransform;

        [SerializeField] protected RectTransform _rectTransform;
        public void Show()
        {
            OnShow();
            Shown?.Invoke(this, EventArgs.Empty);
        }
        public void Hide()
        {
            OnHide();
            Hidden?.Invoke(this, EventArgs.Empty);
        }

        protected abstract void OnShow();
        protected abstract void OnHide();

    }
}
