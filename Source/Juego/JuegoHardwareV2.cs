using Meadow;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Displays;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Accelerometers;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Units;
using System;
using System.Threading;

namespace WildernessLabs.Hardware.Juego
{
    /// <summary>
    /// Represents the hardware interface for the Juego v2 device
    /// </summary>
    public class JuegoHardwareV2 : IJuegoHardware
    {
        /// <inheritdoc/>
        protected IF7CoreComputeMeadowDevice Device { get; }
        /// <inheritdoc/>
        protected IDigitalInterruptPort? McpInterrupt_1 { get; }
        /// <inheritdoc/>
        protected IDigitalInterruptPort? McpInterrupt_2 { get; }
        /// <inheritdoc/>
        protected IDigitalOutputPort? Mcp_Reset { get; }
        /// <inheritdoc/>
        public IPixelDisplay? Display { get; }
        /// <inheritdoc/>
        public IDigitalOutputPort? DisplayBacklightPort { get; }
        /// <inheritdoc/>
        protected II2cBus I2cBus { get; }
        /// <inheritdoc/>
        protected ISpiBus? SpiBus { get; }
        /// <inheritdoc/>
        public Mcp23008? Mcp_1 { get; protected set; }
        /// <inheritdoc/>
        public Mcp23008? Mcp_2 { get; protected set; }
        /// <inheritdoc/>
        public Mcp23008? Mcp_VersionInfo { get; set; }
        /// <inheritdoc/>
        public PushButton? Right_UpButton { get; protected set; }
        /// <inheritdoc/>
        public PushButton? Right_DownButton { get; protected set; }
        /// <inheritdoc/>
        public PushButton? Right_LeftButton { get; protected set; }
        /// <inheritdoc/>
        public PushButton? Right_RightButton { get; protected set; }
        /// <inheritdoc/>
        public PushButton? Left_UpButton { get; protected set; }
        /// <inheritdoc/>
        public PushButton? Left_DownButton { get; protected set; }
        /// <inheritdoc/>
        public PushButton? Left_LeftButton { get; protected set; }
        /// <inheritdoc/>
        public PushButton? Left_RightButton { get; protected set; }
        /// <inheritdoc/>
        public PushButton? StartButton { get; protected set; }
        /// <inheritdoc/>
        public PushButton? SelectButton { get; protected set; }
        /// <inheritdoc/>
        public PiezoSpeaker? LeftSpeaker { get; set; }
        /// <inheritdoc/>
        public PiezoSpeaker? RightSpeaker { get; set; }
        /// <inheritdoc/>
        public PwmLed? BlinkyLed { get; protected set; }
        /// <inheritdoc/>
        public Bmi270? MotionSensor => null;

        /// <inheritdoc/>
        public DisplayConnector DisplayHeader => (DisplayConnector)Connectors[0]!;

        /// <inheritdoc/>
        public I2cConnector? Qwiic => null;

        /// <inheritdoc/>
        public IConnector?[] Connectors
        {
            get
            {
                if (_connectors == null)
                {
                    _connectors = new IConnector[1];
                    _connectors[0] = CreateDisplayConnector();
                }

                return _connectors;
            }
        }

        private IConnector?[]? _connectors;

        /// <summary>
        /// Create a new Juego hardware v2 object
        /// </summary>
        public JuegoHardwareV2(IF7CoreComputeMeadowDevice device, II2cBus i2cBus)
        {
            Device = device;
            I2cBus = i2cBus;

            Resolver.Log.Info("Initialize hardware...");

            // DEV NOTE: **ALWAYS** Set up PWMs first - Nuttx PWM driver will step on pin configs otherwise
            /* try - code left intentionally, restore once the PWM bug is fixed
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
             } */

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
                BlinkyLed = new PwmLed(device.Pins.D20, TypicalForwardVoltage.Green);
            }
            catch (Exception e)
            {
                Resolver.Log.Error($"Err BlinkyLed: {e.Message}");
            }

            try
            {
                var config = new SpiClockConfiguration(new Frequency(24, Frequency.UnitType.Megahertz), SpiClockConfiguration.Mode.Mode0);
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

                if (SpiBus != null)
                {
                    Display = new Ili9341(
                        spiBus: SpiBus,
                        chipSelectPort: chipSelectPort,
                        dataCommandPort: dcPort,
                        resetPort: resetPort,
                        width: 240, height: 320)
                    {
                        SpiBusSpeed = new Frequency(24, Frequency.UnitType.Megahertz),
                    };

                    ((Ili9341)Display).SetRotation(RotationType._270Degrees);

                    Resolver.Log.Info("Display initialized");
                }
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

        internal DisplayConnector? CreateDisplayConnector()
        {
            Resolver.Log.Trace("Creating display connector");

            if (Mcp_1 == null)
            {
                return null;
            }

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