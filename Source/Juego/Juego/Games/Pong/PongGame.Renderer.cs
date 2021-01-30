using System.Threading;
using Meadow.Foundation.Graphics;

namespace Juego.Games
{
    public partial class PongGame
    {
        public void Init(GraphicsLibrary gl)
        {
            if(gl.Height <= 64)
            {
                gl.CurrentFont = new Font8x12();
            }
            else
            {
                gl.CurrentFont = new Font12x16();
            }
            
            gl.Clear();
            gl.DrawText(0, 0, "Meadow Pong");
            gl.DrawText(0, 16, "v0.2.0");
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
            graphics.DrawText((int)(graphics.Width), 0, $"{cpuScore}",
                alignment: GraphicsLibrary.TextAlignment.Right);

            graphics.Show();
        }
    }
}