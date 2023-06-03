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
    const int renderDelay = 16; // Renderer FPS | 16 = 60fps

    public static string activeEntity = "default entity";

    // === RENDERING LOOP
    public static void RenderLoop()  // Rendering Loops
    {
        while (isRunning)
        {
            frame++;    // Add a frame to the frame counter for debug purpose
            Clear();
            UiRenderer();
            WaterRenderer();
            BulletsRenderer();
            ExplosionsRenderer();
            //CollisionPreviewRenderer();         // ! Need some fix
            EntitiesRenderer();
            PlayersRenderer();
            //DebugMapTilePositionRenderer();   // DEBUG FOR TILE IN TILE MAP POSITION SYNCH
            //DebugMapTileRenderer();           // DEBUG FOR TILE PRESENCE (@)
            DebugPivotRenderer();               // DEBUG FOR PIVOT POSITION
            //CollisionCheck();                 // ! Don't Exist, why is still here?
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

        // Debug Entity Info
        SetCursorPosition(0, 4);
        WriteLine($"Entity Count: {entities.Count}");

        // Debug TileMap Info
        SetCursorPosition(20, 4);
        WriteLine($"Map tiles.Length {Map.tiles.Length} | Score: | Life: | Time:");

        // Debug Player Info
        if (players.Count > 0)
        {
            SetCursorPosition(45, 0);
            if (players.Count > 0) Write($"P.Shoot:{players[0].gun.bullets.Count} / {players[0].gun.bulletAmount} | G.Shoot:{bullets.Count}");
            SetCursorPosition(45, 1);
            Write($"InputTimer:{inputTimer} / {inputDelay} | {InputManager.inputReady}");

            SetCursorPosition(75, 0);
            Write($"G.Explosion Count:{explosions.Count}");
            SetCursorPosition(75, 1);
            if (players.Count > 0) Write($"P.Shoot Timer:{players[0].gun.shootTimer} / {players[0].gun.shootDelay} | {players[0].gun.canShoot}");

            SetCursorPosition(0, 3);
            if (bullets.Count > 0) WriteLine("Rendering Bullets");
            SetCursorPosition(20, 3);
            if (explosions.Count > 0) WriteLine("Rendering Explosions");

            // SetCursorPosition(20, 4);    // ! This is used for debug by method call on enemy move event

            // Debug Player Position
            SetCursorPosition(0, mapLenghtY + mapStartY - 2);
            if (players.Count > 0) Write($"P.0 Pos X: {players[0].posX} | Y: {players[0].posY} | E.0 Pos X: {entities[0].posX} | Y: {entities[0].posY}");
        }
        else
        {
            SetCursorPosition(0, mapLenghtY + mapStartY - 2);
            Write($"=-NO PLAYERS ALIVE-= | E.0 Pos X: {entities[0].posX} | Y: {entities[0].posY}");
        }

        SetCursorPosition(0, mapLenghtY + mapStartY - 1);
        Write($"|===| MapLimit X: {mapStartX},{mapLenghtX + mapStartX} | Y: {mapStartY},{mapLenghtY + mapStartY}");

        DrawLineH(0, WindowWidth, mapStartY - 1, "_");              // Draw Line for Debug Panel (Upper)

        // ! Avoid due to performance issue
        //DrawBoxEmpty(mapStartX, mapLenghtX, mapStartY, mapLenghtY); // Draw Map Border
        //DrawBox(10,20,5,10, "X"); // Test Box Draw
    }

    public static void DebugEvent(string text)
    {
        SetCursorPosition(mapLenghtX - 40, mapLenghtY + mapStartY - 1);
        Write($"DEBUG EVENT => {text}");
    }

    static void BulletsRenderer()
    {
        // Shoots rendering
        if (bullets.Count > 0)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].isAlive) bullets.RemoveAt(i);
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
                if (!explosions[i].isExploded) Write(explosionGfxStart); // ! Check index out of range error
                else Write(explosionGfxEnd);
            }
        }
    }

    static void DebugMapTileRenderer()     // Show Tile over MapTile array 
    {
        // Iterate within MapTile array and draw the tile type content fore each array
        for (int y = 0; y < Map.tiles.GetLength(1); y++)        // Iterate through each row(y)
        {
            for (int x = 0; x < Map.tiles.GetLength(0); x++)    // Iterate through each column(x) in the current row(y)
            {
                if (Map.tiles[x, y] != null)
                {
                    SetCursorPosition(x, y);
                    Write("@");
                }
            }
            UiInputRender();
        }
    }

    static void CollisionPreviewRenderer()
    {
        // Save a copy of the original tiles array in order to avoid asynch changed during the rendering nested loop
        Array.Copy(Map.tiles, Map.tilesCopy, Map.tiles.Length);

        for (int y = 0; y < Map.tilesCopy.GetLength(1); y++)        // Iterate through each row(y)
        {
            for (int x = 0; x < Map.tilesCopy.GetLength(0); x++)    // Iterate through each column(x) in the current row(y)
            {
                if (Map.tilesCopy[x, y] != null)
                {
                    Tile? tileToCheck = Map.tilesCopy[x, y];
                    if (tileToCheck.parent != null)     // Prevent possible null entity parent of tile
                    {
                        if (tileToCheck.posX + tileToCheck.parent.direction.x < mapLenghtX + mapStartX - 1
                            && tileToCheck.posY + tileToCheck.parent.direction.y < mapLenghtY + mapStartY - 1
                            && tileToCheck.posX + tileToCheck.parent.direction.x > mapStartX
                            && tileToCheck.posY + tileToCheck.parent.direction.y > mapStartY)
                        {
                            ForegroundColor = ConsoleColor.Red;
                            SetCursorPosition(tileToCheck.posX + tileToCheck.parent.direction.x, tileToCheck.posY + tileToCheck.parent.direction.y);
                            // ! Check missing reference error | looks fine | using MapTile array copy just for renderer
                            // ! CHECK WRONG POSITION RENDERING | looks fine but its really slow
                            Write("#");
                        }
                    }
                }
                ForegroundColor = ConsoleColor.Gray;
            }
        }
    }

    static void DebugMapTilePositionRenderer()
    {
        ForegroundColor = ConsoleColor.DarkRed;
        SetCursorPosition(mapLenghtX - 1, mapLenghtY - 1);
        Write("T");
        SetCursorPosition(mapStartX + mapLenghtX - 1, mapStartY + mapLenghtY - 1);
        Write("G");
        ForegroundColor = ConsoleColor.DarkGreen;

        foreach (var tile in Map.tiles)
        {
            if (tile != null && tile.parent != null)
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
                    //Write("+");
                }
            }
        }
        ForegroundColor = ConsoleColor.Gray;
    }

    static void DebugPivotRenderer()
    {
        // Debug Entities Pivot
        if (entities.Count > 0)
        {
            ForegroundColor = ConsoleColor.DarkYellow;
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].posX < mapLenghtX
                    && entities[i].posY < mapLenghtY + mapStartY
                    && entities[i].posX > mapStartX
                    && entities[i].posY > mapStartY - 1)
                {
                    SetCursorPosition(entities[i].posX, entities[i].posY);
                    Write("P");
                }
            }
        }
        // Debug Players Pivot
        if (players.Count > 0)
        {
            ForegroundColor = ConsoleColor.DarkGreen;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].posX < mapLenghtX
                    && players[i].posY < mapLenghtY + mapStartY
                    && players[i].posX > mapStartX
                    && players[i].posY > mapStartY - 1)
                {
                    SetCursorPosition(players[i].posX, players[i].posY);
                    Write("P");
                }
            }
        }
        ForegroundColor = ConsoleColor.Gray;
        UiInputRender();
    }

    static void WaterRenderer()
    {
        //BackgroundColor = ConsoleColor.DarkBlue;
        ForegroundColor = ConsoleColor.DarkBlue;
        for (int i = 0; i < waters.Count; i++)
        {
            // * New for loop //
            // ! Check if cars count > 0 ??? | Check if cars[i].tiles != null ???
            for (int y = 0; y < waters[i].tiles.GetLength(0); y++)        // Iterate through each row(y)
            {
                for (int x = 0; x < waters[i].tiles.GetLength(1); x++)    // Iterate through each column(x) in the current row(y)
                {
                    SetCursorPosition(waters[i].tiles[y, x].posX, waters[i].tiles[y, x].posY);
                    Write(waters[i].tiles[y, x].gfx); // Read and Write char gfx value from the Tile
                }
                UiInputRender();
            }
        }
        //BackgroundColor = ConsoleColor.Black;
        ForegroundColor = ConsoleColor.Gray;
    }

    static void EntitiesRenderer()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            // * New for loop //
            // ! Check if cars count > 0 ??? | Check if cars[i].tiles != null ???
            for (int y = 0; y < entities[i].tiles.GetLength(0); y++)        // Iterate through each row(y)
            {
                for (int x = 0; x < entities[i].tiles.GetLength(1); x++)    // Iterate through each column(x) in the current row(y)
                {
                    SetCursorPosition(entities[i].tiles[y, x].posX, entities[i].tiles[y, x].posY);
                    Write(entities[i].tiles[y, x].gfx); // Read and Write char gfx value from the Tile
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