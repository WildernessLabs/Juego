using System;
using Meadow;
using Meadow.Foundation;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors.Hid;
using Meadow.Units;

namespace JuegoTester
{
    public class JuegoV1a_Hardware : IHardwareConfig
    {
        public IGraphicsDisplay Display { get; protected set; }
        public ISpiBus SpiBus { get; protected set; }
        public II2cBus I2cBus { get; protected set; }

        public RgbPwmLed OnboardLed { get; protected set; }
        public PiezoSpeaker LeftSpeaker { get; protected set; }

        public PiezoSpeaker RightSpeaker { get; protected set; }

        public IAnalogJoystick AnalogJoystick { get; protected set; }

        public PushButton UpButton { get; protected set; }
        public PushButton DownButton { get; protected set; }
        public PushButton LeftButton { get; protected set; }
        public PushButton RightButton { get; protected set; }

        public PushButton StartButton { get; protected set; }
        public PushButton SelectButton { get; protected set; }


        public void Initialize(IF7FeatherMeadowDevice device)
        {
            //==== SPI BUS
            try
            {
                var spiConfig = new SpiClockConfiguration(
                    new Frequency(48000, Frequency.UnitType.Kilohertz),
                    SpiClockConfiguration.Mode.Mode3);

                SpiBus = device.CreateSpiBus(
                    device.Pins.SCK,
                    device.Pins.MOSI,
                    device.Pins.MISO,
                    spiConfig);
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERR creating SPI: {e.Message}");
            }

            //==== Display
            if (SpiBus != null)
            {
                Display = new St7789(
                    device: device,
                    spiBus: SpiBus,
                    chipSelectPin: device.Pins.D14,
                    dcPin: device.Pins.D03,
                    resetPin: device.Pins.D04,
                    width: 240, height: 240,
                    colorMode: ColorType.Format16bppRgb565);
            }

            //==== Onboard LED
            OnboardLed = new RgbPwmLed(device: device,
                redPwmPin: device.Pins.OnboardLedRed,
                greenPwmPin: device.Pins.OnboardLedGreen,
                bluePwmPin: device.Pins.OnboardLedBlue);
            OnboardLed.StartPulse(WildernessLabsColors.ChileanFire);

            //==== Speakers
            LeftSpeaker = new PiezoSpeaker(device, device.Pins.D11);

            RightSpeaker = new PiezoSpeaker(device, device.Pins.D11);


            AnalogJoystick = new AnalogJoystick(device, device.Pins.A00, device.Pins.A01);


            //==== Buttons
            UpButton = new PushButton(device, device.Pins.D06, ResistorMode.InternalPullDown);
            DownButton = new PushButton(device, device.Pins.D05, ResistorMode.InternalPullDown);
            LeftButton = new PushButton(device, device.Pins.D12, ResistorMode.InternalPullDown);
            RightButton = new PushButton(device, device.Pins.D11, ResistorMode.InternalPullDown);
            StartButton = new PushButton(device, device.Pins.D13, ResistorMode.InternalPullDown);
            SelectButton = new PushButton(device, device.Pins.D15, ResistorMode.InternalPullDown);
        }
    }
}