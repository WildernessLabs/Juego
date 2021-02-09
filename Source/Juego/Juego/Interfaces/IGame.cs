using Meadow.Foundation.Graphics;

namespace Juego.Games
{
    public interface IGame
    {
        void Left();
        void Right();
        void Up();
        void Down();

        void Update(GraphicsLibrary gl);

        void Init(GraphicsLibrary gl);

        void Reset();
    }
}