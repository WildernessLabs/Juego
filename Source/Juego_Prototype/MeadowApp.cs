using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Juego.Apps;
using Juego.Games;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.TextDisplayMenu;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;

namespace Juego
{
    public class MeadowApp : App<F7FeatherV1>
    {
        Menu menu;

        IIOConfig hardware;

        IGame currentGame;

        const string version = "0.5.4";

        public MeadowApp()
        {
            //settings test
            /*
            Resolver.Log.Info("Settings test");

            int hiscore = Settings.GetInteger("HiScore", 0);
            byte volume = Settings.GetByte("Volume", 5);
            bool playSound = Settings.GetBoolean("PlaySound", false);

            Resolver.Log.Info($"HiScore: {hiscore}");
            Resolver.Log.Info($"Volume: {volume}");
            Resolver.Log.Info($"PlaySound: {playSound}");

            Settings.SetInteger("HiScore", 10000);
            Settings.SetByte("Volume", 9);
            Settings.SetBoolean("PlaySound", true);
            Settings.SaveSettings();

            hiscore = Settings.GetInteger("HiScore", 0);
            volume = Settings.GetByte("Volume", 5);
            playSound = Settings.GetBoolean("PlaySound", false);

            Resolver.Log.Info($"HiScore: {hiscore}");
            Resolver.Log.Info($"Volume: {volume}");
            Resolver.Log.Info($"PlaySound: {playSound}");

            */

            Initialize();

            hardware.rgbLed.SetColor(Color.Green);
            InitMenu();

           //StartGame("startClock");
        }

        void Initialize()
        {
            Resolver.Log.Info("Initialize game hardware...");

            //hardware = new Config_1c_St7735();
            hardware = new Config_proto_Ssd130x_Spi(); //SSD1309 proto 
            //hardware = new Config_1c_Ssd130x_I2c();
            //hardware = new Config_1c_Ssd130x_Spi();
            //hardware = new Config_1a_St7789();
            //hardware = new Config_1c_St7789();

            //hardware = new Config_1c_Ssd1351();

            DrawSplashScreen(hardware.Graphics);

            Resolver.Log.Info("Create buttons...");

            hardware.Up.Clicked += Up_Clicked;
            hardware.Left.Clicked += Left_Clicked;
            hardware.Right.Clicked += Right_Clicked;
            hardware.Down.Clicked += Down_Clicked;

            if (hardware.Select != null) hardware.Select.Clicked += Select_Clicked;
            if (hardware.Start != null) hardware.Start.Clicked += Start_Clicked;
        }

        void DrawSplashScreen(MicroGraphics graphics)
        {
            graphics.Clear();
            graphics.DrawRectangle(0, 0, hardware.Graphics.Width, hardware.Graphics.Height);
            graphics.DrawText(hardware.Graphics.Width / 2, hardware.Graphics.Height / 3, $"Juego v{version}", ScaleFactor.X1, alignmentH: HorizontalAlignment.Center);
            graphics.Show();
        }

        void InitMenu()
        { 
            CreateMenu(hardware.Graphics);

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
            if (menu.IsEnabled)
            {
             //   menu.Back();
            }
            else
            {
                currentGame?.Left();
            }
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
            playGame = false;

            if(menu.IsEnabled == false)
            {
                menu.Enable();
            }
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
                case "startClock":
                    currentGame = new Clock();
                    break;
                case "startSpace":
                    currentGame = new SpaceRaid();
                    break;
                case "startFrogIt":
                    currentGame = new FrogItGame();
                    break;
                case "startPong":
                    currentGame = new PongGame();
                    break;
                case "startSpan4":
                    currentGame = new Span4Game();
                    break;
                case "startSnake":
                    currentGame = new SnakeGame();
                    break;
                case "startTetraminos":
                    currentGame = new TetraminosGame();
                    break;
                default:
                    EnableMenu();
                    return;
            }

            playGame = true;
            currentGame.Init(hardware.Graphics);
            currentGame.Reset();

            await Task.Run(() =>
            {   //full speed today
                while (playGame == true)
                {
                    currentGame.Update(hardware);

                    Thread.Sleep(3);
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
            Resolver.Log.Info("Load menu data...");

            /*
            
            var menuData = LoadResource("menu.json");
            Resolver.Log.Info($"Data length: {menuData.Length}...");
            Resolver.Log.Info("Create menu..."); 
            menu = new Menu(display, menuData, false);
            
            */

            var menuItems = new MenuItem[]
            {
                new MenuItem("SpaceRaid", command: "startSpace"),
                new MenuItem("Clock", command: "startClock"),
                new MenuItem("FrogIt", command: "startFrogIt"),
                new MenuItem("Pong", command: "startPong"),
                new MenuItem("Span4", command: "startSpan4"),
                new MenuItem("Snake", command: "startSnake"),
                new MenuItem("Tetraminos", command: "startTetraminos"),
                new MenuItem("Options", 
                    subItems: new MenuItem[]{new MenuItem("Sound {value}", id: "sound", type: "OnOff", value: true),
                                             new MenuItem("Volume {value}", id: "volume", type: "Numerical", value: 5),
                                             new MenuItem("Clear scores", command: "clearScores"),
                                             new MenuItem($"Version {version}") }),
            };
            

            menu = new Menu(display, menuItems);
            

            menu.Selected += Menu_Selected;
            menu.Exited += Menu_Exited;
        }

        private void Menu_Exited(object sender, EventArgs e)
        {
            Resolver.Log.Info("Menu exited");
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