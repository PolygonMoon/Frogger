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
    static byte mapStartX = 2;
    static byte mapStartY = 2;
    static byte mapLenghtX = 72;
    static byte mapLenghtY = 26;

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
                              // Starting Char Position Setup
        charPosX = mapStartX + 1 + mapLenghtX / 2;
        charPosY = mapStartY + mapLenghtY - 1;
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
        }
    }


    // === METHODS
    static void RenderUI()
    {
        DrawBox(); // Draw Map Border
        // DrawBox() // Draw UI Border
    }

    static void DrawBox()
    {
        for (int y = 0; y < mapLenghtY + 2; y++)
        {
            for (int x = 0; x < mapLenghtX + 2; x++)
            {
                SetCursorPosition(mapStartX + x, mapStartY + y);
                if (y == 0 && x == 0) Write("┌");                                   // Top-Left
                else if (y == 0 && x == mapLenghtX + 1) Write("┐");                 // Top-Right
                else if (y == mapLenghtY + 1 && x == 0) Write("└");                 // Bottom-Left
                else if (y == mapLenghtY + 1 && x == mapLenghtX + 1) Write("┘");    // Bottom-Right
                else if (y == 0 || y == mapLenghtY + 1) Write("─");
                else if (x == 0 || x == mapLenghtX + 1) Write("│");
            }
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
        Thread.Sleep(renderDelay);
    }
}