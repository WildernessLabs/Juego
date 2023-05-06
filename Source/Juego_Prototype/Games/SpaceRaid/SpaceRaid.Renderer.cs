using Meadow.Foundation.Graphics;
using System;

namespace Juego.Games
{
    public partial class SpaceRaid : IGame
    {
        int[] leftWall = new int[9];
        int[] rightWall = new int[9];

        int offset = 0;


        public void Init(MicroGraphics gl)
        {

        }

        public void Update(IIOConfig ioConfig)
        {
            var gl = ioConfig.Graphics;

            gl.Clear();

            gl.DrawText(0, 0, "Space");
            gl.DrawLine(0, 0, 10, 10, true);

            gl.Show();
        }
    }
}
