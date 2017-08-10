using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NConsoleGraphics;
using System.Threading;

namespace Snake_sproda5
{
    public enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }
    class Figure
    {
        private int x, y, width, height;
        public Figure(int x, int y, int width, int height)
        {
            this.height = height;
            this.x = x;
            this.y = y;
            this.width = width;
        }
        public void Render(ConsoleGraphics graphics, uint color, int x, int y)
        {
            graphics.DrawRectangle(color, x, y, height, width);
        }
        public void Up(ref int x_h, ref int x_b, ref int x_c, ref int y_h, ref int y_b, ref int y_c) { y_h -= 10; y_b = y_h; y_c = y_b; x_b = x_h; x_c = x_b; }
        public void Down(ref int x_h, ref int x_b, ref int x_c, ref int y_h, ref int y_b, ref int y_c) { y_h += 10; y_b = y_h; y_c = y_b; x_b = x_h; x_c = x_b; }
        public void Left(ref int x_h, ref int x_b, ref int x_c, ref int y_h, ref int y_b, ref int y_c) { x_h -= 10; x_b = x_h; x_c = x_b; y_b = y_h; y_c = y_b; }
        public void Right(ref int x_h, ref int x_b, ref int x_c, ref int y_h, ref int y_b, ref int y_c) { x_h += 10; x_b = x_h; x_c = x_b; y_b = y_h; y_c = y_b; }
    }
    class Apple : Figure
    {
        public Apple(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }
    }
    class GameBox : Figure
    {
        public GameBox(int x, int y, int width, int height) : base(x, y, width, height) { }
    }
    class SnakeHead : Figure
    {
        public SnakeHead(int x, int y) : base(x, y, 10, 10) { }
    }
    class SnakeBody : Figure
    {
        public SnakeBody(int x, int y) : base(x, y, 10, 10) { }
    }
    class Program
    {


        static void Main(string[] args)
        {
            int lenght, X_head, Y_head, x_body, y_body, x_clear, y_clear, speed;
            const int delay = 100;
            bool crashed = false;
            bool quit = false;
            Console.CursorVisible = false;
            ConsoleKeyInfo presskey;
            Console.WindowHeight = 84; Console.WindowWidth = 84;
            Direction curDirection = Direction.Up;
            while (!quit)
            {
                Console.Clear();
                Console.Title = "Используй 'a', 's', 'd', 'w'.  Для выхода нажми 'q' ";
                X_head = 100; Y_head = 100;
                x_body = 100; y_body = 110;
                speed = 1000;
                lenght = 1;
                x_clear = 100; y_clear = 120;
                crashed = false;
                quit = false;


                ConsoleGraphics graphics = new ConsoleGraphics();


                GameBox box = new GameBox(10, 10, Console.WindowHeight * 10, Console.WindowWidth * 5);

                SnakeBody body = new SnakeBody(x_body, y_body);
                SnakeBody clearBody = new SnakeBody(x_clear, y_clear);

                SnakeHead head = new SnakeHead(X_head, Y_head);

                // Ждать нажатия клавиши
                while (!Console.KeyAvailable)
                {
                    Thread.Sleep(100);
                }

                // изменение направления
                presskey = Console.ReadKey(true);
                switch (presskey.KeyChar)
                {
                    case 'w':
                        curDirection = Direction.Up;
                        break;

                    case 's':
                        curDirection = Direction.Down;
                        break;

                    case 'a':
                        curDirection = Direction.Left;
                        break;

                    case 'd':
                        curDirection = Direction.Right;
                        break;

                    case 'q':
                        quit = true;
                        break;
                }

                // Игра
                DateTime nextCheck = DateTime.Now.AddMilliseconds(delay);
                while (!quit && !crashed)
                {
                    Console.Title = "Length: " + lenght.ToString();//увеличивает наш счетчик длинны
                    if (X_head < 11 || Y_head < 11 || X_head > Console.WindowHeight * 10 || Y_head > Console.WindowWidth * 5)
                         crashed = true;
                    box.Render(graphics, 0xFFFFFFFF, 10, 10);
                    clearBody.Render(graphics, 0xFF000000, x_clear, y_clear);
                    body.Render(graphics, 0xFF00FF00, x_body, y_body);
                    head.Render(graphics, 0xFFFF0000, X_head, Y_head);

                    Thread.Sleep(speed);
                    while (nextCheck > DateTime.Now)
                    {
                        if (Console.KeyAvailable == true)
                        {
                            presskey = Console.ReadKey(true);
                            switch (presskey.KeyChar)
                            {
                                case 'w':
                                    curDirection = Direction.Up;
                                    break;

                                case 's':
                                    curDirection = Direction.Down;
                                    break;

                                case 'a':
                                    curDirection = Direction.Left;
                                    break;

                                case 'd':
                                    curDirection = Direction.Right;
                                    break;

                                case 'q':
                                    quit = true;
                                    break;
                            }
                        }
                    }


                    if (!quit)
                    {

                        switch (curDirection)
                        {
                            case Direction.Up:
                                head.Up(ref X_head, ref x_body, ref x_clear, ref Y_head, ref y_body, ref y_clear);
                                break;

                            case Direction.Down:
                                head.Down(ref X_head, ref x_body, ref x_clear, ref Y_head, ref y_body, ref y_clear);
                                break;

                            case Direction.Left:
                                head.Left(ref X_head, ref x_body, ref x_clear, ref Y_head, ref y_body, ref y_clear);
                                break;

                            case Direction.Right:
                                head.Right(ref X_head, ref x_body, ref x_clear, ref Y_head, ref y_body, ref y_clear);
                                break;
                        }
                        nextCheck = DateTime.Now.AddMilliseconds(delay);
                    }
                } 


                if (crashed)
                {
                    Console.Title = "*** Crashed! *** Length: " + lenght.ToString() + "     Hit 'q' to quit, or 'r' to retry!";
                    bool retry = false;
                    while (!quit && !retry)
                    {
                        if (Console.KeyAvailable)
                        {
                            presskey = Console.ReadKey(true);
                            switch (presskey.KeyChar)
                            {
                                case 'q':
                                    quit = true;
                                    break;

                                case 'r':
                                    retry = true;
                                    break;
                            }
                        }
                    }
                }

            }

        } 

    }
}