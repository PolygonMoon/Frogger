using static System.Console;
using System.Threading.Tasks;
using static ShootManager;
using static UiTools;

public static class Game
{
    // = CharStatus
    // TODO int lifeCount; 
    // TODO int lifeMax = 4;
    // TODO int playerScore;
    static int charPosX;
    static int charPosY;
    static bool isMoving = false;
    static bool canMove = true;
    static int charLength;
    // = GameStatus
    public static bool isRunning = true;
    static int frame = 0;
    static int inputCount = 0;      // Jump Counter
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
    public static byte mapStartY = 6;
    static byte mapLenghtX = 72; // 72
    static byte mapLenghtY = 26; // 26

    // = Char GFX Setup
    static string charGfxTopA = @"\/^\/"; // * Pivot is on first left char
    static string charGfxBottomA = @"\\_//";

    static string charGfxTopB = @"|/*\|"; // * Pivot is on first left char
    static string charGfxBottomB = @"-\_/-";

    // === Initialize Game Loops
    static public void GameStart()
    {
        GameInit();
        InputHandler();         // Async Loop - Handle Player Input
        ShootsHandler();        // Asynch Loop - Handle Shoots Movements
        ExplosionsHandler();    // Asynch Loop - Handle Explosions Animation
        Renderer();             // Main Loop - Handle Rendering Functions
    }

    // === TASKS & LOOPS
    static void InputHandler()  // Detect and Handle Player Input
    {
        Task.Run(async () =>
                {
                    while (isRunning)
                    {
                        if (inputTimer < inputDelay) inputTimer++;
                        if (inputTimer >= inputDelay) canMove = true;
                        // ! Find a way to detect just one input at time (prevent hold spam)
                        if (Console.KeyAvailable && !isMoving)
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
                                    if (charPosY > mapStartY && ShootManager.canShoot) NewShoot(charPosX + 2, charPosY - 1);
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
        inputCount++;
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

    static void Renderer()  // Rendering Loops
    {
        while (isRunning)
        {
            frame++;
            Clear();
            UiRenderer();
            //MapRenderer();
            ShootsRenderer();
            CharRenderer();
            // CollisionCheck();
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
        Write($"Jump Counter: {inputCount}");

        SetCursorPosition(20, 0);
        Write($"Rendering FPS: {frameRate.ToString("0.00")}");
        SetCursorPosition(20, 1);
        Write($"InputTimer:{inputTimer} / {inputDelay} | {canMove}");

        SetCursorPosition(45, 0);
        Write($"Shoot Count:{shoots.Count} / {maxShoots}");
        SetCursorPosition(45, 1);
        Write($"Shoot Timer:{shootTimer} / {shootDelay} | {canShoot}");

        SetCursorPosition(75, 0);
        Write($"Explosion Count:{explosions.Count}");

        SetCursorPosition(0, 3);
        if (shoots.Count > 0) WriteLine("Rendering Shoots");
        SetCursorPosition(20, 3);
        if (explosions.Count > 0) WriteLine("Rendering Explosions");

        DrawLineH(0, WindowWidth, mapStartY - 2, "_");  // Draw Line at Debug Panel Bottom

        // ! Avoid due to performance issue
        //DrawBoxEmpty(mapStartX, mapLenghtX, mapStartY, mapLenghtY); // Draw Map Border
        //DrawBox(10,20,5,10, "X"); // Test Box Draw
    }

    static void ShootsRenderer()
    {
        // Shoots rendering
        if (shoots.Count > 0)
        {
            for (int i = 0; i < shoots.Count; i++)
            {
                SetCursorPosition(shoots[i].posX, shoots[i].posY);
                Write(shootGfx);
            }
        }
        // Explosions rendering
        if (explosions.Count > 0)
        {
            for (int i = 0; i < explosions.Count; i++)
            {
                SetCursorPosition(explosions[i].posX, explosions[i].posY);
                if (!explosions[i].isExploded) Write(explosionGfxStart);
                else Write(explosionGfxEnd);
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
        SetCursorPosition(96, 0);
        Write($"Input: ");
    }
}