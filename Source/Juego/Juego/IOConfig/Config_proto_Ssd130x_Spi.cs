using System;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;

namespace Juego
{
    public class Config_proto_Ssd130x_Spi : IIOConfig
    {
        public GraphicsLibrary Graphics { get; protected set; }

        public AnalogJoystick Joystick { get; protected set; }

        public PushButton Up { get; protected set; }
        public PushButton Down { get; protected set; }
        public PushButton Left { get; protected set; }
        public PushButton Right { get; protected set; }
        public PushButton Start { get; protected set; }
        public PushButton Select { get; protected set; }

        public Config_proto_Ssd130x_Spi()
        {
            var device = MeadowApp.Device;

            var config = new SpiClockConfiguration(9000, SpiClockConfiguration.Mode.Mode0);

            var bus = device.CreateSpiBus(device.Pins.SCK, device.Pins.MOSI, device.Pins.MISO, config);

            var display = new Ssd1309
            (
                device: device,
                spiBus: bus,
                chipSelectPin: device.Pins.D02,
                dcPin: device.Pins.D01,
                resetPin: device.Pins.D00
            );
            display.IgnoreOutOfBoundsPixels = true;

            Graphics = new GraphicsLibrary(display)
            {
                CurrentFont = new Font8x12(),
            };

            Left = new PushButton(device, device.Pins.D11, ResistorMode.ExternalPullUp);
            Right = new PushButton(device, device.Pins.D10, ResistorMode.ExternalPullUp);
            Down = new PushButton(device, device.Pins.D12, ResistorMode.ExternalPullUp);
            Up = new PushButton(device, device.Pins.D14, ResistorMode.ExternalPullUp);
        }
    }
}