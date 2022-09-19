using Meadow;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors.Hid;

namespace JuegoTester
{
    public interface IHardwareConfig
    {
        IGraphicsDisplay Display { get; }

        RgbPwmLed OnboardLed { get; }

        ISpiBus SpiBus { get; }
        II2cBus I2cBus { get; }

        IAnalogJoystick AnalogJoystick { get; }

        PushButton UpButton { get; }
        PushButton DownButton { get; }
        PushButton LeftButton { get; }
        PushButton RightButton { get; }

        PushButton StartButton { get; }
        PushButton SelectButton { get; }

        PiezoSpeaker LeftSpeaker { get; }

        PiezoSpeaker RightSpeaker { get; }

        void Initialize(IF7FeatherMeadowDevice device);
    }
}