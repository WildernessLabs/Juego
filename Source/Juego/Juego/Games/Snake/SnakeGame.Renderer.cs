using System;
using System.Threading;
using Meadow.Foundation.Graphics;

namespace Juego.Games
{
    public partial class SnakeGame
    {
        int topOffset = 8; //pixels
        int pixelScale = 1;

        public void Init(MicroGraphics gl)
        {
            if(gl.Height <= 64)
            {
                gl.CurrentFont = new Font4x8();
                topOffset = 8; //pixels
                pixelScale = 3;
            }
            else
            {
                gl.CurrentFont = new Font8x12();
                topOffset = 12; //pixels
                pixelScale = 4;
            }

            gl.Clear();
            gl.DrawText(0, 0, "Meadow Snake");
            gl.DrawText(0, 10, "v0.2.0");
            gl.Show();

            Thread.Sleep(1000);


            BoardWidth = gl.Width / pixelScale;
            BoardHeight = (gl.Height - topOffset) / pixelScale;

            Reset();
        }

        public void Update(IIOConfig ioConfig)
        {
            var graphics = ioConfig.Graphics;

            graphics.Clear();

            Update();

            //draw score and level
            graphics.DrawText(0, 0, $"Score: {Level}");

            //draw border
            graphics.DrawRectangle(0, topOffset, graphics.Width, graphics.Height - topOffset);

            //draw food
            graphics.DrawRectangle(FoodPosition.X * pixelScale + 1,
                FoodPosition.Y * pixelScale + topOffset + 1,
                pixelScale, pixelScale);

            //draw snake
            for (int i = 0; i < SnakePosition.Count; i++)
            {
                var point = (Point)SnakePosition[i];

                graphics.DrawRectangle(point.X * pixelScale + 1,
                    point.Y * pixelScale + topOffset + 1,
                    pixelScale, pixelScale, true, true);
            }

          //  if (PlaySound)
          //      speaker.PlayTone(440, 25);

            //show
            graphics.Show();

            Thread.Sleep(Math.Max(250 - Level * 10, 0));
        }
    }
}