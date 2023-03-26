using Meadow;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Units;
using System;

namespace WildernessLabs.Hardware.Juego
{
    public class JuegoHardwareV1 : IJuegoHardware
    {
        protected IF7FeatherMeadowDevice Device { get; }

        public IGraphicsDisplay Display { get; }

        protected ISpiBus Spi { get; }

        public AnalogJoystick? AnalogJoystick { get; protected set; }

        //==== Right side buttons
        public PushButton? Right_UpButton { get; protected set; }
        public PushButton? Right_DownButton { get; protected set; }
        public PushButton? Right_LeftButton { get; protected set; }
        public PushButton? Right_RightButton { get; protected set; }

        //==== Left side buttons
        public PushButton? Left_UpButton => null;
        public PushButton? Left_DownButton => null;
        public PushButton? Left_LeftButton => null;
        public PushButton? Left_RightButton => null;

        //==== Start/Select Buttons
        public PushButton? StartButton { get; protected set; }
        public PushButton? SelectButton { get; protected set; }

        //==== Speakers
        public PiezoSpeaker? LeftSpeaker { get; protected set; }
        public PiezoSpeaker? RightSpeaker { get; protected set; }

        public JuegoHardwareV1(IF7FeatherMeadowDevice device)
        {
            Device = device;

            Resolver.Log.Info("Initialize hardware...");

            //==== Speakers
            try
            {
                LeftSpeaker = new PiezoSpeaker(device.Pins.D12);
            }
            catch (Exception e)
            {
                Resolver.Log.Error($"Err Left Speaker: {e.Message}");
            }
            try
            {
                RightSpeaker = new PiezoSpeaker(device.Pins.D13);
            }
            catch (Exception e)
            {
                Resolver.Log.Error($"Err Right Speaker: {e.Message}");
            }

            //==== SPI
            Resolver.Log.Info("Initializing SPI Bus");
            try
            {
                var config = new SpiClockConfiguration(new Frequency(48000, Frequency.UnitType.Kilohertz), SpiClockConfiguration.Mode.Mode0);
                Spi = Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.COPI, Device.Pins.CIPO, config);
            }
            catch (Exception e)
            {
                Resolver.Log.Error($"Err initializing SPI: {e.Message}");
            }
            Resolver.Log.Info("SPI initialized");

            //Display
            var chipSelectPort = Device.CreateDigitalOutputPort(Device.Pins.D03);
            var dcPort = Device.CreateDigitalOutputPort(Device.Pins.D04);
            var resetPort = Device.CreateDigitalOutputPort(Device.Pins.D14);

            Display = new St7789(
                spiBus: Spi,
                chipSelectPort: chipSelectPort,
                dataCommandPort: dcPort,
                resetPort: resetPort,
                width: 240, height: 240);

            Resolver.Log.Info("Display initialized");

            Right_UpButton = new PushButton(device.Pins.D06, ResistorMode.InternalPullDown);
            Right_DownButton = new PushButton(device.Pins.D05, ResistorMode.InternalPullDown);
            Right_LeftButton = new PushButton(device.Pins.D12, ResistorMode.InternalPullDown);
            Right_RightButton = new PushButton(device.Pins.D11, ResistorMode.InternalPullDown);
            StartButton = new PushButton(device.Pins.D13, ResistorMode.InternalPullDown);
            SelectButton = new PushButton(device.Pins.D15, ResistorMode.InternalPullDown);

            //ToDo confirm analog pins and wire it up
            //AnalogJoystick = new AnalogJoystick(Device.Pins.A00.CreateAnalogInputPort(),
            //    Device.CreateAnalogInputPort(Device.Pins.A01));
        }
    }
}