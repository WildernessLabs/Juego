﻿using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Meadow.Peripherals.Leds;
using Meadow.Peripherals.Sensors.Buttons;

namespace MeadowApp
{
    public class MeadowApp : App<F7FeatherV1>
    {
        RgbPwmLed onboardLed;

        IButton up = null;
        IButton down = null;
        IButton left = null;
        IButton right = null;
        IButton select = null;
        IButton start = null;

        MicroGraphics graphics;

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
                CommonType.CommonAnode);

            up = new PushButton(Device, Device.Pins.D06, ResistorMode.InternalPullDown);
            up.Clicked += Up_Clicked;

            left = new PushButton(Device, Device.Pins.D12, ResistorMode.InternalPullDown);
            left.Clicked += Left_Clicked;

            right = new PushButton(Device, Device.Pins.D11, ResistorMode.InternalPullDown);
            right.Clicked += Right_Clicked;

            down = new PushButton(Device, Device.Pins.D05, ResistorMode.InternalPullDown);
            down.Clicked += Down_Clicked;


            Console.WriteLine("Create display...");

            var config = new SpiClockConfiguration(new Meadow.Units.Frequency(48000, Meadow.Units.Frequency.UnitType.Kilohertz), SpiClockConfiguration.Mode.Mode3);
            var bus = Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config);

              //  (IODeviceMap.Display.ClockPin, IODeviceMap.Display.CopiPin, IODeviceMap.Display.CipoPin, config);

            var display = new St7789(
                device: Device, spiBus: bus,
                chipSelectPin: Device.Pins.D14,
                dcPin: Device.Pins.D03,
                resetPin: Device.Pins.D04,
                width: 240,
                height: 240,
                colorMode: ColorType.Format12bppRgb444
            );

            Console.WriteLine("Create GraphicsLibrary...");

            graphics = new MicroGraphics(display)
            {
                CurrentFont = new Font12x20(),
                Rotation = RotationType._90Degrees,
                IgnoreOutOfBoundsPixels = true,
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