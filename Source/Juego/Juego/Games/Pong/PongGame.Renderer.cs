using System.Threading;
using Meadow.Foundation.Audio;
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

            PongSetup(gl.Width, gl.Height);
        }

        public void Update(IIOConfig ioConfig)
        {
            var gl = ioConfig.Graphics;

            Update();

            gl.Clear();

            gl.DrawCircle(ballX, ballY, ballRadius, true, true);
            gl.DrawRectangle(playerX, playerY, paddleWidth, paddleHeight, true, true);
            gl.DrawRectangle(cpuX, cpuY, paddleWidth, paddleHeight, true, true);

            gl.DrawText(0, 0, $"{playerScore}");
            gl.DrawText((int)(gl.Width), 0, $"{cpuScore}",
                alignment: GraphicsLibrary.TextAlignment.Right);

            gl.Show();
        }
    }
}