using Meadow;
using Meadow.Foundation.Audio;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Hardware;
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
            IJuegoHardware? hardware = null;
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
                II2cBus i2cBus;
                Mcp23008? mcpVersion = null;
                byte version = 0;

                PiezoSpeaker? leftSpeaker = null;
                PiezoSpeaker? rightSpeaker = null;

                try
                {
                    logger?.Info("Instantiating speakers");
                    // hack for PWM init bug .... move back into the hardware classes once it's fixed
                    leftSpeaker = new PiezoSpeaker(ccm.Pins.PB8);
                    rightSpeaker = new PiezoSpeaker(ccm.Pins.PB9);
                }
                catch
                {
                    logger?.Info("Failed to instantiate speakers");
                }

                try
                {
                    logger?.Info("Intantiating I2C Bus");
                    i2cBus = ccm.CreateI2cBus(busSpeed: I2cBusSpeed.FastPlus);
                }
                catch
                {
                    logger?.Info("Failed to instantiate I2C Bus");
                    logger?.Info("Cannot instantiate Juego hardware");
                    return null;
                }

                try
                {
                    logger?.Info("Intantiating version MCP23008");
                    mcpVersion = new Mcp23008(i2cBus, address: 0x23);
                    version = mcpVersion.ReadFromPorts();
                }
                catch
                {
                    logger?.Info("Failed to instantiate version MCP23008");
                }

                try
                {
                    if (mcpVersion != null &&
                        version >= JuegoHardwareV3.MinimumHardareVersion)
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
                            Mcp_VersionInfo = mcpVersion!,
                            LeftSpeaker = leftSpeaker,
                            RightSpeaker = rightSpeaker,
                        };
                    }
                }
                catch
                {
                    logger?.Info("Failed to instantiate Juego hardware");
                }
            }

            return hardware;
        }
    }
}