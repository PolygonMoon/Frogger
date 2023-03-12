using static System.Console;
using System.Threading.Tasks;

public static class Game
{
    // = CharStatus
    static int charPosX;
    static int charPosY;
    static bool isMoving = true;
    // = GameStatus
    static bool isRunning = true;
    static int frame = 0;
    static int frameInput = 0;
    static float frameRate;
    // = GameSetup
    const int inputDelay = 20;
    const int renderDelay = 8;
    static int verticalRange;
    static int horizontalRange;
    // = Map Setup
    static byte mapStartX = 0;
    static byte mapStartY = 5;
    static byte mapLenghtX = 72; // 72
    static byte mapLenghtY = 26; // 26

    // = Char GFX Setup
    static string charGfxTopA = @"\/°°\/"; // * Pivot is on first left char
    static string charGfxBottomA = @"\\__//";

    static string charGfxTopB = @"|/°°\|"; // * Pivot is on first left char
    static string charGfxBottomB = @"/\__/\";

    static public void GameStart()
    {
        GameInit();
        InputHandler();
        Renderer();
    }


    // = GAME INIT
    static void GameInit()
    {
        frameRate = 1 / 8;    // ! Check why can't write 0,0001 numbers

        // Map Setup
        mapLenghtX = (byte)(byte)(WindowWidth - mapStartX);
        mapLenghtY = (byte)(WindowHeight - mapStartY);

        // Starting Char Position Setup
        charPosX = mapStartX + mapLenghtX / 2 - charGfxBottomA.Length / 2 - 1;
        charPosY = mapStartY + mapLenghtY - 2;
        // Movement Rules Setup
        verticalRange = 2;
        horizontalRange = 2;
    }

    // === TASKS
    static void InputHandler()
    {
        Task.Run(async () =>
                {
                    while (isRunning)
                    {
                        if (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo input = Console.ReadKey();
                            switch (input.Key)
                            {
                                case ConsoleKey.W:
                                    // ! ADD if (inside map size)
                                    charPosY -= verticalRange;
                                    break;
                                case ConsoleKey.S:
                                    charPosY += verticalRange;
                                    break;
                                case ConsoleKey.A:
                                    charPosX -= horizontalRange;
                                    break;
                                case ConsoleKey.D:
                                    charPosX += horizontalRange;
                                    break;
                                case ConsoleKey.Spacebar:
                                    //Shoot();
                                    break;
                                case ConsoleKey.Escape:
                                    isRunning = false;
                                    break;
                                default:
                                    break;
                            }
                            frameInput++;
                            isMoving = !isMoving;
                            await Task.Delay(inputDelay);
                        }
                    }
                });
    }

    static void Renderer()
    {
        while (isRunning)
        {
            frame++;
            Clear();
            RenderUI();
            charRender();
            // CollisionCheck
            Thread.Sleep(renderDelay);
        }
    }


    // === METHODS
    static void RenderUI()
    {
        // TODO use horizontal console lenght
        DrawLineH(0, WindowWidth, mapStartY - 1, "_");      // Draw Line at Debug Panel Bottom

        // ! Avoid due to performance issue
        //DrawBoxEmpty(mapStartX, mapLenghtX, mapStartY, mapLenghtY); // Draw Map Border
        //DrawBox(10,20,5,10, "X"); // Test Box Draw
    }

    static void DrawBoxEmpty(int xStart, int xLength, int yStart, int yLength)
    {
        // Draw Corner
        SetCursorPosition(xStart, yStart);
        Write("┌");
        SetCursorPosition(xStart + xLength + 1, yStart);
        Write("┐");
        SetCursorPosition(xStart, yStart + yLength + 1);
        Write("└");
        SetCursorPosition(xStart + xLength + 1, yStart + yLength + 1);
        Write("┘");
        DrawLineH(xStart + 1, xLength, yStart, "-");
        DrawLineH(xStart + 1, xLength, yStart + yLength + 1, "-");

        DrawLineV(yStart + 1, yLength, xStart, "|");
        DrawLineV(yStart + 1, yLength, xStart + xLength + 1, "|");
    }

    static void DrawBox(int xStart, int xLength, int yStart, int yLength, string symbol)
    {
        for (int y = 0; y < yLength + 2; y++)
        {
            for (int x = 0; x < xLength + 2; x++)
            {
                SetCursorPosition(xStart + x, yStart + y);
                Write(symbol);
            }
        }
    }

    static void DrawLineH(int xStart, int xLength, int yStart, string symbol)
    {
        for (int i = 0; i < xLength; i++)
        {
            SetCursorPosition(xStart + i, yStart);
            Write(symbol);
        }
    }
    static void DrawLineV(int yStart, int yLength, int xStart, string symbol)
    {
        for (int i = 0; i < yLength; i++)
        {
            SetCursorPosition(xStart, yStart + i);
            Write(symbol);
        }
    }

    static void charRender()
    {
        // Debug UI Render
        SetCursorPosition(0, 0);
        Write($"FRAME: {frame}");
        SetCursorPosition(22, 0);
        Write($"Frame Refresh Rate: {frameRate}");
        //SetCursorPosition(22,1);
        //(Write($"Cursos Position  X:{Console.CursorLeft} Y:{Console.CursorTop}");
        // Draw input frame iteraction count
        SetCursorPosition(0, 1);
        Write($"Input Pass: {frameInput}");

        // Draw player // ! Call RenderChar() from input handler
        // TODO Use IdleRender, on jump render JumpRender, wait 0.2 then render IdleRender
        if (isMoving)
        {
            SetCursorPosition(charPosX, charPosY);
            Write(charGfxTopA);
            SetCursorPosition(charPosX, charPosY + 1);
            Write(charGfxBottomA);
        }
        else
        {
            SetCursorPosition(charPosX, charPosY);
            Write(charGfxTopB);
            SetCursorPosition(charPosX, charPosY + 1);
            Write(charGfxBottomB);
        }
    }
}