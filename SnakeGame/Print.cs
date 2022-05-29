using System;
using static System.Console;

namespace SnakeGame {
    public struct Print {

        private int posX { get; }
        private int posY { get; }
        private string? text { get; }
        private ConsoleColor color { get; }

        public Print(int posX, int posY, string? text, ConsoleColor color) {
            this.posX = posX;
            this.posY = posY;
            this.text = text;
            this.color = color;
        }

        public void PrintText() {
            Console.ForegroundColor = color;
            SetCursorPosition(posX, posY);
            WriteLine(text);
            Console.ResetColor();
        }
    }
}
