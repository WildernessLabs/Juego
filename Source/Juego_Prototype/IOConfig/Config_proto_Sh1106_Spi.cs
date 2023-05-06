using Meadow.Foundation.Audio;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Peripherals.Leds;

namespace Juego
{
    public class Config_proto_Sh1106_Spi : IIOConfig
    {
        public MicroGraphics Graphics { get; protected set; }

        public AnalogJoystick Joystick { get; protected set; }

        public PushButton Up { get; protected set; }
        public PushButton Down { get; protected set; }
        public PushButton Left { get; protected set; }
        public PushButton Right { get; protected set; }
        public PushButton Start { get; protected set; }
        public PushButton Select { get; protected set; }

        public PiezoSpeaker speakerLeft { get; protected set; }
        public PiezoSpeaker speakerRight { get; protected set; }

        public RgbPwmLed rgbLed { get; protected set; }

        public Config_proto_Sh1106_Spi()
        {
            var device = MeadowApp.Device;

            var config = new SpiClockConfiguration(new Meadow.Units.Frequency(12000, Meadow.Units.Frequency.UnitType.Kilohertz), SpiClockConfiguration.Mode.Mode0);

            var bus = device.CreateSpiBus(device.Pins.SCK, device.Pins.MOSI, device.Pins.MISO, config);

            var display = new Sh1106
            (
                spiBus: bus,
                chipSelectPin: device.Pins.D02,
                dcPin: device.Pins.D01,
                resetPin: device.Pins.D00
            );

            Graphics = new MicroGraphics(display)
            {
                CurrentFont = new Font8x12(),
                IgnoreOutOfBoundsPixels = true,
                Rotation = RotationType._180Degrees
            };

            Left = new PushButton(device.Pins.D13, ResistorMode.InternalPullDown);
            Right = new PushButton(device.Pins.D09, ResistorMode.InternalPullDown);
            Down = new PushButton(device.Pins.D11, ResistorMode.InternalPullDown);
            Up = new PushButton(device.Pins.D12, ResistorMode.InternalPullDown);

            rgbLed = new RgbPwmLed(
                redPwmPin: device.Pins.OnboardLedRed,
                greenPwmPin: device.Pins.OnboardLedGreen,
                bluePwmPin: device.Pins.OnboardLedBlue,
                CommonType.CommonAnode);
        }
    }
}