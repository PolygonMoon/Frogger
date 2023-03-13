using static System.Console;
using System.Threading.Tasks;

public static class Game
{
    // = CharStatus
    static int charPosX;
    static int charPosY;
    static bool isMoving = false;
    static int charLength;
    // = GameStatus
    static bool isRunning = true;
    static int frame = 0;
    static int frameInput = 0;
    static float frameRate;
    // = GameSetup
    const int inputDelay = 16;
    const int renderDelay = 16;
    static int inputTimer = 0;
    static int verticalRange;
    static int horizontalRange;
    // = Map Setup
    static byte mapStartX = 0;
    static byte mapStartY = 5;
    static byte mapLenghtX = 72; // 72
    static byte mapLenghtY = 26; // 26

    // = Char GFX Setup
    static string charGfxTopA = @"\/^\/"; // * Pivot is on first left char
    static string charGfxBottomA = @"\\_//";

    static string charGfxTopB = @"|/*\|"; // * Pivot is on first left char
    static string charGfxBottomB = @"-\_/-";

    // === Starting Game Loops
    static public void GameStart()
    {
        GameInit();
        InputHandler();
        Renderer();
    }

    // === TASKS & LOOPS
    static void InputHandler()
    {
        Task.Run(async () =>
                {
                    while (isRunning)
                    {
                        inputTimer++;
                        // ! Find a way to detect just one input at time (prevent hold spam)
                        if (Console.KeyAvailable && !isMoving)
                        {
                            inputTimer = 0;
                            frameInput++;
                            JumpAnimation();

                            ConsoleKeyInfo input = Console.ReadKey();
                            switch (input.Key)
                            {
                                case ConsoleKey.W:
                                    if (charPosY > mapStartY)
                                    {
                                        charPosY -= verticalRange;
                                        isMoving = true;
                                    }
                                    break;
                                case ConsoleKey.S:
                                    if (charPosY < mapStartY + mapLenghtY - 2)
                                    {
                                        charPosY += verticalRange;
                                        isMoving = true;
                                    }
                                    break;
                                case ConsoleKey.A:
                                    if (charPosX > 1)
                                    {
                                        charPosX -= horizontalRange;
                                        isMoving = true;
                                    }
                                    break;
                                case ConsoleKey.D:
                                    if (charPosX < mapLenghtX + mapStartX - charLength)
                                    {
                                        charPosX += horizontalRange;
                                        isMoving = true;
                                    }
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
                        }
                        await Task.Delay(inputDelay);
                    }
                });
    }

    static void JumpAnimation()
    {
        Task.Run(async () =>
               {
                   Thread.Sleep(120);
                   isMoving = false;
                   await Task.Delay(1);
                   return;
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
    static void GameInit()
    {
        frameRate = (1.0f / renderDelay) * 1000f;

        // Map Setup
        mapLenghtX = (byte)(byte)((WindowWidth - mapStartX) - 1);
        mapLenghtY = (byte)(WindowHeight - mapStartY);

        // Char Setup
        charLength = charGfxBottomA.Length;
        charPosX = mapStartX + mapLenghtX / 2 - charLength / 2 - 1;
        charPosY = mapStartY + mapLenghtY - 2;
        // Movement Rules Setup
        verticalRange = 2;
        horizontalRange = 2;
    }

    // == RENDERING
    static void RenderUI()
    {
        DrawLineH(0, WindowWidth, mapStartY - 1, "_");  // Draw Line at Debug Panel Bottom

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
        Write($"Frame Refresh Rate: {frameRate.ToString("0.00")}");
        SetCursorPosition(22, 1);
        Write($"InputTimer:{inputTimer}");
        // Draw input frame iteraction count
        SetCursorPosition(0, 1);
        Write($"Input Pass: {frameInput}");

        // Draw player // ! Call RenderChar() from input handler
        // TODO Use IdleRender, on jump render JumpRender, wait 0.2 then render IdleRender
        if (!isMoving)
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