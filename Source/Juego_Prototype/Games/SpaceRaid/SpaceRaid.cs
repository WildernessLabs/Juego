using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juego.Games
{
    public partial class SpaceRaid : IGame
    {
        public void Down()
        {
         
        }

        public void Left()
        {
         
        }

        public void Reset()
        {
         
            offset = 0;

            for(int i = 0; i < 9; i++)
            {
                leftWall[i] = 32;
                rightWall[i] = 96;
            }
        }

        public void Right()
        {
            
        }

        public void Up()
        {
         
        }
    }
}