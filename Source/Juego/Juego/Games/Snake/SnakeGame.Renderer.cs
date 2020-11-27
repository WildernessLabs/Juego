using System;
using System.Threading;
using Meadow.Foundation.Graphics;

namespace Juego.Games
{
    public partial class SnakeGame
    {
        static int topOffset = 8; //pixels
        static int pixelScale = 3;

        public void Init(GraphicsLibrary gl)
        {
            gl.CurrentFont = new Font4x8();

            gl.Clear();
            gl.DrawText(0, 0, "Meadow Snake");
            gl.DrawText(0, 10, "v0.1.0");
            gl.Show();

            Thread.Sleep(1000);
        }

        public void Update(GraphicsLibrary graphics)
        {
            graphics.Clear();

            Update();

            //draw score and level
            graphics.DrawText(0, 0, $"Score: {Level}");

            //draw border
            graphics.DrawRectangle(0, topOffset, 128, 64 - topOffset);

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