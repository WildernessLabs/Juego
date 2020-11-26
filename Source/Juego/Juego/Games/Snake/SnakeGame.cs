﻿using System;
using System.Collections;

namespace Juego.Games
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point(Point point)
        {
            X = point.X;
            Y = point.Y;
        }
    }

    public enum SnakeDirection : byte
    {
        Up,
        Down,
        Left,
        Right,
        Stop, //to start
    }

    public partial class SnakeGame : IGame
    {
        public int Score { get; private set; }
        public int Level { get; private set; }

        public int BoardWidth { get; private set; }
        public int BoardHeight { get; private set; }

        int topOffset = 10;

        public ArrayList SnakePosition { get; private set; }

        public Point FoodPosition { get; private set; }

        public SnakeDirection Direction { get; private set; }

        public bool PlaySound { get; private set; }

        Random rand = new Random((int)DateTime.Now.Ticks);

        enum CellType : byte
        {
            Empty,
            Food,
        }

        public SnakeGame(int width, int height)
        {
            BoardWidth = width; //ignore borders
            BoardHeight = height; //make room for text

            SnakePosition = new ArrayList();

            Reset();
        }

        public void Update()
        {
            Score++;
            PlaySound = false;

            if (Direction == SnakeDirection.Stop)
                return;

            var head = new Point((Point)SnakePosition[0]);
            var tail = new Point((Point)SnakePosition[SnakePosition.Count - 1]);

            if (Direction == SnakeDirection.Left)
            { head.X--; }
            if (Direction == SnakeDirection.Right)
            { head.X++; }
            if (Direction == SnakeDirection.Up)
            { head.Y--; }
            if (Direction == SnakeDirection.Down)
            { head.Y++; }

            for (int i = 0; i < SnakePosition.Count - 1; i++)
            {
                SnakePosition[SnakePosition.Count - 1 - i] = new Point((Point)SnakePosition[SnakePosition.Count - 2 - i]);
            }

            SnakePosition[0] = head;

            if (IsCellEmpty(head.X, head.Y, true) == false)
            {
                Reset();
            }

            if (head.X == FoodPosition.X && head.Y == FoodPosition.Y)
            {
                SnakePosition.Add(tail);
                UpdateFood();
                PlaySound = true;
            }
        }

        public void Reset()
        {
            SnakePosition = new ArrayList();
            SnakePosition.Add(new Point(BoardWidth / 2, BoardHeight / 2));
            Direction = SnakeDirection.Stop;

            Level = 0;
            Score = 0;

            UpdateFood();
        }

        void UpdateFood()
        {
            int foodX, foodY;
            do
            {
                foodX = rand.Next() % (BoardWidth - 2) + 1;
                foodY = rand.Next() % (BoardHeight - topOffset - 1) + topOffset + 1;
            }
            while (IsCellEmpty(foodX, foodY) == false);

            FoodPosition = new Point(foodX, foodY);
            Level++;
        }

        bool IsCellEmpty(int x, int y, bool ignoreHead = false)
        {
            Point snakeBody;

            if (x < 1 ||
                y < topOffset ||
                x >= BoardWidth - 1 ||
                y >= BoardHeight - 1)
                return false;

            for (int i = ignoreHead ? 1 : 0; i < SnakePosition.Count; i++)
            {
                snakeBody = (Point)SnakePosition[i];
                if (snakeBody.X == x && snakeBody.Y == y)
                {
                    return false;
                }
            }
            return true;
        }

        public void Left()
        {
            Direction = SnakeDirection.Left;
        }

        public void Right()
        {
            Direction = SnakeDirection.Right;
        }

        public void Up()
        {
            Direction = SnakeDirection.Up;
        }

        public void Down()
        {
            Direction = SnakeDirection.Down;
        }
    }
}