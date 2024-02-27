using Meadow;
using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;
using Meadow.Units;

namespace Juego_Demo
{
    public class DisplayController
    {
        readonly MicroGraphics graphics;

        public Acceleration3D? Acceleration3D
        {
            get => acceleration3D;
            set
            {
                acceleration3D = value;
                Update();
            }
        }
        Acceleration3D? acceleration3D = null;

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

        public DisplayController(IPixelDisplay display)
        {
            graphics = new MicroGraphics(display)
            {
                Rotation = RotationType._270Degrees,
                IgnoreOutOfBoundsPixels = true,
                CurrentFont = new Font12x20()
            };

            graphics.Clear(Color.YellowGreen);
            graphics.DrawText(0, 0, "Initializing ...", Color.Black);

            graphics.Show();
        }

        public void Update()
        {
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



        void Draw()
        {
            graphics.DrawText(x: graphics.Width / 2, y: 0, "Hello Juego!", WildernessLabsColors.AzureBlue, alignmentH: HorizontalAlignment.Center);

            //D-Pad
            graphics.DrawRectangle(5, 100, 30, 30, WildernessLabsColors.DustyGray, Left_LeftButtonState);
            graphics.DrawRectangle(65, 100, 30, 30, WildernessLabsColors.DustyGray, Left_RightButtonState);
            graphics.DrawRectangle(35, 70, 30, 30, WildernessLabsColors.DustyGray, Left_UpButtonState);
            graphics.DrawRectangle(35, 130, 30, 30, WildernessLabsColors.DustyGray, Left_DownButtonState);
            graphics.DrawCircle(50, 115, 7, WildernessLabsColors.DustyGray, true);

            //Start and Select
            graphics.DrawRoundedRectangle(graphics.Width / 2 - 35, 140, 30, 15, 4, WildernessLabsColors.ChileanFire, SelectButtonState);
            graphics.DrawRoundedRectangle(graphics.Width / 2 + 5, 140, 30, 15, 4, WildernessLabsColors.ChileanFire, StartButtonState);

            //Buttons
            graphics.DrawCircle(graphics.Width - 90, 115, 15, WildernessLabsColors.PearGreen, Right_LeftButtonState);
            graphics.DrawCircle(graphics.Width - 20, 115, 15, WildernessLabsColors.PearGreen, Right_RightButtonState);
            graphics.DrawCircle(graphics.Width - 55, 80, 15, WildernessLabsColors.PearGreen, Right_UpButtonState);
            graphics.DrawCircle(graphics.Width - 55, 150, 15, WildernessLabsColors.PearGreen, Right_DownButtonState);

            //Motion
            if (acceleration3D is { } accel)
            {
                graphics.DrawCircle(graphics.Width / 2, 200, 30, WildernessLabsColors.AzureBlue, false);
                //radius is 30 .... new circle is 10 ... scale position via x & y
                graphics.DrawCircle(graphics.Width / 2 + (int)(accel.X.Gravity * 20), 200 + (int)(accel.Y.Gravity * -20), 10, WildernessLabsColors.AzureBlue, true);
            }
        }
    }
}