using Meadow.Foundation.Graphics;

namespace Juego.Games
{
    public partial class Span4Game
    {
        int CellSize = 9;
        int yStart = 9;
        int xStart = 0;

        public void Init(GraphicsLibrary gl)
        {
            gl.CurrentFont = new Font4x8();
        }

        public void Update(GraphicsLibrary gl)
        {
            gl.Clear(false);
            DrawGame(gl);
            gl.Show();
        }

        void DrawGame(GraphicsLibrary graphics)
        {
            //draw gameboard
            graphics.DrawRectangle(0, 9, 64, 55, true, false);

            for (int i = 1; i < 7; i++)
            {
                graphics.DrawLine(CellSize * i,
                    yStart,
                    CellSize * i,
                    yStart + CellSize * 6 + 1,
                    true);
            }

            for (int j = 1; j < 6; j++)
            {
                graphics.DrawLine(xStart,
                    yStart + j * CellSize,
                    63 + xStart,
                    yStart + j * CellSize,
                    true);
            }

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (GameField[x, y] == 0) { continue; }
                    DrawChipOnBoard(x, y, GameField[x, y] == 1, graphics);
                }
            }

            //Game state
            switch (GameState)
            {
                case Span4Game.GameStateType.Draw:
                    graphics.DrawText(2, 0, "Draw");
                    break;
                case Span4Game.GameStateType.Player1Wins:
                    graphics.DrawText(2, 0, "Player 1 Wins!");
                    break;
                case Span4Game.GameStateType.Player2Wins:
                    graphics.DrawText(2, 0, "Player 2 Wins!");
                    break;
                case Span4Game.GameStateType.Player1Turn:
                    DrawPreviewChip(CurrentColumn, true, graphics);
                    break;
                case Span4Game.GameStateType.Player2Turn:
                    DrawPreviewChip(CurrentColumn, false, graphics);
                    break;
            }

            //Draw side display
            int xText = 75;
            graphics.DrawText(xText, 0, "Span4!");

            graphics.DrawText(xText, 18, "Player 1");
            DrawChip(115, 21, true, graphics);

            graphics.DrawText(xText, 27, "Player 2");
            DrawChip(115, 30, false, graphics);

            graphics.DrawText(xText, 45, "Score:");
            graphics.DrawText(xText, 54, $"{Player1Wins} to {Player2Wins}");
        }

        void DrawPreviewChip(int column, bool isFilled, GraphicsLibrary graphics)
        {
            DrawChip(xStart + column * CellSize + 5,
                5,
                isFilled,
                graphics);
        }

        void DrawChipOnBoard(int column, int row, bool isFilled, GraphicsLibrary graphics)
        {
            DrawChip(xStart + column * CellSize + 5,
                yStart + (Height - row - 1) * CellSize + 5,
                isFilled, graphics);
        }
        void DrawChip(int xCenter, int yCenter, bool isFilled, GraphicsLibrary graphics)
        {
            graphics.DrawCircle(xCenter, yCenter, 3,
                            true, isFilled, true);
        }
    }
}
