using System;
using System.Threading;
using System.Diagnostics;
using static System.Console;

namespace SnakeGame {
    class Program {

        /* ===== Основные переменные ===== */
        private const int MAP_WIDTH = 50;
        private const int MAP_HEIGHT = 40;
        private const int SIZE_RATIO = 2;
        private const int ANIMATION_FRAME = 1;
        private const int SCREEN_WIDTH = MAP_WIDTH * SIZE_RATIO;
        private const int SCREEN_HEIGHT = MAP_HEIGHT * SIZE_RATIO;
        private const ConsoleColor BORDER_COLOR = ConsoleColor.White;

        private const ConsoleColor snakeHeadColor = ConsoleColor.Green;
        private const ConsoleColor snakeBodyColor = ConsoleColor.Green;
        private const ConsoleColor foodColor = ConsoleColor.DarkRed;

        private static readonly Random random = new Random();

        /* ===== Инициализация ===== */
        static void Main() {
            SetWindowSize(SCREEN_WIDTH, SCREEN_HEIGHT);
            SetBufferSize(SCREEN_WIDTH, SCREEN_HEIGHT);
            CursorVisible = false;

            new Print(SCREEN_WIDTH / 3, SCREEN_HEIGHT / 2, "Press Enter to start.", ConsoleColor.White).PrintText();
            ReadKey();

            while (true) {
                if (ReadKey().Key == ConsoleKey.Enter) GameLoop();
            }
        }

        /* ===== Игровой цикл ===== */
        static void GameLoop() {

            short score = 0; // Очки
            short speed = ANIMATION_FRAME * 200; // Скорость
            int lagMs = 0; // Лаг

            Clear();
            DrawBorder();

            new Print(3, 3, $"Score: {score}", ConsoleColor.Yellow).PrintText(); 
            new Print(3, 5, $"Speed: {speed}", ConsoleColor.Blue).PrintText();

            Direction currMovement = Direction.Right;
            Stopwatch stopwatch = new Stopwatch();

            /* ===== Змейка ===== */
            var snake = new Snake(10, 5, snakeHeadColor, snakeBodyColor);

            /* ===== Еда ===== */
            Pixel food = GeneradeFood(snake);
            food.Draw();

            while (true) {

                stopwatch.Restart();
                Direction oldMovement = currMovement;

                while (stopwatch.ElapsedMilliseconds <= speed - lagMs) {
                    if (oldMovement == currMovement)
                        currMovement = ReadMovement(currMovement);
                }

                stopwatch.Restart();

                if (snake.Head.X == food.X && snake.Head.Y == food.Y) {
                    snake.Move(currMovement, true);

                    food = GeneradeFood(snake);
                    food.Draw();

                    score++;
                    if (speed > 100) speed -= 5;

                    new Print(3, 3, $"Score: {score}", ConsoleColor.Yellow).PrintText(); // Начисление очков
                    new Print(3, 5, $"Speed: {speed}", ConsoleColor.Blue).PrintText(); // Скорость
                    Task.Run(() => Beep(1000, 200)); // Звук поедания
                } else snake.Move(currMovement);

                /* ===== Коллизии ===== */
                if (snake.Head.X == MAP_WIDTH - 1
                    || snake.Head.X == 0
                    || snake.Head.Y == MAP_HEIGHT - 1
                    || snake.Head.Y == 0
                    || snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y)) {
                    break;
                }

                lagMs = (int)stopwatch.ElapsedMilliseconds;
            }

            /* ===== Конец ===== */
            Clear();
            new Print(SCREEN_WIDTH / 3, SCREEN_HEIGHT / 2, $"Game over! Score: {score}", ConsoleColor.White).PrintText();
            new Print(SCREEN_WIDTH / 3, (SCREEN_HEIGHT / 2) + 3, $"Press Enter to start.", ConsoleColor.White).PrintText();
            Task.Run(() => Beep(200, 800)); // Звук Game over
        }

        /* ===== Генерация еды ===== */
        static Pixel GeneradeFood(Snake snake) {
            Pixel food;

            do {
                food = new Pixel(random.Next(1, MAP_WIDTH - 2), random.Next(minValue: 1, MAP_HEIGHT - 2), foodColor);
            } while (snake.Head.X == food.X && snake.Head.Y == food.Y 
                || snake.Body.Any(b => b.X == food.X && b.Y == food.Y));
            return food;
        }

        /* ===== Управление ===== */
        static Direction ReadMovement(Direction currDirection) {
            if (!KeyAvailable)
                return currDirection;

            ConsoleKey key = ReadKey(true).Key;

            currDirection = key switch {
                ConsoleKey.UpArrow when currDirection != Direction.Down => Direction.Up,
                ConsoleKey.DownArrow when currDirection != Direction.Up => Direction.Down,
                ConsoleKey.LeftArrow when currDirection != Direction.Right => Direction.Left,
                ConsoleKey.RightArrow when currDirection != Direction.Left => Direction.Right,
                _ => currDirection
            };

            return currDirection;
        }

        /* ===== Рендер границ ===== */
        static void DrawBorder() {
            for (int i = 0; i < MAP_WIDTH; i++) {
                new Pixel(i, 0, BORDER_COLOR, '░').Draw();
                new Pixel(i, MAP_HEIGHT - 1, BORDER_COLOR, '░').Draw();
            }
            for (int i = 0; i < MAP_HEIGHT; i++) {
                new Pixel(0, i, BORDER_COLOR, '░').Draw();
                new Pixel(MAP_WIDTH - 1, i, BORDER_COLOR, '░').Draw();
            }
        }
    }
}
