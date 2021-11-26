using Meadow.Foundation.Audio;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Displays.TftSpi;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using System;

namespace Juego
{
    public class Config_1c_Ssd1351 : IIOConfig
    {
        public GraphicsLibrary Graphics { get; protected set; }

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


        public Config_1c_Ssd1351()
        {
            Console.WriteLine("Config_1c_Ssd1351");

            var device = MeadowApp.Device;

            var bus = device.CreateSpiBus(300);

            var display = new Ssd1351(
                device: device, 
                spiBus: bus,
                chipSelectPin: device.Pins.D14,
                dcPin: device.Pins.D03,
                resetPin: device.Pins.D04,
                width: 128,
                height: 128
            );
            //   display.IgnoreOutOfBoundsPixels = true;

            display.Clear(true);

            Graphics = new GraphicsLibrary(display)
            {
                CurrentFont = new Font12x20(),
                Rotation = RotationType._180Degrees,
            };

            Console.WriteLine("Draw line");

            Graphics.Clear();
            Graphics.DrawLine(0, 0, 10, 10, true);
            Graphics.Show();

            Up = new PushButton(device, device.Pins.D06, ResistorMode.InternalPullDown);
            Down = new PushButton(device, device.Pins.D05, ResistorMode.InternalPullDown);
            Left = new PushButton(device, device.Pins.D12, ResistorMode.InternalPullDown);
            Right = new PushButton(device, device.Pins.D11, ResistorMode.InternalPullDown);
            Start = new PushButton(device, device.Pins.D13, ResistorMode.InternalPullDown);
            Select = new PushButton(device, device.Pins.D15, ResistorMode.InternalPullDown);

            rgbLed = new RgbPwmLed(device: device,
                redPwmPin: device.Pins.OnboardLedRed,
                greenPwmPin: device.Pins.OnboardLedGreen,
                bluePwmPin: device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);
        }
    }
}