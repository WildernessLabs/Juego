﻿using Meadow.Foundation.Audio;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Displays.TftSpi;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;

namespace Juego
{
    public class Config_1a_St7789 : IIOConfig
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


        public Config_1a_St7789()
        {
            var device = MeadowApp.Device;

            var config = new SpiClockConfiguration(48000, SpiClockConfiguration.Mode.Mode3);
            var bus = device.CreateSpiBus(IODeviceMap.Display.ClockPin, IODeviceMap.Display.CopiPin,
                IODeviceMap.Display.CipoPin, config);

            var display = new St7789(
                device: device, spiBus: bus,
                chipSelectPin: IODeviceMap.Display.CSPin,
                dcPin: IODeviceMap.Display.DCPin,
                resetPin: IODeviceMap.Display.ResetPin,
                width: 240,
                height: 240,
                displayColorMode: DisplayBase.DisplayColorMode.Format12bppRgb444
            );
            display.IgnoreOutOfBoundsPixels = true;

            Graphics = new GraphicsLibrary(display)
            {
                CurrentFont = new Font12x20(),
                Rotation = GraphicsLibrary.RotationType._180Degrees,
            };

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