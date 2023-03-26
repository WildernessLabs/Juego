using Meadow;
using Meadow.Logging;
using System;

namespace WildernessLabs.Hardware.Juego
{
    public class Juego
    {
        private Juego() { }

        /// <summary>
        /// Create an instance of the ProjectLab class
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static IJuegoHardware Create()
        {
            IJuegoHardware hardware;
            Logger? logger = Resolver.Log;

            logger?.Debug("Initializing Juego...");

            var device = Resolver.Device; //convenience local var

            // make sure not getting instantiated before the App Initialize method
            if (Resolver.Device == null)
            {
                var msg = "Juego instance must be created no earlier than App.Initialize()";
                logger?.Error(msg);
                throw new Exception(msg);
            }

            if (device is IF7FeatherMeadowDevice { } feather)
            {
                logger?.Info("Instantiating Jeugo v1 hardware");
                hardware = new JuegoHardwareV1(feather);
            }
            else if (device is IF7CoreComputeMeadowDevice { } ccm)
            {
                logger?.Info("Instantiating Jeugo v2 hardware");
                hardware = new JuegoHardwareV2(ccm);
            }
            else
            {
                throw new NotSupportedException(); //should never get here
            }

            return hardware;
        }
    }
}