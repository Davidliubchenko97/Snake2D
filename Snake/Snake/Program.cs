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
            graphics.DrawRectangle(color, x, y, height, width, 2);
        }
        public void Up(ref int y_h)
        {
             y_h -= 10;
        }
        public void Down(ref int y_h)
        {
             y_h += 10;
        }
        public void Left(ref int x_h)
        {
             x_h -= 10;
        }
        public void Right(ref int x_h)
        {
             x_h += 10;
        }
    }
    class Apple : Figure
    {
        public Apple(int x, int y) : base(x, y, 10, 10) { }
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
            Random x_Random_to_apple = new Random();
            Random y_Random_to_apple = new Random();
            int[] x_apple = new int[100000];
            int[] y_apple = new int[100000];
            for (int i = 0; i < 100; i++)
            {
                    x_apple[i] = 10*x_Random_to_apple.Next(2, 50);
                    y_apple[i] = 10*y_Random_to_apple.Next(2, 30);
            }

            int lenght, X_head, Y_head;
            const int delay = 100;
            int i_to_apple;
            int speed = 500;
            bool crashed = false;
            bool quit = false;
            bool retry = false;
            Console.CursorVisible = false;
            ConsoleKeyInfo presskey;
            Console.WindowHeight = 84; Console.WindowWidth = 84;
            Direction curDirection = Direction.Up;
            while (!quit)
            {
                Console.Clear();
                Console.Title = "Используй 'a', 's', 'd', 'w'.  Для выхода нажми 'q' ";
                X_head = 100; Y_head = 100;

                i_to_apple = 0;
                lenght = 1;
                crashed = false;
                quit = false;


                ConsoleGraphics graphics = new ConsoleGraphics();
                GameBox box = new GameBox(10, 10, 300, 500);
                SnakeBody[] body = new SnakeBody[1];
                int[] x_body_mass = new int[1];
                int[] y_body_mass = new int[1];
                x_body_mass[0] = 100; y_body_mass[0] = 110;

                Apple apple = new Apple(x_apple[i_to_apple], y_apple[i_to_apple]);

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
                    if (X_head < 10 || Y_head < 10 || X_head > 500 || Y_head > 300)
                        crashed = true;

                    if ((X_head == x_apple[i_to_apple])&& (Y_head == y_apple[i_to_apple]))
                    {
                        lenght++;
                        if (lenght % 2 == 0&& speed != 0)
                        {
                            speed -= 50;
                        }
                            
                        
                        i_to_apple++;
                    }

                    while (nextCheck > DateTime.Now)
                    {
                        if (Console.KeyAvailable)
                        {
                            presskey = Console.ReadKey(true);
                            switch (presskey.KeyChar)
                            {
                                case 'w':
                                    if (curDirection != Direction.Down)
                                    {
                                        curDirection = Direction.Up;
                                    }
                                    break;

                                case 's':
                                    if (curDirection != Direction.Up)
                                    {
                                        curDirection = Direction.Down;
                                    }
                                    break;

                                case 'a':
                                    if (curDirection != Direction.Right)
                                    {
                                        curDirection = Direction.Left;
                                    }
                                    break;

                                case 'd':
                                    if (curDirection != Direction.Left)
                                    {
                                        curDirection = Direction.Right;
                                    }
                                    break;

                                case 'q':
                                    quit = true;
                                    break;
                            }
                        }
                    }

                    Thread.Sleep(speed);
                    graphics.FillRectangle(0xFF000000, 0, 0, graphics.ClientHeight, graphics.ClientWidth);
                    box.Render(graphics, 0xFFFFFFFF, 10, 10);
                    head.Render(graphics, 0xFFFF0000, X_head, Y_head);
                    apple.Render(graphics, 0xFF00FF00, x_apple[i_to_apple], y_apple[i_to_apple]);

                    if (!quit)
                    {

                        switch (curDirection)
                        {
                            case Direction.Up:
                                x_body_mass[body.Length - 1] = X_head;
                                y_body_mass[body.Length - 1] = Y_head;
                                head.Up(ref Y_head);
                                break;

                            case Direction.Down:
                                x_body_mass[body.Length - 1] = X_head;
                                y_body_mass[body.Length - 1] = Y_head;
                                head.Down(ref Y_head);
                                break;

                            case Direction.Left:
                                x_body_mass[body.Length - 1] = X_head;
                                y_body_mass[body.Length - 1] = Y_head;
                                head.Left(ref X_head);
                                break;

                            case Direction.Right:
                                x_body_mass[body.Length - 1] = X_head;
                                y_body_mass[body.Length - 1] = Y_head;
                                head.Right(ref X_head);
                                break;
                        }
                        nextCheck = DateTime.Now.AddMilliseconds(delay);
                    }
                }


                if (crashed)
                {
                    Console.Title = "*** Crashed! *** Length: " + lenght.ToString() + "     Hit 'q' to quit, or 'r' to retry!";

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