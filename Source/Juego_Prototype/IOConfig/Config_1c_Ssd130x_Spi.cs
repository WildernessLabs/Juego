﻿using Meadow.Foundation.Audio;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Peripherals.Leds;

namespace Juego
{
    public class Config_1c_Ssd130x_Spi : IIOConfig
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


        public Config_1c_Ssd130x_Spi()
        {
            var device = MeadowApp.Device;

            //    var config = new SpiClockConfiguration(3000, SpiClockConfiguration.Mode.Mode3);
            var bus = device.CreateSpiBus(device.Pins.SCK, device.Pins.MOSI, device.Pins.MISO);

            var display = new Ssd1309(
                spiBus: bus,
                chipSelectPin: IODeviceMap.Display.CSPin,
                dcPin: IODeviceMap.Display.DCPin,
                resetPin: IODeviceMap.Display.ResetPin);

            display.Contrast = 255;


            Graphics = new MicroGraphics(display)
            {
                CurrentFont = new Font8x12(),
                IgnoreOutOfBoundsPixels = true,
            };

            Up = new PushButton(device.Pins.D06, ResistorMode.InternalPullDown);
            Down = new PushButton(device.Pins.D05, ResistorMode.InternalPullDown);
            Left = new PushButton(device.Pins.D12, ResistorMode.InternalPullDown);
            Right = new PushButton(device.Pins.D11, ResistorMode.InternalPullDown);
            Start = new PushButton(device.Pins.D13, ResistorMode.InternalPullDown);
            Select = new PushButton(device.Pins.D15, ResistorMode.InternalPullDown);

            rgbLed = new RgbPwmLed(
                redPwmPin: device.Pins.OnboardLedRed,
                greenPwmPin: device.Pins.OnboardLedGreen,
                bluePwmPin: device.Pins.OnboardLedBlue,
                CommonType.CommonAnode);
        }
    }
}