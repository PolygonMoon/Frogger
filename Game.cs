using static System.Console;
using System.Threading.Tasks;
using static InputManager;
using static ShootManager;
using static UiTools;

public static class Game
{
    // = CharStatus // ! Move to Entity.cs 
    // TODO int playerScore;    // Move to Score.cs
    public static int charPosX;
    public static int charPosY;
    public static int charLength;
    public static int moveCounter = 0;      // Jump Counter

    // = Char GFX Setup
    static string charGfxTopA = @"\/^\/"; // * Pivot is on first left char
    static string charGfxBottomA = @"\\_//";

    static string charGfxTopB = @"|/*\|"; // * Pivot is on first left char
    static string charGfxBottomB = @"/\_/\";

    // = Entity Manager
    public static List<Entity> players = new List<Entity>();
    public static List<Entity> cars = new List<Entity>();
    public static List<Entity> enemies = new List<Entity>();
    public static List<Entity> trunks = new List<Entity>();
    public static List<Entity> coins = new List<Entity>();
    public static List<Entity> powerUps = new List<Entity>();

    // = Game Status
    public static bool isRunning = true;
    // TODO int lifeCount;
    // TODO int lifeMax = 4;
    static int currentLevelIndex;   // TODO Current Level Index to track current level 
    static int frame = 0;

    static float frameRate;
    // = Game Setup
    public const int inputRefresh = 16;
    public const int inputDelay = 3;       // Pause between input detection | Also used to manage the Player movement speed
    const int renderDelay = 16;
    public static int inputTimer;
    // = Movement Setup
    public static int verticalRange = 2;
    public static int horizontalRange = 2;
    // = Map Setup
    public static byte mapStartX = 0;
    public static byte mapStartY = 6;
    public static byte mapLenghtX = 72; // 72
    public static byte mapLenghtY = 26; // 26

    // === Initialize Game Loops
    static public void GameStart()
    {
        GameInit();             // ! Move Map/Level stuff to MapInit();
        //MapInit();            // ! Include LoadMap() here!GunHandler();    
        //EntityInit();         // 
        InputHandler();         // Async Loop - Handle Player Input
        ExplosionsHandler();    // Asynch Loop - Handle Explosions Animation
        Renderer();             // Main Loop - Handle Rendering Functions
    }

    // === TASKS & LOOPS
    static void Renderer()  // Rendering Loops
    {
        while (isRunning)
        {
            frame++;
            Clear();
            UiRenderer();
            //MapRenderer();
            BulletsRenderer();
            ExplosionsRenderer();
            EntitiesRenderer();
            PlayerRenderer();
            // CollisionCheck();
            Thread.Sleep(renderDelay);
        }
    }

    // === METHODS
    static void GameInit()
    {
        // System Init
        frameRate = (1.0f / renderDelay) * 1000f;   // Calculate the Frame Rate
        inputTimer = inputDelay;                    // Enable istant player input availability 

        // =============== MAP INIT
        // Map Setup
        // TODO (if actualLevelIndex == 0) LoadMap(0);
        // ! LoadMap(index);  // From Map.cs
        mapLenghtX = (byte)(byte)((WindowWidth - mapStartX) - 1);
        mapLenghtY = (byte)(WindowHeight - mapStartY);

        // === ENTITIES SETUP
        // ! Add NewPlayerInit() | OR CHECK CONSTRUCTOR TO CREATE ISTANT QUICKLY
        // Setup First Player
        charLength = charGfxBottomA.Length;
        Entity player = new Entity();
        player.spawnX = mapStartX + mapLenghtX / 2 - charLength / 2 - 1;
        player.spawnY = mapStartY + mapLenghtY - 2;
        player.Spawn(player.spawnX, player.spawnY, Entity.EntityType.Enemy, Entity.MoveType.Left);
        player.Init();
        player.gun = new Gun();
        player.gun.owner = player;
        players.Add(player);
        player.gun.BulletHandler();

        // Setup First Player
        charLength = charGfxBottomA.Length;
        Entity player2 = new Entity();
        player2.spawnX = mapStartX + mapLenghtX / 2 - charLength / 2 - 1 + 20;
        player2.spawnY = mapStartY + mapLenghtY - 2;
        player2.Spawn(player2.spawnX, player2.spawnY, Entity.EntityType.Enemy, Entity.MoveType.Left);
        player2.Init();
        player2.gun = new Gun();
        player2.gun.owner = player2;
        players.Add(player2);
        player2.gun.BulletHandler();


        // Setup Car
        Entity car = new Entity();
        car.Spawn(mapLenghtX / 2, mapLenghtY / 2, Entity.EntityType.Enemy, Entity.MoveType.Left);
        car.Init();

        cars.Add(car);
    }

    // === RENDERING
    static void UiRenderer()
    {
        // Debug UI Render
        SetCursorPosition(0, 0);
        Write($"FRAME: {frame}");
        SetCursorPosition(0, 1);
        Write($"Jump Counter: {moveCounter}");

        SetCursorPosition(20, 0);
        Write($"Rendering FPS: {frameRate.ToString("0.00")}");
        SetCursorPosition(20, 1);
        Write($"InputTimer:{inputTimer} / {inputDelay} | {inputReady}");

        SetCursorPosition(45, 0);
        Write($"P.Shoot:{players[0].gun.bullets.Count} / {players[0].gun.bulletAmount} | G.Shoot:{bullets.Count}");
        SetCursorPosition(45, 1);
        Write($"P.Shoot Timer:{players[0].gun.shootTimer} / {players[0].gun.shootDelay} | {players[0].gun.canShoot}");

        SetCursorPosition(75, 0);
        Write($"G.Explosion Count:{explosions.Count}");

        SetCursorPosition(0, 3);
        if (bullets.Count > 0) WriteLine("Rendering Bullets");
        SetCursorPosition(20, 3);
        if (explosions.Count > 0) WriteLine("Rendering Explosions");

        // Debug Player Position
        SetCursorPosition(0, mapLenghtY + mapStartY - 1);
        Write($"X: {players[0].posX} | Y: {players[0].posY}");
        Write($"|===| MapLimit X: {mapStartX},{mapLenghtX + mapStartX} | Y: {mapStartY},{mapLenghtY + mapStartY}");

        DrawLineH(0, WindowWidth, mapStartY - 1, "_");              // Draw Line for Debug Panel (Upper)

        // ! Avoid due to performance issue
        //DrawBoxEmpty(mapStartX, mapLenghtX, mapStartY, mapLenghtY); // Draw Map Border
        //DrawBox(10,20,5,10, "X"); // Test Box Draw
    }

    static void BulletsRenderer()
    {
        // Shoots rendering
        if (bullets.Count > 0)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].isExploded) bullets.RemoveAt(i);
                else
                {
                    SetCursorPosition(bullets[i].posX, bullets[i].posY);
                    Write(bullets[i].gfx);
                }
            }
        }
    }

    static void ExplosionsRenderer()
    {
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

    static void EntitiesRenderer()
    {
        for (int i = 0; i < cars.Count; i++)
        {
            foreach (var tile in cars[i].tiles)
            {
                SetCursorPosition(tile.posX, tile.posY);
                Write(tile.gfx);
            }
        }
        // ! Add different for loops to render Cars, Trunk, Enemy, Coins, PowerUp etc

    }

    static void PlayerRenderer()
    {
        for (int i = 0; i < players.Count; i++)
        {
            // Draw player
            if (!isMoving)
            {
                SetCursorPosition(players[i].posX, players[i].posY);
                Write(charGfxTopA);
                SetCursorPosition(players[i].posX, players[i].posY + 1);
                Write(charGfxBottomA);
            }
            else
            {
                SetCursorPosition(players[i].posX, players[i].posY);
                Write(charGfxTopB);
                SetCursorPosition(players[i].posX, players[i].posY + 1);
                Write(charGfxBottomB);
            }
        }
        // Magic Trick to avoid ReadKey() input render inside near char | Used as debug input info
        SetCursorPosition(96, 0);
        Write($"Input: ");
    }
}