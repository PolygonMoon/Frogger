using static System.Console;
using System.Threading.Tasks;
using static InputManager;
using static EntityManager;
using static ShootManager;
using static Renderer;
using static UiTools;

public static class Game
{
    // = Player Status // * Extra Player Status is managed as ecxeption | Do not use normal Entity behaviour
    public static int lifeCount;        // TODO lifeCount-- when all player Entity dies
    public static int lifeMax = 4;      // TODO Overide the value from Map.lifeAmount
    // TODO int playerScore;    // Move to Score.cs ??
    public static int moveCounter = 0;  // Jump Counter

    // = Player GFX Setup   // * Players Gfx and Rendering is managed as ecxeption | Do not use normal Entity behaviour 
    public static int charLength;
    // Top String
    public static string charGfxTopA = @"\/^\/"; // * Pivot is on first left char
    public static string charGfxBottomA = @"\\_//";
    public static char[,] charGfxA = {
        {'F', '/', '^', 'F', '/'}, // ! Fix the F becuase \ broke the formatting
        {'F', 'F', '_', '/', '/'}
    };
    // Bottom String
    public static string charGfxTopB = @"|/*\|"; // * Pivot is on first left char
    public static string charGfxBottomB = @"/\_/\";
    public static char[,] charGfxB = {
         {'|', '/', '*', 'F', '|'}, // ! Fix the F becuase \ broke the formatting
         {'/', 'F', '_', '/', 'F'}
    };

    // = Game Status
    public static bool isRunning = true;
    static int currentLevelIndex;   // TODO Current Level Index to track current level 

    // = Game Setup
    public const int inputRefresh = 16;
    public const int inputDelay = 3;       // Pause between input detection | Also used to manage the Player movement speed
    public static int inputTimer;
    // = Movement Setup
    public static int verticalRange = 2;
    public static int horizontalRange = 2;

    // = Map Setup
    public static byte mapStartX = 0;   // Manually chose map X start position
    public static byte mapStartY = 6;   // Manually chose map Y start position
    public static byte mapLenghtX;
    public static byte mapLenghtY;

    // === Initialize Game Loops
    static public void GameStart()
    {
        GameInit();             // ! Move Map/Level stuff to MapInit();
        MapInit();              // ! Include LoadMap() here!GunHandler();    
        EntityInit();           // Entity Initializer
        InputHandler();         // START - Async.Task Loop - Handle Player Input
        ExplosionsHandler();    // START - Async.Task Loop - Handle Explosions Animation
        MovementHandler();      // START - Async.Task Loop - EntityManager Brain | Manage Movements & Shooting
        RenderLoop();           // START - while Main Loop - Handle Rendering Functions
    }

    // === METHODS
    static void GameInit()
    {
        // System Init
        Renderer.CalculateFrameRate();              // Calculate the Frame Rate
        EntityManager.CalculateBrainFrameRate();    // Calculate the Frame Rate of the Entity Manager Brain
        inputTimer = inputDelay;                    // Enable istant player input availability 

        // =============== MAP INIT
        // Map Setup
        // TODO (if actualLevelIndex == 0) LoadMap(0);
        // ! LoadMap(index);  // From Map.cs

        // Set Map size by window size // TODO Try to force a fixed windows size
        mapLenghtX = (byte)(WindowWidth - mapStartX); // ? Using -1 to avoid scroll bar
        mapLenghtY = (byte)(WindowHeight - mapStartY);
    }

    static void MapInit()
    {
        Map.MapInit(100, "New Map Name");
    }

    // === ENTITIES SETUP // ! TESTING PLACEHOLDER SOLUTION ! //
    static void EntityInit()
    {
        // DEBUG LOG
        WriteLine("|== DEBUG INFO ==|");
        WriteLine("- Players Initialization");

        // ! Add NewPlayerInit() | OR CHECK CONSTRUCTOR TO CREATE ISTANT QUICKLY

        // TODO Use AddEntity(int amount) method
        // for (amount)
        // Spawn

        // Setup First Player
        charLength = charGfxBottomA.Length;
        Entity player = new Entity();
        player.name = "Player_0";
        player.spawnX = mapStartX + mapLenghtX / 2 - 3;
        player.spawnY = mapStartY + mapLenghtY - 2;
        player.Instantiate(player.spawnX, player.spawnY, Entity.EntityType.Player, Entity.MoveType.Free, 1);
        player.gun = new Gun();
        player.gun.owner = player;
        player.gun.direction = player.DefineDirection(Entity.MoveType.Up);
        players.Add(player);
        player.gun.BulletHandler();

        // Setup Second Player
        charLength = charGfxBottomA.Length;
        player.name = "Player_1";
        Entity player2 = new Entity();
        player2.spawnX = mapStartX + mapLenghtX / 2 - 3 + 20;
        player2.spawnY = mapStartY + mapLenghtY - 2 - 6;
        player2.Instantiate(player2.spawnX, player2.spawnY, Entity.EntityType.Player, Entity.MoveType.Free, 1);
        player2.gun = new Gun();
        player2.gun.owner = player2;
        player2.gun.direction = player2.DefineDirection(Entity.MoveType.Up);
        players.Add(player2);
        player2.gun.BulletHandler();

        WriteLine("- Cars Initialization");
        // Setup Car
        Entity car = new Entity();
        car.name = "Car_0";
        car.Instantiate(mapLenghtX / 2, mapStartY + 2, Entity.EntityType.Enemy, Entity.MoveType.Left, 5);
        cars.Add(car);
        entities.Add(car);

        Entity car2 = new Entity();
        car2.name = "Car_2";
        car2.Instantiate(mapLenghtX / 2 - 20, mapStartY + 4, Entity.EntityType.Enemy, Entity.MoveType.Static, 1);
        cars.Add(car2);
        entities.Add(car2);

        Entity car3 = new Entity();
        car3.name = "Car_3";
        car3.Instantiate(mapLenghtX / 2 - 20, mapStartY + 8, Entity.EntityType.Enemy, Entity.MoveType.Left, 2);
        cars.Add(car3);
        entities.Add(car3);

        Entity car4 = new Entity();
        car4.name = "Car_4";
        car4.Instantiate(mapLenghtX - 30, mapStartY + 6, Entity.EntityType.Enemy, Entity.MoveType.Right, 1);
        cars.Add(car4);
        entities.Add(car4);

        Entity car5 = new Entity();
        car5.name = "Car_5";
        car5.Instantiate(mapLenghtX - 10, mapStartY + 2, Entity.EntityType.Enemy, Entity.MoveType.Down, 1);
        cars.Add(car5);
        entities.Add(car5);


        WriteLine("- Walls Initialization");
        // Setup Wall
        Entity wall = new Entity();
        wall.name = "Wall_0";
        wall.gfx = new char[,]{
            { 'W', 'W', 'W'},
            { 'W', 'W', 'W'}
        };
        wall.Instantiate(mapStartX + 2, mapStartY + 2, Entity.EntityType.Wall, Entity.MoveType.Static, 5);
        entities.Add(wall);

        WriteLine("- Water Initialization");
        // Setup Wall
        Entity water = new Entity();
        water.name = "Wall_0";
        water.gfx = new char[,]{
            { '-', '-', '-', '-', '-', '-'},
            { '-', '-', '-', '-', '-', '-'}
        };
        water.Instantiate(mapStartX + 2, mapLenghtY - 4, Entity.EntityType.Water, Entity.MoveType.Static, 5);
        waters.Add(water);
    }


}