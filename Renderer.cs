using static System.Console;
using static Game;
using static EntityManager;
using static ShootManager;
using static UiTools;

public static class Renderer
{
    // = Renderer Setup
    static int frame = 0;
    static float frameRate;
    const int renderDelay = 8; // Renderer FPS | 16 = 60fps

    public static string activeEntity = "default entity";


    // === RENDERING LOOP
    public static void RenderLoop()  // Rendering Loops
    {
        while (isRunning)
        {
            frame++;    // Add a frame to the frame counter for debug purpose
            Clear();
            UiRenderer();
            //MapRenderer();
            BulletsRenderer();
            ExplosionsRenderer();
            EntitiesRenderer();
            PlayersRenderer();
            // CollisionCheck();
            Thread.Sleep(renderDelay);
        }
    }

    // == METHODS
    public static void CalculateFrameRate()
    {
        frameRate = (1.0f / renderDelay) * 1000f;
    }

    // === RENDERER
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
        Write($"EntityBrain FPS: {EntityManager.brainFps.ToString("0.00")}");

        SetCursorPosition(45, 0);
        Write($"P.Shoot:{players[0].gun.bullets.Count} / {players[0].gun.bulletAmount} | G.Shoot:{bullets.Count}");
        SetCursorPosition(45, 1);
        Write($"InputTimer:{inputTimer} / {inputDelay} | {InputManager.inputReady}");

        SetCursorPosition(75, 0);
        Write($"G.Explosion Count:{explosions.Count}");
        SetCursorPosition(75, 1);
        Write($"P.Shoot Timer:{players[0].gun.shootTimer} / {players[0].gun.shootDelay} | {players[0].gun.canShoot}");

        SetCursorPosition(0, 3);
        if (bullets.Count > 0) WriteLine("Rendering Bullets");
        SetCursorPosition(20, 3);
        if (explosions.Count > 0) WriteLine("Rendering Explosions");

        // Debug Entity
        SetCursorPosition(0, 4);
        WriteLine($"Entity Count: {entities.Count}");
        // SetCursorPosition(20, 4);    // ! This is used for debug by method call on enemy move event

        // Debug Player Position
        SetCursorPosition(0, availableLenghtY + mapStartY - 1);
        Write($"X: {players[0].posX} | Y: {players[0].posY}");
        Write($"|===| MapLimit X: {mapStartX},{availableLenghtX + mapStartX} | Y: {mapStartY},{availableLenghtY + mapStartY}");

        DrawLineH(0, WindowWidth, mapStartY - 1, "_");              // Draw Line for Debug Panel (Upper)

        // ! Avoid due to performance issue
        //DrawBoxEmpty(mapStartX, mapLenghtX, mapStartY, mapLenghtY); // Draw Map Border
        //DrawBox(10,20,5,10, "X"); // Test Box Draw
    }

    public static void DebugEvent(string text)
    {
        SetCursorPosition(20, 4);
        WriteLine($"DEBUG EVENT => {text}");
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
            // * New for loop
            for (int y = 0; y < cars[i].tiles.GetLength(0); y++)        // Iterate through each row(y)
            {
                for (int x = 0; x < cars[i].tiles.GetLength(1); x++)    // Iterate through each column(x) in the current row(y)
                {
                    SetCursorPosition(cars[i].tiles[y, x].posX, cars[i].tiles[y, x].posY);
                    Write(cars[i].tiles[y, x].gfx); // Read and Write char gfx value from the Tile
                }
                // Magic Trick to avoid ReadKey() input render inside near char | Used as debug input info
                SetCursorPosition(96, 0);
                Write($"Input: ");
            }

            // * Old foreach loop | using nested for loop instead of foreach | Better handle tiles destroy event at run-time
            // foreach (var tile in cars[i].tiles) 
            // {
            //     SetCursorPosition(tile.posX, tile.posY);
            //     Write(tile.gfx);    // Read char gfx value from the Tile
            // }
        }
        // ! Add different for loops to render Cars, Trunk, Enemy, Coins, PowerUp etc
        // ? First try to use a single pass renderer? | Use a generic entities.Count for loop

    }

    static void PlayersRenderer()    // ! We're using two string instead of a single char array for the players.
    {
        for (int i = 0; i < players.Count; i++)
        {
            // Draw player
            if (!InputManager.isMoving)
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