using static System.Console;
using System.Threading.Tasks;

public static class Game
{
    // = GameStatus
    int charPosX;
    int charPosY;
    bool isMoving = true;

    bool isRunning = true;
    int frame = 0;
    int frameInput = 0;
    float frameRate;
    // = Map Setup
    byte mapStartX = 4;
    byte mapStartY = 6;
    byte mapLenghtX = 40;
    byte mapLenghtY = 20;
    // = Char GFX Setup
    string charGfxTopA = @"\/°°\/"; // * Pivot is on first left char
    string charGfxBottomA = @"\\__//";

    string charGfxTopB = @"|/°°\|"; // * Pivot is on first left char
    string charGfxBottomB = @"/\__/\";

    public void GameStart()
    {
        GameInit();
        InputHandler();
        Renderer();
    }


    // = GAME INIT
    void GameInit()
    {
        RenderUI();
        charPosX = mapStartX + mapLenghtX / 2;
        charPosY = mapStartY + mapLenghtY - 1;
    }



    // === TASKS
    void InputHandler()
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
                                    charPosY -= 2;
                                    break;
                                case ConsoleKey.S:
                                    charPosY += 2;
                                    break;
                                case ConsoleKey.A:
                                    charPosX -= 2;
                                    break;
                                case ConsoleKey.D:
                                    charPosX += 2;
                                    break;
                                case ConsoleKey.Escape:
                                    isRunning = false;
                                    break;
                                default:
                                    break;
                            }
                            frameInput++;
                            isMoving = !isMoving;
                            await Task.Delay(20);
                        }
                    }
                });
    }

    void Renderer()
    {
        while (isRunning)
        {
            frame++;
            charRender();
            // CollisionCheck
        }
    }

    // === METHODS
    void RenderUI()
    {
        Clear();
        // DrawBox() // Draw Map Border
        // DrawBox() // Draw UI Border
    }

    void charRender()
    {
        Clear();
        SetCursorPosition(0, 0);
        Write($"FRAME: {frame}");
        SetCursorPosition(16, 0);
        Write($"Frame Refresh Rate: {frameRate}");

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

        // Draw input frame iteraction count
        SetCursorPosition(0, 1);
        Write($"Input Pass: {frameInput}");
        Thread.Sleep(8);
    }
}