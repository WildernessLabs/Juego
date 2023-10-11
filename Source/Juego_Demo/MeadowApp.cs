using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Audio;
using Meadow.Units;
using System;
using System.Threading.Tasks;
using WildernessLabs.Hardware.Juego;

namespace Juego_Demo
{
    public class MeadowApp : App<F7CoreComputeV2>
    {
        private IJuegoHardware juego;
        private DisplayController displayController;
        private MicroAudio audioLeft, audioRight;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize");

            juego = Juego.Create();

            if (juego.Display is { } display)
            {
                displayController = new DisplayController(display);
            }

            //---- BMI270 Accel/IMU
            if (juego.MotionSensor is { } bmi270)
            {
                Resolver.Log.Info("Found BMI270");
                bmi270.Updated += Bmi270Updated;
            }

            if (juego.Left_LeftButton is { } leftDpad)
            {
                leftDpad.PressStarted += (s, e) => displayController.Left_LeftButtonState = true;
                leftDpad.PressEnded += (s, e) => displayController.Left_LeftButtonState = false;
            }
            if (juego.Left_RightButton is { } rightDpad)
            {
                rightDpad.PressStarted += (s, e) => displayController.Left_RightButtonState = true;
                rightDpad.PressEnded += (s, e) => displayController.Left_RightButtonState = false;
            }
            if (juego.Left_UpButton is { } upDpad)
            {
                upDpad.PressStarted += (s, e) => displayController.Left_UpButtonState = true;
                upDpad.PressEnded += (s, e) => displayController.Left_UpButtonState = false;
            }
            if (juego.Left_DownButton is { } downDpad)
            {
                downDpad.PressStarted += (s, e) => displayController.Left_DownButtonState = true;
                downDpad.PressEnded += (s, e) => displayController.Left_DownButtonState = false;
            }

            if (juego.Right_LeftButton is { } leftButton)
            {
                leftButton.PressStarted += (s, e) => displayController.Right_LeftButtonState = true;
                leftButton.PressEnded += (s, e) => displayController.Right_LeftButtonState = false;
            }
            if (juego.Right_RightButton is { } rightButton)
            {
                rightButton.PressStarted += (s, e) => displayController.Right_RightButtonState = true;
                rightButton.PressEnded += (s, e) => displayController.Right_RightButtonState = false;
            }
            if (juego.Right_UpButton is { } upButton)
            {
                upButton.PressStarted += (s, e) => displayController.Right_UpButtonState = true;
                upButton.PressEnded += (s, e) => displayController.Right_UpButtonState = false;
            }
            if (juego.Right_DownButton is { } downButton)
            {
                downButton.PressStarted += (s, e) => displayController.Right_DownButtonState = true;
                downButton.PressEnded += (s, e) => displayController.Right_DownButtonState = false;
            }

            if (juego.SelectButton is { } selectButton)
            {
                selectButton.PressStarted += (s, e) => displayController.SelectButtonState = true;
                selectButton.PressEnded += (s, e) => displayController.SelectButtonState = false;
            }
            if (juego.StartButton is { } startButton)
            {
                startButton.PressStarted += (s, e) => displayController.StartButtonState = true;
                startButton.PressEnded += (s, e) => displayController.StartButtonState = false;
            }

            audioLeft = new MicroAudio(juego.LeftSpeaker);
            audioRight = new MicroAudio(juego.RightSpeaker);

            return Task.CompletedTask;
        }

        public async override Task Run()
        {
            Resolver.Log.Info("Run...");

            displayController?.Update();
            juego.MotionSensor?.StartUpdating(TimeSpan.FromMilliseconds(500));

            await audioLeft.PlaySystemSound(SystemSoundEffect.PowerUp);
            await audioRight.PlayGameSound(GameSoundEffect.LevelComplete);

            return;
        }

        private void Bmi270Updated(object sender, IChangeResult<(Acceleration3D? Acceleration3D, AngularVelocity3D? AngularVelocity3D, Temperature? Temperature)> e)
        {
            Resolver.Log.Info($"BMI270: X:{e.New.Acceleration3D.Value.X.Gravity:0.0}g, Y:{e.New.Acceleration3D.Value.Y.Gravity:0.0}g, Z:{e.New.Acceleration3D.Value.Z.Gravity:0.0}g");

            if (displayController != null)
            {
                displayController.Acceleration3D = e.New.Acceleration3D;
            }
        }
    }
}