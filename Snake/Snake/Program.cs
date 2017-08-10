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
            graphics.DrawRectangle(color, x, y, height, width,2);
        }
        public void Up(ref int x_h, ref int x_b, ref int x_c, ref int y_h, ref int y_b, ref int y_c)
        {
            Thread.Sleep(100); y_c = y_b; x_c = x_b; x_b = x_h; y_b = y_h; y_c = y_b; y_h -= 10; 
        }
        public void Down(ref int x_h, ref int x_b, ref int x_c, ref int y_h, ref int y_b, ref int y_c)
        {
            Thread.Sleep(100); y_c = y_b; x_c = x_b; x_b = x_h; y_b = y_h; y_c = y_b; y_h += 10; 
        }
        public void Left(ref int x_h, ref int x_b, ref int x_c, ref int y_h, ref int y_b, ref int y_c)
        {
            Thread.Sleep(100); y_c = y_b; x_c = x_b; x_b = x_h; y_b = y_h; y_c = y_b; x_h -= 10; 
        }
        public void Right(ref int x_h, ref int x_b, ref int x_c, ref int y_h, ref int y_b, ref int y_c)
        {
            Thread.Sleep(100); y_c = y_b; x_c = x_b; x_b = x_h;  y_b = y_h; y_c = y_b; x_h += 10; 
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
                while (x_apple[i] % 10 == 0)
                {
                    x_apple[i] = x_Random_to_apple.Next(20, 500);

                }
                while (y_apple[i] % 11 == 0)
                {
                    y_apple[i] = y_Random_to_apple.Next(20, 300);
                }
            }

            int lenght, X_head, Y_head, x_body, y_body, x_clear, y_clear;
            const int delay = 100;
            int i_toRandom_apple;
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
                x_body = 100; y_body = 110;
                i_toRandom_apple = 0;
                lenght = 1;
                
                x_clear = 100; y_clear = 120;
                crashed = false;
                quit = false;       


                ConsoleGraphics graphics = new ConsoleGraphics();
                GameBox box = new GameBox(10, 10, 300, 500);
                SnakeBody body = new SnakeBody(x_body, y_body);
                Apple apple = new Apple(x_apple[i_toRandom_apple], y_apple[i_toRandom_apple]);
                SnakeBody clear = new SnakeBody(x_clear, y_clear);
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

                    if ((X_head - x_apple[i_toRandom_apple])<2 && (Y_head - y_apple[i_toRandom_apple])<2)
                    {
                        lenght++;
                        i_toRandom_apple++;
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
                    graphics.FillRectangle(0xFF000000, 0, 0, graphics.ClientHeight, graphics.ClientWidth);
                    box.Render(graphics, 0xFFFFFFFF, 10, 10);
                    clear.Render(graphics, 0xFF000000, x_clear, y_clear);
                    body.Render(graphics, 0xFF00A0FF, x_body, y_body);
                    head.Render(graphics, 0xFFFF0000, X_head, Y_head);
                    apple.Render(graphics, 0xFF00FF00, x_apple[i_toRandom_apple], y_apple[i_toRandom_apple]);

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