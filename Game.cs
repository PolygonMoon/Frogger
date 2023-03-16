using static System.Console;
using System.Threading.Tasks;
using static InputManager;
using static ShootManager;
using static Renderer;
using static UiTools;

public static class Game
{
    // = Player Status // ! Move to Entity.cs 
    public static int lifeCount;        // TODO lifeCount-- when all player Entity dies
    public static int lifeMax = 4;      // TODO Overide the value from Map.lifeMax
    // TODO int playerScore;    // Move to Score.cs ??
    public static int moveCounter = 0;  // Jump Counter

    // = Player GFX Setup   // * Players Gfx and Rendering is managed as ecxeption | Do not use norma Entity behaviour 
    public static int charLength;
    // Top String
    public static string charGfxTopA = @"\/^\/"; // * Pivot is on first left char
    public static string charGfxBottomA = @"\\_//";
    // Bottom String
    public static string charGfxTopB = @"|/*\|"; // * Pivot is on first left char
    public static string charGfxBottomB = @"/\_/\";

    // = Entity Manager
    public static List<Entity> players = new List<Entity>();
    public static List<Entity> entities = new List<Entity>();   // Temp, contain all entities
    public static List<Entity> cars = new List<Entity>();
    public static List<Entity> enemies = new List<Entity>();
    public static List<Entity> trunks = new List<Entity>();
    // ? Use a single Pickable entities List? (Coins, PowerUp, LifeUp, Etc...)
    public static List<Entity> coins = new List<Entity>();
    public static List<Entity> powerUps = new List<Entity>();

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
        RenderLoop();             // Main Loop - Handle Rendering Functions
    }

    // === METHODS
    static void GameInit()
    {
        // System Init
        Renderer.CalculateFrameRate();              // Calculate the Frame Rate
        inputTimer = inputDelay;                    // Enable istant player input availability 

        // =============== MAP INIT
        // Map Setup
        // TODO (if actualLevelIndex == 0) LoadMap(0);
        // ! LoadMap(index);  // From Map.cs
        mapLenghtX = (byte)(byte)((WindowWidth - mapStartX) - 1);
        mapLenghtY = (byte)(WindowHeight - mapStartY);
    }

    // === ENTITIES SETUP
    static void EntityInit()
    {
        // ! Add NewPlayerInit() | OR CHECK CONSTRUCTOR TO CREATE ISTANT QUICKLY
        // Setup First Player
        charLength = charGfxBottomA.Length;
        Entity player = new Entity();
        player.name = "Player_0";
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
        player.name = "Player_1";
        Entity player2 = new Entity();
        player2.spawnX = mapStartX + mapLenghtX / 2 - charLength / 2 - 1 + 20;
        player2.spawnY = mapStartY + mapLenghtY - 2 - 6;
        player2.Spawn(player2.spawnX, player2.spawnY, Entity.EntityType.Enemy, Entity.MoveType.Left);
        player2.Init();
        player2.gun = new Gun();
        player2.gun.owner = player2;
        players.Add(player2);
        player2.gun.BulletHandler();


        // Setup Car
        Entity car = new Entity();
        car.name = "Car_0";
        car.Spawn(mapLenghtX / 2, mapLenghtY / 2, Entity.EntityType.Enemy, Entity.MoveType.Left);
        car.Init();
        cars.Add(car);

        Entity car2 = new Entity();
        car2.name = "Car_2";
        car2.Spawn(mapLenghtX / 2 - 20, mapLenghtY / 2 - 6, Entity.EntityType.Enemy, Entity.MoveType.Left);
        car2.Init();
        cars.Add(car2);
    }


}