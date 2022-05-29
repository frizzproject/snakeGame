using System;
using System.Collections.Generic;

namespace SnakeGame {
    public class Snake {
        private readonly ConsoleColor headColor;
        private readonly ConsoleColor bodyColor;

        public Snake(int posX, int posY, ConsoleColor headColor, ConsoleColor bodyColor, int bodyLength = 3) {
            this.headColor = headColor;
            this.bodyColor = bodyColor;

            Head = new Pixel(posX, posY, this.headColor, '█');

            for (int i = bodyLength; i >= 0; i--) {
                Body.Enqueue(new Pixel(Head.X - i - 1, posY, this.bodyColor, '█'));
            }

            Draw();
        }
        public Pixel Head { get; private set; } // Голова

        public Queue<Pixel> Body { get; } = new Queue<Pixel>();  // Тело

        /* ===== Движение змеи ===== */
        public void Move(Direction direction, bool eat = false) {
            Clear();

            Body.Enqueue(new Pixel(Head.X, Head.Y, bodyColor, '█'));

            if (!eat)
                Body.Dequeue();

            Head = direction switch {
                Direction.Right => new Pixel(Head.X + 1, Head.Y, headColor, '█'),
                Direction.Left => new Pixel(Head.X - 1, Head.Y, headColor, '█'),
                Direction.Up => new Pixel(Head.X, Head.Y - 1, headColor, '█'),
                Direction.Down => new Pixel(Head.X, Head.Y + 1, headColor, '█'),
                _ => Head
            };

            Draw();
        }

        /* ===== Рендер змеи ===== */
        public void Draw() {
            Head.Draw();

            foreach (Pixel pixel in Body) {
                pixel.Draw();
            }
        }
        public void Clear() {
            Head.Clear();

            foreach (Pixel pixel in Body) {
                pixel.Clear();
            }
        }
    }
}
