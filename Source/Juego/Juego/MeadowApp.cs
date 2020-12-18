using Juego.Games;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Displays.TextDisplayMenu;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors.Buttons;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Juego
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        RgbPwmLed onboardLed;

        Menu menu;

        GraphicsLibrary graphics;
        Ssd1309 ssd1309;

        IButton up = null;
        IButton down = null;
        IButton left = null;
        IButton right = null;
        IButton select = null;

        IGame currentGame;

        public MeadowApp()
        {
            Initialize();

            
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
            onboardLed.SetColor(Color.Red);

            Console.WriteLine("Create display...");

            var config = new SpiClockConfiguration(12000, SpiClockConfiguration.Mode.Mode0);

            var bus = Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config);

            // If display is SPI
            ssd1309 = new Ssd1309
            (
                device: Device,
                spiBus: bus,
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00
            );

            // If display is I2C
            //ssd1309 = new Ssd1309
            //(
            //    i2cBus: Device.CreateI2cBus(),
            //    address: 60
            //);

            ssd1309.IgnoreOutOfBoundsPixels = true;

            Console.WriteLine("Create GraphicsLibrary...");

            graphics = new GraphicsLibrary(ssd1309)
            {
                CurrentFont = new Font8x12(),
            };

            graphics.Clear();
            graphics.DrawRectangle(0, 0, 128, 64);
            graphics.DrawText(64, 26, "Juego v0.2", GraphicsLibrary.ScaleFactor.X1, GraphicsLibrary.TextAlignment.Center);
            graphics.Show();

            Console.WriteLine("Create buttons...");

            up = new PushButton(Device, Device.Pins.D09, ResistorMode.PullUp);
            up.Clicked += Up_Clicked;

            left = new PushButton(Device, Device.Pins.D10, ResistorMode.PullUp);
            left.Clicked += Left_Clicked;

            right = new PushButton(Device, Device.Pins.D04, ResistorMode.PullUp);
            right.Clicked += Right_Clicked;

            down = new PushButton(Device, Device.Pins.D03, ResistorMode.PullUp);
            down.Clicked += Down_Clicked;

            select = new PushButton(Device, Device.Pins.D06, ResistorMode.PullUp);
            select.Clicked += Select_Clicked; ;

            onboardLed.SetColor(Color.Green);
        }

        void InitMenu()
        { 
            CreateMenu(graphics);

            menu.Enable();
        }

        void Up_Clicked(object sender, EventArgs e)
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

        void Left_Clicked(object sender, EventArgs e)
        {
            currentGame?.Left();
        }

        void Right_Clicked(object sender, EventArgs e)
        {
            currentGame?.Right();
        }

        void Down_Clicked(object sender, EventArgs e)
        {
            if (menu.IsEnabled)
            {
                menu.Next();
            }
            else
            {
                currentGame?.Down();
            }
        }

        void Select_Clicked(object sender, EventArgs e)
        {
            menu.Select();
        }

        void Menu_Selected(object sender, MenuSelectedEventArgs e)
        {
            DisableMenu();

            var t = StartGame(e.Command);
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