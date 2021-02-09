using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors.Buttons;

namespace MeadowApp
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbPwmLed onboardLed;

        IButton up = null;
        IButton down = null;
        IButton left = null;
        IButton right = null;
        IButton select = null;
        IButton start = null;

        GraphicsLibrary graphics;

        public MeadowApp()
        {
            Initialize();
         //   CycleColors(1000);
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);

            up = new PushButton(Device, Device.Pins.D06, ResistorMode.InternalPullDown);
            up.Clicked += Up_Clicked;

            left = new PushButton(Device, Device.Pins.D12, ResistorMode.InternalPullDown);
            left.Clicked += Left_Clicked;

            right = new PushButton(Device, Device.Pins.D11, ResistorMode.InternalPullDown);
            right.Clicked += Right_Clicked;

            down = new PushButton(Device, Device.Pins.D05, ResistorMode.InternalPullDown);
            down.Clicked += Down_Clicked;


            Console.WriteLine("Create display...");

            var config = new SpiClockConfiguration(48000, SpiClockConfiguration.Mode.Mode3);
            var bus = Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config);

              //  (IODeviceMap.Display.ClockPin, IODeviceMap.Display.CopiPin, IODeviceMap.Display.CipoPin, config);

            var display = new St7789(
                device: Device, spiBus: bus,
                chipSelectPin: Device.Pins.D14,
                dcPin: Device.Pins.D03,
                resetPin: Device.Pins.D04,
                width: 240,
                height: 240,
                displayColorMode: DisplayBase.DisplayColorMode.Format12bppRgb444
            );
            display.IgnoreOutOfBoundsPixels = true;

            Console.WriteLine("Create GraphicsLibrary...");

            graphics = new GraphicsLibrary(display)
            {
                CurrentFont = new Font12x20(),
                Rotation = GraphicsLibrary.RotationType._90Degrees,
            };
        }

        void Update(string msg)
        {
            Console.WriteLine(msg);
            graphics.Clear();
            graphics.DrawText(0, 0, msg, Color.AliceBlue);
            graphics.Show();
        }

        private void Down_Clicked(object sender, EventArgs e)
        {
            Update("down");
        }

        private void Right_Clicked(object sender, EventArgs e)
        {
            Update("right");
        }

        private void Left_Clicked(object sender, EventArgs e)
        {
            Update("left");
        }

        private void Up_Clicked(object sender, EventArgs e)
        {
            Update("up");
        }

        void CycleColors(int duration)
        {
            Console.WriteLine("Cycle colors...");

            while (true)
            {
                ShowColorPulse(Color.Blue, duration);
                ShowColorPulse(Color.Cyan, duration);
                ShowColorPulse(Color.Green, duration);
                ShowColorPulse(Color.GreenYellow, duration);
                ShowColorPulse(Color.Yellow, duration);
                ShowColorPulse(Color.Orange, duration);
                ShowColorPulse(Color.OrangeRed, duration);
                ShowColorPulse(Color.Red, duration);
                ShowColorPulse(Color.MediumVioletRed, duration);
                ShowColorPulse(Color.Purple, duration);
                ShowColorPulse(Color.Magenta, duration);
                ShowColorPulse(Color.Pink, duration);
            }
        }

        void ShowColorPulse(Color color, int duration = 1000)
        {
            onboardLed.StartPulse(color, duration / 2);
            Thread.Sleep(duration);
            onboardLed.Stop();
        }

        void ShowColor(Color color, int duration = 1000)
        {
            Console.WriteLine($"Color: {color}");
            onboardLed.SetColor(color);
            Thread.Sleep(duration);
            onboardLed.Stop();
        }
    }
}