using System;
using System.Threading;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Juego.Core;
using Meadow.Units;

namespace Juego_Demo
{
	// Change F7FeatherV2 to F7FeatherV1 for V1.x boards
	public class MeadowApp : App<F7CoreComputeV2>
	{
        JuegoHardware_v2 hardware;

        DisplayController displayController;

        public override Task Initialize()
        {
            Console.WriteLine("Initialize");

            hardware = new JuegoHardware_v2(Device);

            if (hardware.Display is { } display)
            {
                displayController = new DisplayController(display);
            }

            //---- buttons
            if (hardware.Left_LeftButton is { } leftDpad)
            {
                leftDpad.PressStarted += (s, e) => displayController.Left_LeftButtonState = true;
                leftDpad.PressEnded += (s, e) => displayController.Left_LeftButtonState = false;
            }

            if (hardware.Left_RightButton is { } rightDpad)
            {
                rightDpad.PressStarted += (s, e) => displayController.Left_RightButtonState = true;
                rightDpad.PressEnded += (s, e) => displayController.Left_RightButtonState = false;
            }

            if (hardware.Left_UpButton is { } upDpad)
            {
                upDpad.PressStarted += (s, e) => displayController.Left_UpButtonState = true;
                upDpad.PressEnded += (s, e) => displayController.Left_UpButtonState = false;
            }

            if (hardware.Left_DownButton is { } downDpad)
            {
                downDpad.PressStarted += (s, e) => displayController.Left_DownButtonState = true;
                downDpad.PressEnded += (s, e) => displayController.Left_DownButtonState = false;
            }

            if (hardware.Right_LeftButton is { } leftButton)
            {
                leftButton.PressStarted += (s, e) => displayController.Right_LeftButtonState = true;
                leftButton.PressEnded += (s, e) => displayController.Right_LeftButtonState = false;
            }

            if (hardware.Right_RightButton is { } rightButton)
            {
                rightButton.PressStarted += (s, e) => displayController.Right_RightButtonState = true;
                rightButton.PressEnded += (s, e) => displayController.Right_RightButtonState = false;
            }

            if (hardware.Right_UpButton is { } upButton)
            {
                upButton.PressStarted += (s, e) => displayController.Right_UpButtonState = true;
                upButton.PressEnded += (s, e) => displayController.Right_UpButtonState = false;
            }

            if (hardware.Right_DownButton is { } downButton)
            {
                downButton.PressStarted += (s, e) => displayController.Right_DownButtonState = true;
                downButton.PressEnded += (s, e) => displayController.Right_DownButtonState = false;
            }

            if (hardware.SelectButton is { } selectButton)
            {
                selectButton.PressStarted += (s, e) => displayController.SelectButtonState = true;
                selectButton.PressEnded += (s, e) => displayController.SelectButtonState = false;
            }

            if (hardware.StartButton is { } startButton)
            {
                startButton.PressStarted += (s, e) => displayController.StartButtonState = true;
                startButton.PressEnded += (s, e) => displayController.StartButtonState = false;
            }

            return base.Initialize();
        }

        public async override Task Run()
        {
            Console.WriteLine("Run...");

            if (displayController != null)
            {
                displayController.Update();
            }

            for (int i = 0; i < 5; i++)
            {
                await hardware.LeftSpeaker.PlayTone(new Frequency(440), TimeSpan.FromMilliseconds(500));
                await hardware.RightSpeaker.PlayTone(new Frequency(540), TimeSpan.FromMilliseconds(500));
            }

            return;
        }
    }
}
