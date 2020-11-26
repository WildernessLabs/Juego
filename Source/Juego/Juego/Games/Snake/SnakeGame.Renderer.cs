using System;
using System.Threading;
using Meadow.Foundation.Graphics;

namespace Juego.Games
{
    public partial class SnakeGame
    {
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
          //  var level = $"Level: {Level}";
            graphics.DrawText(0, 0, $"Score: {Level}");
          //  graphics.DrawText(BoardWidth - level.Length * 4, 0, level);

            //draw border
            graphics.DrawRectangle(0, topOffset, 128, 64 - topOffset);

            //draw food
            graphics.DrawPixel(FoodPosition.X, FoodPosition.Y);

            //draw food
            for (int i = 0; i < SnakePosition.Count; i++)
            {
                var point = (Point)SnakePosition[i];

                graphics.DrawPixel(point.X, point.Y);
            }

          //  if (PlaySound)
          //      speaker.PlayTone(440, 25);

            //show
            graphics.Show();

            Thread.Sleep(Math.Max(250 - Level * 10, 0));
        }
    }
}