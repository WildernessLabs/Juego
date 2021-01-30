using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Juego.Games;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Displays.TextDisplayMenu;
using Meadow.Foundation.Displays.Tft;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;

namespace Juego
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbPwmLed onboardLed;

        Menu menu;

        GraphicsLibrary graphics;
        St7789 display;

        int dW = 240;
        int dH = 240;

        IButton up = null;
        IButton down = null;
        IButton left = null;
        IButton right = null;
        IButton select = null;
        IButton start = null;

        IGame currentGame;

        public MeadowApp()
        {
            Initialize();

            onboardLed.SetColor(Color.Green);
            InitMenu();
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);

            Console.WriteLine("Create display...");

            var config = new SpiClockConfiguration(48000, SpiClockConfiguration.Mode.Mode3);
            var bus = Device.CreateSpiBus(IODeviceMap.Display.ClockPin, IODeviceMap.Display.CopiPin,
                IODeviceMap.Display.CipoPin, config);

            display = new St7789(
                device: Device, spiBus: bus,
                chipSelectPin: IODeviceMap.Display.CSPin,
                dcPin: IODeviceMap.Display.DCPin,
                resetPin: IODeviceMap.Display.ResetPin,
                width: (uint)dW,
                height: (uint)dH,
                displayColorMode: DisplayBase.DisplayColorMode.Format12bppRgb444
            );
            display.IgnoreOutOfBoundsPixels = true;

            Console.WriteLine("Create GraphicsLibrary...");

            graphics = new GraphicsLibrary(display) {
                CurrentFont = new Font8x12(),
                Rotation = GraphicsLibrary.RotationType._90Degrees,
            };

            graphics.Clear();
            graphics.DrawRectangle(0, 0, 128, 64);
            graphics.DrawText(64, 26, "Juego v0.2", GraphicsLibrary.ScaleFactor.X1, GraphicsLibrary.TextAlignment.Center);
            graphics.Show();

            Console.WriteLine("Create buttons...");

            up = new PushButton(Device, IODeviceMap.Buttons.UpPin);
            up.Clicked += Up_Clicked;

            left = new PushButton(Device, IODeviceMap.Buttons.LeftPin);
            left.Clicked += Left_Clicked;

            right = new PushButton(Device, IODeviceMap.Buttons.RightPin);
            right.Clicked += Right_Clicked;

            down = new PushButton(Device, IODeviceMap.Buttons.DownPin);
            down.Clicked += Down_Clicked;

            //select = new PushButton(Device, IODeviceMap.Buttons.SelectPin);
            //select.Clicked += Select_Clicked;

            //start = new PushButton(Device, IODeviceMap.Buttons.StartPin);
            //start.Clicked += Start_Clicked;
        }

        void InitMenu()
        { 
            CreateMenu(graphics);

            menu.Enable();
        }

        private void Down_Clicked(object sender, EventArgs e)
        {
            if (menu.IsEnabled) { menu.Next(); }
            else
            {
                currentGame?.Down();
            }
        }

        private void Right_Clicked(object sender, EventArgs e)
        {
            if (menu.IsEnabled)
            {
                menu.Select();
            }
            else
            {
                currentGame?.Right();
            }
        }

        private void Left_Clicked(object sender, EventArgs e)
        {
            currentGame?.Left();
        }

        private void Up_Clicked(object sender, EventArgs e)
        {
            if (menu.IsEnabled)
            {
                menu.Previous();
            }
            else
            {
                currentGame?.Up();
            }
        }

        private void Select_Clicked(object sender, EventArgs e)
        {
            if (menu.IsEnabled) { menu.Next(); }
        }

        private void Start_Clicked(object sender, EventArgs e)
        {
            if (menu.IsEnabled) {
                menu.Select();
            }
        }


        bool playGame = false;
        async Task StartGame(string command)
        {
            switch (command)
            {
                case "startFrogger":
                    currentGame = new FroggerGame();
                    break;
                case "startPong":
                    currentGame = new PongGame();
                    break;
                case "startSpan4":
                    currentGame = new Span4Game();
                    break;
                case "startSnake":
                    currentGame = new SnakeGame(42, 18);
                    break;
                case "startTetraminos":
                    currentGame = new TetraminosGame();
                    break;
                default:
                    EnableMenu();
                    return;
            }

            playGame = true;
            currentGame.Init(graphics);
            currentGame.Reset();

            await Task.Run(() =>
            {   //full speed today
                while (playGame == true)
                {
                    currentGame.Update(graphics);
                }
            });
        }

        void EnableMenu()
        {
            menu?.Enable();
        }

        void DisableMenu()
        {
            menu?.Disable();
        }

        void CreateMenu(ITextDisplay display)
        {
            Console.WriteLine("Load menu data...");

            /*   var menuData = LoadResource("menu.json");
               Console.WriteLine($"Data length: {menuData.Length}...");
               Console.WriteLine("Create menu..."); 
               menu = new Menu(display, menuData, false); */

            var menuItems = new MenuItem[]
            {
                new MenuItem("Frogger", command: "startFrogger"),
                new MenuItem("Pong", command: "startPong"),
                new MenuItem("Span4", command: "startSpan4"),
                new MenuItem("Snake", command: "startSnake"),
                new MenuItem("Tetraminos", command: "startTetraminos"),
            };

            menu = new Menu(display, menuItems);

            menu.Selected += Menu_Selected;
        }

        private void Menu_Selected(object sender, MenuSelectedEventArgs e)
        {
            DisableMenu();

            var t = StartGame(e.Command);
        }

        byte[] LoadResource(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"Juego.{filename}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }
}
    }