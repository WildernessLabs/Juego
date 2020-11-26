using System.Threading;
using Meadow.Foundation.Graphics;

namespace Juego.Games
{
    public partial class PongGame
    {
        public void Init(GraphicsLibrary gl)
        {
            gl.CurrentFont = new Font8x12();

            gl.Clear();
            gl.DrawText(0, 0, "Meadow Pong");
            gl.DrawText(0, 12, "v0.1.0");
            gl.Show();

            Thread.Sleep(1000);
        }

        public void Update(GraphicsLibrary graphics)
        {
            Update();

            graphics.Clear();

            graphics.DrawCircle(ballX, ballY, ballRadius, true, true);
            graphics.DrawRectangle(playerX, playerY, paddleWidth, paddleHeight, true, true);
            graphics.DrawRectangle(cpuX, cpuY, paddleWidth, paddleHeight, true, true);

            graphics.DrawText(0, 0, $"{playerScore}");
            graphics.DrawText(128, 0, $"{cpuScore}",
                alignment: GraphicsLibrary.TextAlignment.Right);

            graphics.Show();
        }
    }
}
