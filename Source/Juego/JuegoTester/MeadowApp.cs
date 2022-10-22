using JuegoTester;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace JuegoTester
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7FeatherV1>
    {
        DisplayController displayController;
        IHardwareConfig hardware;

        public override Task Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            // get the correct hardware config depending on board version
            hardware = new JuegoV1a_Hardware();
      
            // Initialize the board specific hardware
            hardware.Initialize(Device);

            displayController = new DisplayController(hardware.Display);

            //---- Joystick
            if (hardware.AnalogJoystick is { } joystick)
            {
                joystick.Updated += JoystickUpdated;
                joystick.StartUpdating(TimeSpan.FromSeconds(0.25));
            }

            //---- buttons
            hardware.StartButton.PressStarted += (s, e) => {

                Console.WriteLine("Start Button press started");
                displayController.StartButtonState = true;
            };

            hardware.SelectButton.PressStarted += (s, e) => {

                Console.WriteLine("Select Button press started");
                displayController.SelectButtonState = true;
            };

            hardware.LeftButton.PressStarted += (s, e) => {

                Console.WriteLine("Left Button press started");
                displayController.LeftButtonState = true;
            };

            hardware.LeftButton.PressEnded += (s, e) =>
            {
                Console.WriteLine("Left Button press ended");
                displayController.LeftButtonState = false;
            };

            hardware.StartButton.PressStarted += (s, e) => displayController.StartButtonState = true;
            hardware.StartButton.PressEnded += (s, e) => displayController.StartButtonState = false;

            hardware.SelectButton.PressStarted += (s, e) => displayController.SelectButtonState = true;
            hardware.SelectButton.PressEnded += (s, e) => displayController.SelectButtonState = false;

            hardware.LeftButton.PressStarted += (s, e) => displayController.LeftButtonState = true;
            hardware.LeftButton.PressEnded += (s, e) => displayController.LeftButtonState = false;

            hardware.RightButton.PressStarted += (s, e) => displayController.RightButtonState = true;
            hardware.RightButton.PressEnded += (s, e) => displayController.RightButtonState = false;

            hardware.UpButton.PressStarted += (s, e) => displayController.UpButtonState = true;
            hardware.UpButton.PressEnded += (s, e) => displayController.UpButtonState = false;

            hardware.DownButton.PressStarted += (s, e) => displayController.DownButtonState = true;
            hardware.DownButton.PressEnded += (s, e) => displayController.DownButtonState = false;
 
            //---- heartbeat
            hardware.OnboardLed.StartPulse(WildernessLabsColors.PearGreen);

            Console.WriteLine("Initialization complete");

            return base.Initialize();
        }

        private void JoystickUpdated(object sender, IChangeResult<Meadow.Peripherals.Sensors.Hid.AnalogJoystickPosition> e)
        {

            Console.WriteLine($"Joystick: {e.New.Horizontal:0.0},{e.New.Vertical:0.0}");

            displayController.AnalogJoystickPosition = e.New;
            displayController.DigitalJoystickPosition = hardware.AnalogJoystick.DigitalPosition.Value;
        }

        public override Task Run()
        {
            Console.WriteLine("Run...");

            displayController.Update();

            Console.WriteLine("starting blink");
            hardware.OnboardLed.StartBlink(WildernessLabsColors.PearGreen, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(2000), 0.5f);

            return base.Run();
        }
    }
}