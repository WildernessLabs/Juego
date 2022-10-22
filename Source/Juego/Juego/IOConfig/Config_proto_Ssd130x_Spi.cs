using System;
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
    public class Config_proto_Ssd130x_Spi : IIOConfig
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

        public Config_proto_Ssd130x_Spi()
        {
            var device = MeadowApp.Device;

            var config = new SpiClockConfiguration(new Meadow.Units.Frequency(12000, Meadow.Units.Frequency.UnitType.Kilohertz), SpiClockConfiguration.Mode.Mode0);

            var bus = device.CreateSpiBus(device.Pins.SCK, device.Pins.MOSI, device.Pins.MISO, config);

            var display = new Ssd1309
            (
                device: device,
                spiBus: bus,
                chipSelectPin: device.Pins.D03,
                dcPin: device.Pins.D01,
                resetPin: device.Pins.D00
            );

            display.Contrast = 128;

            Graphics = new MicroGraphics(display)
            {
                CurrentFont = new Font8x12(),
                IgnoreOutOfBoundsPixels = true
            };

            Left = new PushButton(device, device.Pins.D12, ResistorMode.ExternalPullUp);
            Right = new PushButton(device, device.Pins.D11, ResistorMode.ExternalPullUp);
            Down = new PushButton(device, device.Pins.D13, ResistorMode.ExternalPullUp);
            Up = new PushButton(device, device.Pins.D09, ResistorMode.ExternalPullUp);

            rgbLed = new RgbPwmLed(device: device,
                redPwmPin: device.Pins.OnboardLedRed,
                greenPwmPin: device.Pins.OnboardLedGreen,
                bluePwmPin: device.Pins.OnboardLedBlue,
                CommonType.CommonAnode);
        }
    }
}