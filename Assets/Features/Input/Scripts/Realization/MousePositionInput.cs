using Zenject;

namespace Features.Input
{
    public class MousePositionInput : InputElement, ICameraRotationInput
    {
        public float InputX => _mouseX;
        public float InputY => _mouseY;

        private float _mouseX;
        private float _mouseY;

        private const string MouseXAxis = "Mouse X";
        private const string MouseYAxis = "Mouse Y";

        public MousePositionInput(TickableManager tickableManager) : base(tickableManager)
        {
        }

        public override void Tick()
        {
            _mouseX = UnityEngine.Input.GetAxis(MouseXAxis);
            _mouseY = UnityEngine.Input.GetAxis(MouseYAxis);            
        }
    }
}