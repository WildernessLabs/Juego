using Meadow;
using Meadow.Foundation.Audio;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Logging;
using System;

namespace WildernessLabs.Hardware.Juego
{
    /// <summary>
    /// Juego hardware factory class for Juego v1, v2, and v3 hardware
    /// </summary>
    public class Juego
    {
        private Juego() { }

        /// <summary>
        /// Create an instance of the Juego class for the current hardware
        /// </summary>
        public static IJuegoHardware? Create()
        {
            IJuegoHardware? hardware;
            Logger? logger = Resolver.Log;

            logger?.Debug("Initializing Juego...");

            var device = Resolver.Device;

            // make sure not getting instantiated before the App Initialize method
            if (Resolver.Device == null)
            {
                var msg = "Juego instance must be created no earlier than App.Initialize()";
                logger?.Error(msg);
                throw new Exception(msg);
            }

            if (device is IF7FeatherMeadowDevice { } feather)
            {
                logger?.Info("Instantiating Juego v1 hardware");
                hardware = new JuegoHardwareV1(feather);
            }
            else if (device is IF7CoreComputeMeadowDevice { } ccm)
            {
                try
                {
                    // hack for PWM init bug .... move back into the hardware classes once it's fixed
                    var leftSpeaker = new PiezoSpeaker(ccm.Pins.PB8);
                    var rightSpeaker = new PiezoSpeaker(ccm.Pins.PB9);

                    var i2cBus = ccm.CreateI2cBus(busSpeed: Meadow.Hardware.I2cBusSpeed.FastPlus);
                    logger?.Info("I2C Bus instantiated");

                    var mcpVersion = new Mcp23008(i2cBus, address: 0x23);

                    logger?.Trace("McpVersion up");
                    var version = mcpVersion.ReadFromPorts();

                    logger?.Info($"Hardware version is {version}");

                    if (version >= JuegoHardwareV3.MinimumHardareVersion)
                    {
                        logger?.Info("Instantiating Juego v3 hardware");
                        hardware = new JuegoHardwareV3(ccm, i2cBus)
                        {
                            Mcp_VersionInfo = mcpVersion,
                            LeftSpeaker = leftSpeaker,
                            RightSpeaker = rightSpeaker,
                        };
                    }
                    else
                    {
                        logger?.Info("Instantiating Juego v2 hardware");
                        hardware = new JuegoHardwareV2(ccm, i2cBus)
                        {
                            Mcp_VersionInfo = mcpVersion,
                            LeftSpeaker = leftSpeaker,
                            RightSpeaker = rightSpeaker,
                        };
                    }
                }
                catch (Exception e)
                {
                    logger?.Debug($"Failed to create McpVersion: {e.Message}");
                    hardware = null;
                }
            }
            else
            {
                throw new NotSupportedException();
            }

            return hardware;
        }
    }
}