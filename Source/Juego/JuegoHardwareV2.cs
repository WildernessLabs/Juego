﻿using Meadow;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Meadow.Logging;
using Meadow.Units;
using System;
using System.Threading;

namespace WildernessLabs.Hardware.Juego
{
    public class JuegoHardwareV2 : IJuegoHardware
    {
        protected IF7CoreComputeMeadowDevice Device { get; }
        protected IDigitalInterruptPort McpInterrupt_1 { get; }

        protected IDigitalInterruptPort McpInterrupt_2 { get; }

        protected IDigitalOutputPort Mcp_Reset { get; }

        public IGraphicsDisplay Display { get; }

        public IDigitalOutputPort DisplayBacklightPort { get; }

        protected II2cBus I2cBus { get; }
        protected ISpiBus SpiBus { get; }

        public Mcp23008 Mcp_1 { get; protected set; }
        public Mcp23008 Mcp_2 { get; protected set; }
        public Mcp23008 Mcp_VersionInfo { get; protected set; }

        public PushButton? Right_UpButton { get; protected set; }
        public PushButton? Right_DownButton { get; protected set; }
        public PushButton? Right_LeftButton { get; protected set; }
        public PushButton? Right_RightButton { get; protected set; }

        public PushButton? Left_UpButton { get; protected set; }
        public PushButton? Left_DownButton { get; protected set; }
        public PushButton? Left_LeftButton { get; protected set; }
        public PushButton? Left_RightButton { get; protected set; }

        public PushButton? StartButton { get; protected set; }
        public PushButton? SelectButton { get; protected set; }

        public PiezoSpeaker? LeftSpeaker { get; protected set; }
        public PiezoSpeaker? RightSpeaker { get; protected set; }

        public PwmLed? BlinkyLed { get; protected set; }

        /// <summary>
        /// Gets the display header connector on the Juego board
        /// </summary>
        public DisplayConnector DisplayHeader => (DisplayConnector)Connectors[0];

        /// <summary>
        /// Collection of connectors on the Juego board
        /// </summary>
        public IConnector?[] Connectors
        {
            get
            {
                if (_connectors == null)
                {
                    _connectors = new IConnector[1];
                    _connectors[1] = CreateDisplayConnector();
                }

                return _connectors;
            }
        }

        private IConnector?[]? _connectors;

        public JuegoHardwareV2(IF7CoreComputeMeadowDevice device)
        {
            Device = device;

            Resolver.Log.Info("Initialize hardware...");

            // DEV NOTE: **ALWAYS** Set up PWMs first - Nuttx PWM driver will step on pin configs otherwise
            try
            {
                LeftSpeaker = new PiezoSpeaker(device.Pins.PB8); //D03
            }
            catch (Exception e)
            {
                Resolver.Log.Error($"Err Left Speaker: {e.Message}");
            }

            try
            {
                RightSpeaker = new PiezoSpeaker(device.Pins.PB9); //D04
            }
            catch (Exception e)
            {
                Resolver.Log.Error($"Err Right Speaker: {e.Message}");
            }

            try
            {
                I2cBus = Device.CreateI2cBus(busSpeed: I2cBusSpeed.FastPlus);
                Resolver.Log.Info("I2C initialized");
            }
            catch (Exception e)
            {
                Resolver.Log.Error($"Err initializing I2C Bus: {e.Message}");
            }

            try
            {
                Mcp_Reset = Device.CreateDigitalOutputPort(Device.Pins.D11, true);
                McpInterrupt_1 = Device.CreateDigitalInterruptPort(Device.Pins.D09, InterruptMode.EdgeRising);
                Mcp_1 = new Mcp23008(I2cBus, 0x20, McpInterrupt_1, Mcp_Reset);
                Resolver.Log.Info("Mcp23008 #1 initialized");
            }
            catch (Exception e)
            {
                Resolver.Log.Error($"Err MCP 1: {e.Message}");
            }

            try
            {
                McpInterrupt_2 = Device.CreateDigitalInterruptPort(Device.Pins.D10, InterruptMode.EdgeRising);
                Mcp_2 = new Mcp23008(I2cBus, 0x21, McpInterrupt_2);
                Resolver.Log.Info("Mcp23008 #2 initialized");
            }
            catch (Exception e)
            {
                Resolver.Log.Error($"Err MCP 2: {e.Message}");
            }

            try
            {
                Mcp_VersionInfo = new Mcp23008(I2cBus, 0x23);
                Resolver.Log.Info("Mcp23008 version initialized");
            }
            catch (Exception e)
            {
                Resolver.Log.Error($"Err MCP 3: {e.Message}");
            }

            try
            {
                BlinkyLed = new PwmLed(device.Pins.D20, TypicalForwardVoltage.Green);
            }
            catch (Exception e)
            {
                Resolver.Log.Error($"Err BlinkyLed: {e.Message}");
            }

            try
            {
                var config = new SpiClockConfiguration(new Frequency(48000, Frequency.UnitType.Kilohertz), SpiClockConfiguration.Mode.Mode0);
                SpiBus = Device.CreateSpiBus(Device.Pins.SPI5_SCK, Device.Pins.SPI5_COPI, Device.Pins.SPI5_CIPO, config);
            }
            catch (Exception e)
            {
                Resolver.Log.Error($"Err initializing SPI: {e.Message}");
            }
            Resolver.Log.Info("SPI initialized");

            if (Mcp_1 != null)
            {
                DisplayBacklightPort = Device.CreateDigitalOutputPort(Device.Pins.D05, true);

                var chipSelectPort = Mcp_1.CreateDigitalOutputPort(Mcp_1.Pins.GP5);
                var dcPort = Mcp_1.CreateDigitalOutputPort(Mcp_1.Pins.GP6);
                var resetPort = Mcp_1.CreateDigitalOutputPort(Mcp_1.Pins.GP7);

                Thread.Sleep(50);

                Display = new Ili9341(
                    spiBus: SpiBus,
                    chipSelectPort: chipSelectPort,
                    dataCommandPort: dcPort,
                    resetPort: resetPort,
                    width: 240, height: 320)
                {
                    SpiBusSpeed = new Frequency(48000, Frequency.UnitType.Kilohertz),
                };

                ((Ili9341)Display).SetRotation(RotationType._270Degrees);

                Resolver.Log.Info("Display initialized");
            }

            if (Mcp_1 != null)
            {
                var upPort = Mcp_1.Pins.GP1.CreateDigitalInterruptPort(InterruptMode.EdgeBoth, ResistorMode.InternalPullUp);
                var rightPort = Mcp_1.CreateDigitalInterruptPort(Mcp_1.Pins.GP2, InterruptMode.EdgeBoth, ResistorMode.InternalPullUp);
                var downPort = Mcp_1.CreateDigitalInterruptPort(Mcp_1.Pins.GP3, InterruptMode.EdgeBoth, ResistorMode.InternalPullUp);
                var leftPort = Mcp_1.CreateDigitalInterruptPort(Mcp_1.Pins.GP4, InterruptMode.EdgeBoth, ResistorMode.InternalPullUp);

                Left_UpButton = new PushButton(upPort);
                Left_RightButton = new PushButton(rightPort);
                Left_DownButton = new PushButton(downPort);
                Left_LeftButton = new PushButton(leftPort);
            }

            if (Mcp_2 != null)
            {
                var upPort = Mcp_2.CreateDigitalInterruptPort(Mcp_2.Pins.GP5, InterruptMode.EdgeBoth, ResistorMode.InternalPullUp);
                var rightPort = Mcp_2.CreateDigitalInterruptPort(Mcp_2.Pins.GP4, InterruptMode.EdgeBoth, ResistorMode.InternalPullUp);
                var downPort = Mcp_2.CreateDigitalInterruptPort(Mcp_2.Pins.GP3, InterruptMode.EdgeBoth, ResistorMode.InternalPullUp);
                var leftPort = Mcp_2.CreateDigitalInterruptPort(Mcp_2.Pins.GP2, InterruptMode.EdgeBoth, ResistorMode.InternalPullUp);
                var startPort = Mcp_2.CreateDigitalInterruptPort(Mcp_2.Pins.GP1, InterruptMode.EdgeBoth, ResistorMode.InternalPullUp);
                var selectPort = Mcp_2.CreateDigitalInterruptPort(Mcp_2.Pins.GP0, InterruptMode.EdgeBoth, ResistorMode.InternalPullUp);

                Right_UpButton = new PushButton(upPort);
                Right_RightButton = new PushButton(rightPort);
                Right_DownButton = new PushButton(downPort);
                Right_LeftButton = new PushButton(leftPort);
                StartButton = new PushButton(startPort);
                SelectButton = new PushButton(selectPort);
            }
        }
        internal DisplayConnector CreateDisplayConnector()
        {
            Resolver.Log.Trace("Creating display connector");

            return new DisplayConnector(
               "Display",
                new PinMapping
                {
                new PinMapping.PinAlias(DisplayConnector.PinNames.CS, Mcp_1.Pins.GP5),
                new PinMapping.PinAlias(DisplayConnector.PinNames.RST, Mcp_1.Pins.GP7),
                new PinMapping.PinAlias(DisplayConnector.PinNames.DC, Mcp_1.Pins.GP6),
                new PinMapping.PinAlias(DisplayConnector.PinNames.CLK, Device.Pins.SCK),
                new PinMapping.PinAlias(DisplayConnector.PinNames.COPI, Device.Pins.COPI),
                });
        }
    }
}