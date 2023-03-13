using static System.Console;
using System.Threading.Tasks;
using static ShootManager;
using static UiTools;

public static class Game
{
    // = CharStatus
    static int charPosX;
    static int charPosY;
    static bool isMoving = false;
    static bool canMove = true;
    static int charLength;
    // = GameStatus
    public static bool isRunning = true;
    static int frame = 0;
    static int frameInput = 0;
    static float frameRate;
    // = GameSetup
    const int inputRefresh = 16;
    const int inputDelay = 3;       // Pause between input detection | Also used to manage the Player movement speed
    const int renderDelay = 16;
    static int inputTimer;
    static int verticalRange;
    static int horizontalRange;
    // = Map Setup
    public static byte mapStartX = 0;
    public static byte mapStartY = 5;
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
        InputHandler();     // Async Loop - Handle Player Input
        ShootsHandler();    // Asynch Loop - Handle Shoots Movements
        Renderer();         // Main Loop - Handle Rendering Methods
    }

    // === TASKS & LOOPS
    static void InputHandler()
    {
        Task.Run(async () =>
                {
                    while (isRunning)
                    {
                        if (inputTimer < inputDelay) inputTimer++;
                        if (inputTimer >= inputDelay) canMove = true;
                        // ! Find a way to detect just one input at time (prevent hold spam)
                        if (Console.KeyAvailable && !isMoving && canMove)
                        {
                            ConsoleKeyInfo input = Console.ReadKey();
                            switch (input.Key)
                            {
                                case ConsoleKey.W:
                                    if (charPosY > mapStartY && canMove)
                                    {
                                        charPosY -= verticalRange;
                                        UpdateInput();
                                    }
                                    break;
                                case ConsoleKey.S:
                                    if (charPosY < mapStartY + mapLenghtY - 2 && canMove)
                                    {
                                        charPosY += verticalRange;
                                        UpdateInput();
                                    }
                                    break;
                                case ConsoleKey.A:
                                    if (charPosX > 1 && canMove)
                                    {
                                        charPosX -= horizontalRange;
                                        UpdateInput();
                                    }
                                    break;
                                case ConsoleKey.D:
                                    if (charPosX < mapLenghtX + mapStartX - charLength && canMove)
                                    {
                                        charPosX += horizontalRange;
                                        UpdateInput();
                                    }
                                    break;
                                case ConsoleKey.Spacebar:
                                    if (charPosY > mapStartY && ShootManager.canShoot) Spawn(charPosX + 2, charPosY - 1);
                                    break;
                                case ConsoleKey.Escape:
                                    isRunning = false;
                                    break;
                                default:
                                    break;
                            }
                        }
                        // Clear the input buffer if input is not being processed within the switch cases
                        else if (Console.KeyAvailable)
                        {
                            Console.ReadKey(true); // Read the key to remove it from the input buffer
                        }
                        await Task.Delay(inputRefresh);
                    }
                });
    }

    static void UpdateInput()
    {
        canMove = false;
        isMoving = true;
        inputTimer = 0;
        frameInput++;
        JumpAnimation();
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
            UiRenderer();
            //MapRenderer();
            ShootsRenderer();
            CharRenderer();
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
        inputTimer = inputDelay;
    }

    // == RENDERING
    static void UiRenderer()
    {
        // Debug UI Render
        SetCursorPosition(0, 0);
        Write($"FRAME: {frame}");
        SetCursorPosition(0, 1);
        Write($"Input Pass: {frameInput}");

        SetCursorPosition(18, 0);
        Write($"Frame Refresh Rate: {frameRate.ToString("0.00")}");
        SetCursorPosition(18, 1);
        Write($"InputTimer:{inputTimer} / {inputDelay} | {canMove}");

        SetCursorPosition(48, 0);
        Write($"Shoot Count:{shoots.Count} / {maxShoots}");
        SetCursorPosition(48, 1);
        Write($"Shoot Timer:{shootTimer} / {shootDelay} | {canShoot}");

        DrawLineH(0, WindowWidth, mapStartY - 2, "_");  // Draw Line at Debug Panel Bottom

        // ! Avoid due to performance issue
        //DrawBoxEmpty(mapStartX, mapLenghtX, mapStartY, mapLenghtY); // Draw Map Border
        //DrawBox(10,20,5,10, "X"); // Test Box Draw
    }

    static void ShootsRenderer()
    {
        if (shoots.Count > 0)
        {
            // ! Cause Expetion due to shoots list run-time modifications
            // foreach (var shoot in shoots)
            // {
            //     {
            //         SetCursorPosition(shoot.posX, shoot.posY);
            //         Write(shootGfx);
            //     }
            // }

            for (int i = 0; i < shoots.Count; i++)
            {
                SetCursorPosition(shoots[i].posX, shoots[i].posY);
                Write(shootGfx);
            }
        }
    }

    static void CharRenderer()
    {
        // Draw player
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
        // Magic Trick to avoid ReadKey() input render inside near char
        SetCursorPosition(68, 0);
        Write($"Input: ");
    }
}