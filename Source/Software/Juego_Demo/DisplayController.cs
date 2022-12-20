using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using System;

namespace Juego_Demo
{
    public class DisplayController
    {
        readonly MicroGraphics graphics;

        public bool Right_UpButtonState
        {
            get => right_upButtonState;
            set
            {
                right_upButtonState = value;
                Update();
            }
        }
        bool right_upButtonState = false;

        public bool Right_DownButtonState
        {
            get => right_downButtonState;
            set
            {
                right_downButtonState = value;
                Update();
            }
        }
        bool right_downButtonState = false;

        public bool Right_LeftButtonState
        {
            get => right_leftButtonState;
            set
            {
                right_leftButtonState = value;
                Update();
            }
        }
        bool right_leftButtonState = false;

        public bool Right_RightButtonState
        {
            get => right_rightButtonState;
            set
            {
                right_rightButtonState = value;
                Update();
            }
        }
        bool right_rightButtonState = false;

        public bool Left_UpButtonState
        {
            get => left_upButtonState;
            set
            {
                left_upButtonState = value;
                Update();
            }
        }
        bool left_upButtonState = false;

        public bool Left_DownButtonState
        {
            get => left_downButtonState;
            set
            {
                left_downButtonState = value;
                Update();
            }
        }
        bool left_downButtonState = false;

        public bool Left_LeftButtonState
        {
            get => left_leftButtonState;
            set
            {
                left_leftButtonState = value;
                Update();
            }
        }
        bool left_leftButtonState = false;

        public bool Left_RightButtonState
        {
            get => left_rightButtonState;
            set
            {
                left_rightButtonState = value;
                Update();
            }
        }
        bool left_rightButtonState = false;

        public bool StartButtonState
        {
            get => startButtonState;
            set
            {
                startButtonState = value;
                Update();
            }
        }
        bool startButtonState = false;

        public bool SelectButtonState
        {
            get => selectButtonState;
            set
            {
                selectButtonState = value;
                Update();
            }
        }
        bool selectButtonState = false;

        bool isUpdating = false;
        bool needsUpdate = false;

        public DisplayController(IGraphicsDisplay display)
        {
            Console.WriteLine("Display controller ctor");

            graphics = new MicroGraphics(display)
            {
                Rotation = RotationType._270Degrees,
                IgnoreOutOfBoundsPixels = true,
                CurrentFont = new Font12x16()
            };

            graphics.Clear(Color.YellowGreen);
            graphics.DrawText(0, 0, "Initializing ...", Color.Black);

            graphics.Show();
        }

        public void Update()
        {
            Console.WriteLine("Update");

            if (isUpdating)
            {   //queue up the next update
                needsUpdate = true;
                return;
            }

            isUpdating = true;

            graphics.Clear();
            Draw();
            graphics.Show();

            isUpdating = false;

            if (needsUpdate)
            {
                needsUpdate = false;
                Update();
            }
        }

        void DrawStatus(string label, string value, Color color, int yPosition)
        {
            graphics.DrawText(x: 2, y: yPosition, label, color: color);
            graphics.DrawText(x: 318, y: yPosition, value, alignmentH: HorizontalAlignment.Right, color: color);
        }

        void Draw()
        {
            graphics.DrawText(x: 2, y: 0, "Hello Juego!", WildernessLabsColors.AzureBlue);

            DrawStatus("Up D-pad:", $"{(Left_UpButtonState ? "pressed" : "released")}", WildernessLabsColors.ChileanFire, 60);
            DrawStatus("Down D-pad:", $"{(Left_DownButtonState ? "pressed" : "released")}", WildernessLabsColors.ChileanFire, 80);
            DrawStatus("Left D-pad:", $"{(Left_LeftButtonState ? "pressed" : "released")}", WildernessLabsColors.ChileanFire, 100);
            DrawStatus("Right D-pad:", $"{(Left_RightButtonState ? "pressed" : "released")}", WildernessLabsColors.ChileanFire, 120);


            DrawStatus("Up button:", $"{(Right_UpButtonState ? "pressed" : "released")}", WildernessLabsColors.ChileanFire, 160);
            DrawStatus("Down button:", $"{(Right_DownButtonState ? "pressed" : "released")}", WildernessLabsColors.ChileanFire, 180);
            DrawStatus("Left button:", $"{(Right_LeftButtonState ? "pressed" : "released")}", WildernessLabsColors.ChileanFire, 200);
            DrawStatus("Right button:", $"{(Right_RightButtonState ? "pressed" : "released")}", WildernessLabsColors.ChileanFire, 220);
        }
    }
}