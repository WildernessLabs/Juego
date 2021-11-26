using System;
using System.Threading;
using Meadow.Foundation.Graphics;

namespace Juego.Games
{
    public partial class PongGame
    {
        public void Init(MicroGraphics gl)
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

            Console.WriteLine("Show start");

            Update();

            //Console.WriteLine($"CPU {cpuX}, {cpuY}, {paddleWidth}, {paddleHeight}");

            gl.Clear();

            gl.DrawCircle(ballX, ballY, ballRadius, true, true);
            gl.DrawRectangle(playerX, playerY, paddleWidth, paddleHeight, true, true);
            gl.DrawRectangle(cpuX, cpuY, paddleWidth, paddleHeight, true, true);

            Console.WriteLine("Assets drawn");

            gl.DrawText(0, 0, $"{playerScore}");
            gl.DrawText(gl.Width, 0, $"{cpuScore}",
                alignment: TextAlignment.Right);

            gl.Show();

            Console.WriteLine("Show complete");
        }
    }
}