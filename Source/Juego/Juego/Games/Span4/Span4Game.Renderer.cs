﻿using Meadow.Foundation.Graphics;

namespace Juego.Games
{
    public partial class Span4Game
    {
        int cellSize;
        int chipRadius;
        int yStart;
        int xStart = 0;

        int boardWidth;
        int boardHeight;

        public void Init(GraphicsLibrary gl)
        {
            if(gl.Width == 240)
            {
                yStart = 20;
                cellSize = 18;
                chipRadius = 7;
                boardWidth = 128;
                boardHeight = 110;

                gl.CurrentFont = new Font12x16();
            }
            else
            {
                yStart = 9;
                cellSize = 9;
                chipRadius = 3;
                boardWidth = 64;
                boardHeight = 55;

                gl.CurrentFont = new Font4x8();
            }

            gl.Clear();
            gl.DrawText(0, 0, "Meadow Span4");
            gl.DrawText(0, 16, "v0.2.0");
            gl.Show();
        }

        public void Update(IIOConfig ioConfig)
        {
            var gl = ioConfig.Graphics;

            gl.Clear(false);
            Update();
            DrawGame(gl);
            gl.Show();
        }

        void DrawGame(GraphicsLibrary graphics)
        {
            //draw gameboard
            graphics.DrawRectangle(0, yStart, boardWidth, boardHeight, true, false);

            for (int i = 1; i < 7; i++)
            {
                graphics.DrawLine(cellSize * i,
                    yStart,
                    cellSize * i,
                    yStart + cellSize * 6 + 1,
                    true);
            }

            for (int j = 1; j < 6; j++)
            {
                graphics.DrawLine(xStart,
                    yStart + j * cellSize,
                    boardWidth + xStart - 1,
                    yStart + j * cellSize,
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
            int xText = boardWidth + 11;
            int yText = 0;
            graphics.DrawText(xText, yText, "Span4!");
            yText += graphics.CurrentFont.Height * 2;

            graphics.DrawText(xText, yText, "Player 1");
            yText += graphics.CurrentFont.Height + 1;
         //   DrawChip(115, 21, true, graphics);

            graphics.DrawText(xText, yText, "Player 2");
            yText += graphics.CurrentFont.Height * 2;
            //   DrawChip(115, 30, false, graphics);

            graphics.DrawText(xText, yText, "Score:");
            yText += graphics.CurrentFont.Height + 1;
            graphics.DrawText(xText, yText, $"{Player1Wins} to {Player2Wins}");
        }

        void DrawPreviewChip(int column, bool isFilled, GraphicsLibrary graphics)
        {
            DrawChip(xStart + column * cellSize + (cellSize + 1) / 2,
                (cellSize + 1) / 2,
                isFilled,
                graphics);
        }

        void DrawChipOnBoard(int column, int row, bool isFilled, GraphicsLibrary graphics)
        {
            DrawChip(xStart + column * cellSize + (cellSize + 1)/2,
                yStart + (Height - row - 1) * cellSize + (cellSize + 1) / 2,
                isFilled, graphics);
        }
        void DrawChip(int xCenter, int yCenter, bool isFilled, GraphicsLibrary graphics)
        {
            graphics.DrawCircle(xCenter, yCenter, chipRadius,
                            true, isFilled, true);
        }
    }
}