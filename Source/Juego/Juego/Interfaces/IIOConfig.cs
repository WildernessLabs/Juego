using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;

namespace Juego
{
    public interface IIOConfig
    {
        GraphicsLibrary Graphics { get; }

        AnalogJoystick Joystick { get; }

        PushButton Up { get; }
        PushButton Down { get; }
        PushButton Left { get; }
        PushButton Right { get; }
        PushButton Start { get; }
        PushButton Select { get; }
    }
}