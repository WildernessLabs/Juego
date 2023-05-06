using System;
using Meadow.Foundation;
using Meadow.Foundation.Audio;
using Meadow.Foundation.Graphics;

namespace Juego.Games
{
    public partial class LanderGame
    {
        readonly byte cellSize = 8;

        DrawPixelDel DrawPixel;

        public void Init(MicroGraphics gl)
        {
            gl.Clear();
            gl.DrawText(0, 0, "Meadow Lander");
            gl.DrawText(0, 16, "v0.0.1");
            gl.Show();
        }

        public void Update(IIOConfig ioConfig)
        {
            var gl = ioConfig.Graphics;

            Update();

            gl.Clear();
            DrawBackground(gl);

            // DrawLives();
            gl.Show();
        }

        void DrawBackground(MicroGraphics graphics)
        {
          
        }

        void DrawLives(MicroGraphics graphics)
        {

        }

        delegate void DrawPixelDel(int x, int y, bool colored, MicroGraphics graphics, Color color);

        void DrawPixel1x(int x, int y, bool colored, MicroGraphics graphics, Color color)
        {
            graphics.DrawPixel(x, y, colored?color:Color.Black);
        }

        void DrawPixel2x(int x, int y, bool colored, MicroGraphics graphics)
        {
            x *= 2;
            y *= 2;

            graphics.DrawPixel(x, y, colored);
            graphics.DrawPixel(x + 1, y, colored);
            graphics.DrawPixel(x, y + 1, colored);
            graphics.DrawPixel(x + 1, y + 1, colored);
        }

        void DrawBitmap(int x, int y, int width, int height, byte[] bitmap, MicroGraphics graphics, Color color)
        {
            for (var ordinate = 0; ordinate < height; ordinate++) //y
            {
                for (var abscissa = 0; abscissa < width; abscissa++) //x
                {
                    var b = bitmap[(ordinate * width) + abscissa];
                    byte mask = 0x01;

                    for (var pixel = 0; pixel < 8; pixel++)
                    {
                        DrawPixel(x + (8 * abscissa) + 7 - pixel, y + ordinate, (b & mask) > 0, graphics, color);

                        mask <<= 1;
                    }
                }
            }
        }
    }
}