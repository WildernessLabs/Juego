# Meadow.Juego

**Convenience library for the Meadow Juego handheld game console**

The **Juego** library is included in the **Meadow.Juego** nuget package and is designed for the [Wilderness Labs](www.wildernesslabs.co) Meadow .NET IoT platform.

Juego is an open-source, Meadow-powered, multigame handheld console with dual DPads, stereo speakers, and a color display.

For more information on developing for Meadow, visit [developer.wildernesslabs.co](http://developer.wildernesslabs.co/).

To view all Wilderness Labs open-source projects, including samples, visit [github.com/wildernesslabs](https://github.com/wildernesslabs/).

## Onboard Hardware

| Peripheral | Description |
|---|---|
| ILI9341 | 240x320 color TFT display |
| BMI270 | Inertial measurement unit / accelerometer |
| Left DPad | 4-way directional pad (left side) |
| Right DPad | 4-way directional pad (right side) |
| Select / Start | Classic game console buttons |
| Piezo speakers | Stereo left and right audio transducers |
| USB-C | Build and deploy port |
| Battery slot | Rechargeable battery for portable play |

## Installation

You can install the library from within Visual Studio using the NuGet Package Manager or from the command line using the .NET CLI:

`dotnet add package Meadow.Juego`

## Usage

```csharp
public class MeadowApp : App<F7CoreComputeV2>
{
    IJuegoHardware juego;

    public override Task Initialize()
    {
        juego = Juego.Create();

        if (juego.Display is { } display)
        {
            var graphics = new MicroGraphics(display)
            {
                IgnoreOutOfBoundsPixels = true,
                CurrentFont = new Font12x16()
            };
            graphics.DrawText(10, 10, "Hello, Juego!");
            graphics.Show();
        }

        if (juego.Left_UpButton is { } upButton)
        {
            upButton.PressStarted += (s, e) => Resolver.Log.Info("Up pressed");
        }

        if (juego.SelectButton is { } selectButton)
        {
            selectButton.PressStarted += (s, e) => Resolver.Log.Info("Select pressed");
        }

        if (juego.MotionSensor is { } imu)
        {
            imu.Updated += (s, e) =>
                Resolver.Log.Info($"X:{e.New.Acceleration3D?.X.Gravity:0.0}g");
        }

        return Task.CompletedTask;
    }

    public override async Task Run()
    {
        if (juego.MotionSensor is { } imu)
            imu.StartUpdating(TimeSpan.FromMilliseconds(250));

        if (juego.LeftSpeaker != null)
            await juego.LeftSpeaker.PlayTone(new Frequency(440), TimeSpan.FromMilliseconds(500));

        if (juego.RightSpeaker != null)
            await juego.RightSpeaker.PlayTone(new Frequency(540), TimeSpan.FromMilliseconds(500));

        await Task.Delay(Timeout.Infinite);
    }
}
```

## How to Contribute

- **Found a bug?** [Report an issue](https://github.com/WildernessLabs/Meadow_Issues/issues)
- Have a **feature idea or driver request?** [Open a new feature request](https://github.com/WildernessLabs/Meadow_Issues/issues)
- Want to **contribute code?** Fork the [Juego](https://github.com/WildernessLabs/Juego) repository and submit a pull request against the `develop` branch

## Need Help?

If you have questions or need assistance, please join the Wilderness Labs [community on Slack](http://slackinvite.wildernesslabs.co/).

## About Meadow

Meadow is a complete, IoT platform with defense-grade security that runs full .NET applications on embeddable microcontrollers and Linux single-board computers including Raspberry Pi and NVIDIA Jetson.

### Build

Use the full .NET platform and tooling such as Visual Studio and plug-and-play hardware drivers to painlessly build IoT solutions.

### Connect

Utilize native support for WiFi, Ethernet, and Cellular connectivity to send sensor data to the Cloud and remotely control your peripherals.

### Deploy

Instantly deploy and manage your fleet in the cloud for OtA, health-monitoring, logs, command + control, and enterprise backend integrations.
