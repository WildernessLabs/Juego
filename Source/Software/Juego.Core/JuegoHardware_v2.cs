using System;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Gateways.Bluetooth;
using Meadow.Devices;
using Meadow.Units;

namespace Juego.Core
{
    public class JuegoHardware_v2
    {
        protected F7CoreComputeV2 Device { get; }
        protected IDigitalInputPort McpInterrupt_1 { get; }

        IGraphicsDisplay Display { get; }

        //==== Comms Busses
        protected II2cBus? I2c { get; }
        protected ISpiBus Spi { get; }

        //==== MCP IO Expanders
        public Mcp23008 Mcp_1 { get; protected set; }
        public Mcp23008 Mcp_2 { get; protected set; }
        public Mcp23008 Mcp_VersionInfo { get; protected set; }

        //AnalogJoystick AnalogJoystick { get; }

        //==== Right side buttons
        public PushButton Right_UpButton { get; protected set; }
        public PushButton Right_DownButton { get; protected set; }
        public PushButton Right_LeftButton { get; protected set; }
        public PushButton Right_RightButton { get; protected set; }

        //==== Left side buttons
        public PushButton Left_UpButton { get; protected set; }
        public PushButton Left_DownButton { get; protected set; }
        public PushButton Left_LeftButton { get; protected set; }
        public PushButton Left_RightButton { get; protected set; }

        //==== Start/Select Buttons
        public PushButton StartButton { get; protected set; }
        public PushButton SelectButton { get; protected set; }

        //==== Speakers
        public PiezoSpeaker LeftSpeaker { get; protected set; }
        public PiezoSpeaker RightSpeaker { get; protected set; }

        public JuegoHardware_v2(F7CoreComputeV2 device)
        {
            this.Device = device;

            Console.WriteLine("Initialize hardware...");

            //==== I2C Bus
            Console.WriteLine("Initializing I2C Bus.");
            try
            {
                I2c = Device.CreateI2cBus();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Err initializing I2C Bus: {e.Message}");
            }
            Console.WriteLine("I2C initialized.");

            //==== MCPs
            try
            {
                //McpInterrupt_1 = Device.CreateDigitalInputPort(Device.Pins.D09, InterruptMode.EdgeRising);
                //Mcp_1 = new Mcp23008(I2c, 0x20, McpInterrupt_1);
                Mcp_1 = new Mcp23008(I2c, 0x20);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Err MCP 1: {e.Message}");
            }
            try
            {
                //McpInterrupt_2 = Device.CreateDigitalInputPort(Device.Pins.D10, InterruptMode.EdgeRising);
                //Mcp_2 = new Mcp23008(I2c, 0x20, McpInterrupt_2);
                Mcp_2 = new Mcp23008(I2c, 0x21);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Err MCP 2: {e.Message}");
            }
            try
            {
                Mcp_VersionInfo = new Mcp23008(I2c, 0x26);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Err MCP 3: {e.Message}");
            }

            //==== Speakers
            try {
                LeftSpeaker = new PiezoSpeaker(device, device.Pins.D12);
            }
            catch (Exception e) {
                Console.WriteLine($"Err Left Speaker: {e.Message}");
            }
            try
            {
                LeftSpeaker = new PiezoSpeaker(device, device.Pins.D13);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Err Left Speaker: {e.Message}");
            }


            //==== SPI
            Console.WriteLine("Initializing SPI Bus.");
            try
            {
                Spi = Device.CreateSpiBus(new Frequency(48, Frequency.UnitType.Kilohertz));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Err initializing SPI: {e.Message}");
            }
            Console.WriteLine("SPI initialized.");
        }
    }
}