using Meadow.Foundation.Audio;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Motion;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;

namespace Meadow.Devices;

/// <summary>
/// Represents the hardware interface for the Juego device
/// </summary>
public interface IJuegoHardware
{
    /// <summary>
    /// Gets the graphics display interface
    /// </summary>
    public IPixelDisplay? Display { get; }

    /// <summary>
    /// Gets the right/up button
    /// </summary>
    public PushButton? Right_UpButton { get; }
    /// <summary>
    /// Gets the right/down button
    /// </summary>
    public PushButton? Right_DownButton { get; }
    /// <summary>
    /// Gets the right/left button
    /// </summary>
    public PushButton? Right_LeftButton { get; }
    /// <summary>
    /// Gets the right/right button
    /// </summary>
    public PushButton? Right_RightButton { get; }

    /// <summary>
    /// Gets the left/up button
    /// </summary>
    public PushButton? Left_UpButton { get; }
    /// <summary>
    /// Gets the left/down button
    /// </summary>
    public PushButton? Left_DownButton { get; }
    /// <summary>
    /// Gets the left/left button
    /// </summary>
    public PushButton? Left_LeftButton { get; }
    /// <summary>
    /// Gets the left/right button
    /// </summary>
    public PushButton? Left_RightButton { get; }

    /// <summary>
    /// Gets the start button
    /// </summary>
    public PushButton? StartButton { get; }
    /// <summary>
    /// Gets the select button
    /// </summary>
    public PushButton? SelectButton { get; }

    // Speakers
    /// <summary>
    /// Gets the left speaker
    /// </summary>
    public PiezoSpeaker? LeftSpeaker { get; }
    /// <summary>
    /// Gets the right speaker
    /// </summary>
    public PiezoSpeaker? RightSpeaker { get; }

    /// <summary>
    /// Gets the PWM LED
    /// </summary>
    public PwmLed? BlinkyLed { get; }

    /// <summary>
    /// Gets the motion sensor
    /// </summary>
    public Bmi270? MotionSensor { get; }

    /// <summary>
    /// Gets the display header connector
    /// </summary>
    public DisplayConnector DisplayHeader { get; }

    /// <summary>
    /// Gets the Stemma QT I2C Qwiic connector
    /// </summary>
    public I2cConnector? Qwiic { get; }
}