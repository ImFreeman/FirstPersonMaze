using Zenject;

namespace Features.Input
{
    public abstract class InputElement : ITickable
    {
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (value != _isActive)
                {
                    if (value)
                    {
                        _tickableManager.Add(this);
                    }
                    else
                    {
                        _tickableManager.Remove(this);
                    }
                    _isActive = value;
                }
            }
        }

        private readonly TickableManager _tickableManager;
        private bool _isActive;

        public InputElement(TickableManager tickableManager)
        {
            _tickableManager = tickableManager;
        }

        public abstract void Tick();
    }
}