using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Sensors.Hid;

namespace JuegoTester
{
    public class DisplayController
    {
        readonly MicroGraphics graphics;

        public AnalogJoystickPosition AnalogJoystickPosition
        {
            get => analogJoystickPosition;
            set
            {
                analogJoystickPosition = value;
                Update();
            }
        }
        AnalogJoystickPosition analogJoystickPosition;

        public DigitalJoystickPosition DigitalJoystickPosition
        {
            get => digitalJoystickPosition;
            set
            {
                digitalJoystickPosition = value;
                Update();
            }
        }
        DigitalJoystickPosition digitalJoystickPosition;

        public bool UpButtonState
        {
            get => upButtonState;
            set
            {
                upButtonState = value;
                Update();
            }
        }
        bool upButtonState = false;

        public bool DownButtonState
        {
            get => downButtonState;
            set
            {
                downButtonState = value;
                Update();
            }
        }
        bool downButtonState = false;

        public bool LeftButtonState
        {
            get => leftButtonState;
            set
            {
                leftButtonState = value;
                Update();
            }
        }
        bool leftButtonState = false;

        public bool RightButtonState
        {
            get => rightButtonState;
            set
            {
                rightButtonState = value;
                Update();
            }
        }
        bool rightButtonState = false;

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

        bool isUpdating = false;
        bool needsUpdate = false;

        public DisplayController(IGraphicsDisplay display)
        {
            graphics = new MicroGraphics(display)
            {
                Rotation = RotationType._180Degrees,
                CurrentFont = new Font12x16()
            };

            graphics.Clear(true);
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

        void DrawStatus(string label, string value, Color color, int yPosition)
        {
            graphics.DrawText(x: 2, y: yPosition, label, color: color);
            graphics.DrawText(x: 238, y: yPosition, value, alignment: TextAlignment.Right, color: color);
        }

        void Draw()
        {
            graphics.DrawText(x: 2, y: 0, "Hello Juego!", WildernessLabsColors.AzureBlue);

            DrawStatus("Joystick:", $"{digitalJoystickPosition}", Color.White, 40);
            DrawStatus("Horizontal:", $"{analogJoystickPosition.Horizontal:0.00}", Color.White, 60);
            DrawStatus("Vertical:", $"{analogJoystickPosition.Vertical:0.00}", Color.White, 80);



            DrawStatus("Select:", $"{(SelectButtonState ? "pressed" : "released")}", WildernessLabsColors.ChileanFire, 120);
            DrawStatus("Start:", $"{(StartButtonState ? "pressed" : "released")}", WildernessLabsColors.ChileanFire, 140);

            DrawStatus("Up:", $"{(UpButtonState ? "pressed" : "released")}", WildernessLabsColors.ChileanFire, 160);
            DrawStatus("Down:", $"{(DownButtonState ? "pressed" : "released")}", WildernessLabsColors.ChileanFire, 180);
            DrawStatus("Left:", $"{(LeftButtonState ? "pressed" : "released")}", WildernessLabsColors.ChileanFire, 200);
            DrawStatus("Right:", $"{(RightButtonState ? "pressed" : "released")}", WildernessLabsColors.ChileanFire, 220);
        }
    }
}