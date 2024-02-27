using Meadow.Foundation.Audio;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Accelerometers;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Units;
using System;

namespace Meadow.Devices;

/// <summary>
/// Represents the hardware interface for the Juego v1 device
/// </summary>
public class JuegoHardwareV1 : IJuegoHardware
{
    /// <inheritdoc/>
    protected IF7FeatherMeadowDevice Device { get; }

    /// <inheritdoc/>
    public IPixelDisplay? Display { get; }

    /// <inheritdoc/>
    protected ISpiBus? SpiBus { get; }

    /// <inheritdoc/>
    public AnalogJoystick? AnalogJoystick { get; protected set; }

    /// <inheritdoc/>
    public PushButton? Right_UpButton { get; protected set; }
    /// <inheritdoc/>
    public PushButton? Right_DownButton { get; protected set; }
    /// <inheritdoc/>
    public PushButton? Right_LeftButton { get; protected set; }
    /// <inheritdoc/>
    public PushButton? Right_RightButton { get; protected set; }
    /// <inheritdoc/>
    public PushButton? Left_UpButton => null;
    /// <inheritdoc/>
    public PushButton? Left_DownButton => null;
    /// <inheritdoc/>
    public PushButton? Left_LeftButton => null;
    /// <inheritdoc/>
    public PushButton? Left_RightButton => null;
    /// <inheritdoc/>
    public PushButton? StartButton { get; protected set; }
    /// <inheritdoc/>
    public PushButton? SelectButton { get; protected set; }
    /// <inheritdoc/>
    public PiezoSpeaker? LeftSpeaker { get; protected set; }
    /// <inheritdoc/>
    public PiezoSpeaker? RightSpeaker { get; protected set; }
    /// <inheritdoc/>
    public PwmLed? BlinkyLed => null;

    /// <inheritdoc/>
    public Bmi270? MotionSensor => null;

    /// <inheritdoc/>
    public DisplayConnector DisplayHeader => (DisplayConnector)Connectors[0]!;

    /// <inheritdoc/>
    public I2cConnector? Qwiic => null;


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
                _connectors[0] = CreateDisplayConnector();
            }

            return _connectors;
        }
    }

    private IConnector?[]? _connectors;

    /// <summary>
    /// Create a new Juego hardware v1 object
    /// </summary>
    public JuegoHardwareV1(IF7FeatherMeadowDevice device)
    {
        Device = device;

        Resolver.Log.Info("Initialize hardware...");

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

        try
        {
            var config = new SpiClockConfiguration(new Frequency(24, Frequency.UnitType.Megahertz), SpiClockConfiguration.Mode.Mode0);
            SpiBus = Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.COPI, Device.Pins.CIPO, config);
            Resolver.Log.Info("SPI initialized");
        }
        catch (Exception e)
        {
            Resolver.Log.Error($"Err initializing SPI: {e.Message}");
        }

        var chipSelectPort = Device.CreateDigitalOutputPort(Device.Pins.D03);
        var dcPort = Device.CreateDigitalOutputPort(Device.Pins.D04);
        var resetPort = Device.CreateDigitalOutputPort(Device.Pins.D14);

        if (SpiBus != null)
        {
            Display = new St7789(
                spiBus: SpiBus,
                chipSelectPort: chipSelectPort,
                dataCommandPort: dcPort,
                resetPort: resetPort,
                width: 240, height: 240);
            Resolver.Log.Info("Display initialized");
        }

        Right_UpButton = new PushButton(device.Pins.D06, ResistorMode.InternalPullDown);
        Right_DownButton = new PushButton(device.Pins.D05, ResistorMode.InternalPullDown);
        Right_LeftButton = new PushButton(device.Pins.D12, ResistorMode.InternalPullDown);
        Right_RightButton = new PushButton(device.Pins.D11, ResistorMode.InternalPullDown);
        StartButton = new PushButton(device.Pins.D13, ResistorMode.InternalPullDown);
        SelectButton = new PushButton(device.Pins.D15, ResistorMode.InternalPullDown);
    }

    internal DisplayConnector CreateDisplayConnector()
    {
        Resolver.Log.Trace("Creating display connector");

        return new DisplayConnector(
           "Display",
            new PinMapping
            {
            new PinMapping.PinAlias(DisplayConnector.PinNames.CS, Device.Pins.D03),
            new PinMapping.PinAlias(DisplayConnector.PinNames.RST, Device.Pins.D14),
            new PinMapping.PinAlias(DisplayConnector.PinNames.DC, Device.Pins.D04),
            new PinMapping.PinAlias(DisplayConnector.PinNames.CLK, Device.Pins.SCK),
            new PinMapping.PinAlias(DisplayConnector.PinNames.COPI, Device.Pins.COPI),
            });
    }
}