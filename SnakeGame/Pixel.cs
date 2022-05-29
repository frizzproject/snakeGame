using System;

namespace SnakeGame {
    public readonly struct Pixel {

        public Pixel(int x, int y,  ConsoleColor сolor, char fill = '█', int pixelSize = 2) {
            this.X = x;
            this.Y = y;
            this.PixelFill = fill;
            this.Color = сolor;
            this.PixelSize = pixelSize;
        }

        public int X { get; } // Позиция по x

        public int Y { get; } // Позиция по y
        public char PixelFill { get; } // Заливка символом
        public ConsoleColor Color { get; } // Цвет
        public int PixelSize { get; } // Размер пикселя

        /* Рендер */
        public void Draw() {
            Console.ForegroundColor = Color;
            for (int x = 0; x < PixelSize; x++) {
                for (int y = 0; y < PixelSize; y++) {
                    Console.SetCursorPosition(X * PixelSize + x, Y * PixelSize + y);
                    Console.Write(PixelFill);
                }
            }
            Console.ResetColor();
        }

        /* ===== Очистка ===== */
        public void Clear() {
            for (int x = 0; x < PixelSize; x++) {
                for (int y = 0; y < PixelSize; y++) {
                    Console.SetCursorPosition(X * PixelSize + x, Y * PixelSize + y);
                    Console.Write(' ');
                }
            }
        }

    }
}
