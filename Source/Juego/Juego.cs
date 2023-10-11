using Meadow;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Hardware;
using Meadow.Logging;
using System;

namespace WildernessLabs.Hardware.Juego
{
    public class Juego
    {
        private Juego() { }

        /// <summary>
        /// Create an instance of the Juego class
        /// </summary>
        public static IJuegoHardware Create()
        {
            IJuegoHardware hardware;
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

            I32PinFeatherBoardPinout pins = device switch
            {
                IF7FeatherMeadowDevice f => f.Pins,
                IF7CoreComputeMeadowDevice c => c.Pins,
                _ => throw new NotSupportedException("Device must be a Feather F7 or F7 Core Compute module"),
            };

            if (device is IF7FeatherMeadowDevice { } feather)
            {
                logger?.Info("Instantiating Juego v1 hardware");
                hardware = new JuegoHardwareV1(feather);
            }
            else if (device is IF7CoreComputeMeadowDevice { } ccm)
            {
                var i2cBus = device.CreateI2cBus(busSpeed: I2cBusSpeed.FastPlus);
                logger?.Info("I2C Bus instantiated");

                try
                {
                    var mcp1 = new Mcp23008(i2cBus, address: 0x23);

                    var version = mcp1.ReadFromPorts();

                    logger?.Trace("McpVersion up");
                    logger?.Info($"Hardware version is {version}");

                    if (version > 3)
                    {
                        logger?.Info("Instantiating Juego v3 hardware");
                        hardware = new JuegoHardwareV3(ccm, i2cBus);
                    }
                    else
                    {
                        logger?.Info("Instantiating Juego v2 hardware");
                        hardware = new JuegoHardwareV2(ccm, i2cBus);
                    }

                }
                catch (Exception e)
                {
                    logger?.Debug($"Failed to create McpVersion: {e.Message}, could be a v2 board");

                    logger?.Info("Instantiating Juego v2 hardware");
                    hardware = new JuegoHardwareV2(ccm, i2cBus);
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