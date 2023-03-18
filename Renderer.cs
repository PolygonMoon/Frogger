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
            BulletsRenderer();
            ExplosionsRenderer();
            EntitiesRenderer();
            PlayersRenderer();
            //DebugTileMapTilesRenderer();    // * DEBUG FOR TILE IN TILE MAP POSITION SYNCH
            DebugPivotRenderer();           // * DEBUG FOR PIVOT POSITION
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

        // Debug TileMap
        SetCursorPosition(20, 4);
        WriteLine($"Map tiles.Length {Map.tiles.Length} | Temp.Car Pivot Tile position");

        // SetCursorPosition(20, 4);    // ! This is used for debug by method call on enemy move event

        // Debug Player Position
        SetCursorPosition(0, mapLenghtY + mapStartY - 2);
        Write($"X: {players[0].posX} | Y: {players[0].posY} | X: {entities[0].posX} | Y: {entities[0].posY}");
        SetCursorPosition(0, mapLenghtY + mapStartY - 1);
        Write($"|===| MapLimit X: {mapStartX},{mapLenghtX + mapStartX} | Y: {mapStartY},{mapLenghtY + mapStartY}");

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

    static void DebugTileMapTilesRenderer()
    {
        ForegroundColor = ConsoleColor.DarkRed;
        SetCursorPosition(Map.lenghtX, Map.lenghtY);
        Write("T");
        SetCursorPosition(mapLenghtX - 1 + mapStartX, mapLenghtY - 1 + mapStartY);
        Write("G");
        ForegroundColor = ConsoleColor.DarkGreen;

        foreach (var tile in Map.tiles)
        {
            if (tile != null)
            {
                ForegroundColor = ConsoleColor.White;
                SetCursorPosition(tile.posX, tile.posY);
                Write("T");
                if (tile.posX + tile.parent.direction.x < mapLenghtX + mapStartX - 1
                && tile.posY + tile.parent.direction.y < mapLenghtY + mapStartY - 1
                && tile.posX + tile.parent.direction.x > mapStartX
                && tile.posY + tile.parent.direction.y > mapStartY)
                {
                    ForegroundColor = ConsoleColor.DarkGreen;
                    SetCursorPosition(tile.posX + tile.parent.direction.x, tile.posY + tile.parent.direction.y);
                    Write("+");

                }
            }
        }
        ForegroundColor = ConsoleColor.Gray;
    }

    static void DebugPivotRenderer()
    {
        ForegroundColor = ConsoleColor.DarkYellow;
        for (int i = 0; i < entities.Count; i++)
        {
            if (entities[i].posX < mapLenghtX
                && entities[i].posY < mapLenghtY + mapStartY
                && entities[i].posX > mapStartX
                && entities[i].posY > mapStartY)
            {
                SetCursorPosition(entities[i].posX, entities[i].posY);
                Write("P");
            }
        }
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].posX < mapLenghtX
                && players[i].posY < mapLenghtY + mapStartY
                && players[i].posX > mapStartX
                && players[i].posY > mapStartY)
            {
                SetCursorPosition(players[i].posX, players[i].posY);
                Write("P");
            }
        }
        ForegroundColor = ConsoleColor.Gray;
        UiInputRender();
    }

    static void EntitiesRenderer()
    {
        for (int i = 0; i < cars.Count; i++)
        {
            // * New for loop //
            // ! Check if cars count > 0 ??? | Check if cars[i].tiles != null ???
            for (int y = 0; y < cars[i].tiles.GetLength(0); y++)        // Iterate through each row(y)
            {
                for (int x = 0; x < cars[i].tiles.GetLength(1); x++)    // Iterate through each column(x) in the current row(y)
                {
                    SetCursorPosition(cars[i].tiles[y, x].posX, cars[i].tiles[y, x].posY);
                    Write(cars[i].tiles[y, x].gfx); // Read and Write char gfx value from the Tile
                }
                UiInputRender();
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
        UiInputRender();
    }

    static void UiInputRender()
    {
        // Magic Trick to avoid ReadKey() input render inside near char | Used as debug input info
        SetCursorPosition(96, 0);
        Write($"Input: ");
    }
}