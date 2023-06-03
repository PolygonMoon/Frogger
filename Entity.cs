using static System.Console;
using static Game;

public struct Direction
{
    public int x;
    public int y;
}


public class Entity
{
    // Entity Status
    public int posX;
    public int posY;
    public Direction direction;
    public bool isAlive = true;
    public bool entityCanMove = false;

    // Collision status
    Entity? collision = null;   // ! NOT USED - Check if is needed

    // Tiles Status
    public Tile[,] tiles = new Tile[1, 1];

    // Spawn Setup  // ! Not needed ??? | Directly use posX,posY within spawn method
    public int spawnX;
    public int spawnY;

    // Entity Setup
    public string name = "default entity name";
    int hpMax;      // ! Not Needed ??? | Hp is manage by single tiles | USE AS BRIDGE ONLY -> pass the value to tiles
    public int moveTimer = 0;   // Timer for moveDelay
    public float moveDelay = 10;  // Pause between the moves | Define entity speed = Move frequency | 62 = 1s | 62 = 62 FPS
    public EntityType entityType;
    public MoveType moveType;
    bool haveCollision;
    bool canExplode;
    bool isBreakable;
    public Gun gun = new Gun();

    // Gfx Setup
    public int lenghtX;
    public int lenghtY;
    public char[,] gfx = {
         {'X', 'X', 'X'},
         {'X', 'X', 'X'}
    };

    // === ENUMS
    public enum EntityType
    {
        Wall,       // Used by Wall & Car   | Player can touch it without die | Block the movement collision check
        Walkable,   // Used by Sidewalks    | Anything can walk over
        Water,
        Enemy,      // Used by Enemy & Car  | Kill Player if touched
        Player,     // Used by Player       | *Player*
        Bullet,
        // ! Move Pickable to a new class?
        CoinUp,
        SpeedUp,
        AmmoUp,
        LifeUp,
        Goal        // Used as Map Goal     | Istantiate a Player gfx clone in Goal position & Teleport the Players to the spawn points
    }
    public enum MoveType
    {
        Left,       // X - 1 | Y nochange
        Right,      // X + 1 | Y nochange
        Down,       // X nochange | Y + 1
        Up,         // X nochange | Y - 1
        Static,     // No Movement
        Free,       // Free Movement   | Used for Players only // ! Not used | Player is managed as exception | Remove this?
        Random      // Random Movement | 
    }

    // === SETUP METHODS
    // ! Remove EntityType & MoveType and try to build something similar to prefab concept?
    public void Instantiate(int x, int y, EntityType newEntityType, MoveType newMoveType, int moveFps)
    {
        entityType = newEntityType;
        moveType = newMoveType;
        moveDelay = moveFps;
        posX = x;
        posY = y;

        TilesInit();
        direction = DefineDirection(moveType);
    }

    public void TilesInit()
    {
        lenghtX = gfx.GetLength(1);
        lenghtY = gfx.GetLength(0);
        if (entityType == EntityType.Player)
        {
            lenghtX = charGfxA.GetLength(1);
            lenghtY = charGfxA.GetLength(0);
        }
        tiles = new Tile[lenghtX, lenghtY];

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                Tile? newTile = new Tile();
                newTile.parent = this;              // Set Tile Parent to this Entity
                newTile.tileType = entityType;
                // If type Player draw different Gfx
                if (entityType == EntityType.Player) newTile.gfx = charGfxA[y, x];
                else newTile.gfx = gfx[y, x];       // Read gfx char array and write the value to new Tile char gfx

                newTile.posX = posX + x;            // Offset the Tile position by the x index value
                newTile.posY = posY + y;            // Offset the Tile position by the y index value
                newTile.indexX = x;                 // Index of the Tile inside the entity tiles array | x
                newTile.indexY = y;                 // Index of the Tile inside the entity tiles array | y
                tiles[x, y] = newTile;              // Copy the new Tile to the tiles Tile
                Map.SubscribeMapTile(newTile);      // Subscribe the new Tile within the MapTile
            }
        }
    }

    public Direction DefineDirection(MoveType type)
    {
        switch (type)
        {
            case MoveType.Left:
                direction = new Direction { x = -1, y = 0 };
                break;
            case MoveType.Right:
                direction = new Direction { x = 1, y = 0 };
                break;
            case MoveType.Down:
                direction = new Direction { x = 0, y = 1 };
                break;
            case MoveType.Up:
                direction = new Direction { x = 0, y = -1 };
                break;
            case MoveType.Static:
                direction = new Direction { x = 0, y = 0 };
                break;
            default:
                // Not managed case
                break;
        }
        return direction;
    }

    // === RUN-TIME METHODS
    public bool MovePlayer(Direction newDirection)
    {
        bool isMoveValid = false;
        isMoveValid = CheckEntityCollision(newDirection);
        //isMoveValid = true;

        // == MOVE PLAYER RULES
        if (isMoveValid)
        {
            // Map Limits Check | Stop Player Movement if adiacent to map limits
            if (posX < mapLenghtX + mapStartX
                && posY < mapLenghtY + mapStartY
                && posX > mapStartX
                && posY > mapStartY - 1)
            {
                // Move Player Pivot
                posX += newDirection.x;
                posY += newDirection.y;

                // Iterate within each entity tiles an UnSubscribe it
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    for (int y = 0; y < tiles.GetLength(1); y++)
                    {
                        // UnSubscribe the tile before the movement
                        Map.UnSubscribeMapTile(tiles[x, y]);
                    }
                }

                // Iterate within each entity tiles an move it
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    for (int y = 0; y < tiles.GetLength(1); y++)
                    {
                        tiles[x, y].Move(newDirection);

                        // Subscribe the tile after the movement
                        Map.SubscribeMapTile(tiles[x, y]);
                    }
                }
                entityCanMove = false;    // Reset move capability
                moveTimer = 0;      // Reset moveTimer Entity move
                return true;
            }
        }
        return false;
    }

    public void MoveEntity(Direction newDirection)
    {
        // Declare a starting false move validity | Will be overwritten by collision check result
        bool isMoveValid = false;
        // Check Entity next movement collision by running a collision check to all entitiy tiles
        isMoveValid = CheckEntityCollision(newDirection);
        //isMoveValid = true;   // * Used for Debug purpose only

        // == MOVE ENTITY RULES
        if (isMoveValid)
        {
            // Map Limits Check | Teleport Entities if out of the map
            if (posX + newDirection.x >= mapStartX + mapLenghtX) posX = mapStartX;
            else if (posX + newDirection.x < mapStartX) posX = mapStartX + mapLenghtX - 1;  // -1 is used to avoid scroll bar issues
            if (posY + newDirection.y >= mapStartY + mapLenghtY) posY = mapStartY - 1;
            else if (posY + newDirection.y < mapStartY) posY = mapStartY + mapLenghtY - 1;  // -1 is used to avoid scroll bar issues

            // Move Entity Pivot
            posX += newDirection.x;
            posY += newDirection.y;

            // Iterate within each entity tiles an UnSubscribe it
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    // UnSubscribe the tile before the movement
                    Map.UnSubscribeMapTile(tiles[x, y]);
                }
            }

            // Iterate within each entity tiles an Move + Subscribe it
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    tiles[x, y].Move(newDirection);

                    // Subscribe the tile after the movement
                    Map.SubscribeMapTile(tiles[x, y]);
                }
            }
            entityCanMove = false;  // Reset move capability
            moveTimer = 0;          // Reset moveTimer Entity move
            // MOVEMENT COMPLETE

            // TODO ! Add an extra method to destroy entity after ste into whater (only if moving entity is Player)
            // Late Collision Check | Used to destroy entity after step into wather
        }
        // MOVEMENT NOT POSSIBLE | Due to collision check
    }

    bool CheckEntityCollision(Direction newDirection)
    {
        bool canTilesMove = true;

        // Iterate within Entity Tiles and check next tile status by movemenet direction for each tile
        for (int y = 0; y < tiles.GetLength(1); y++)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                // Check if actual entity tile can move due to collision check result
                if (tiles[x, y] != null) canTilesMove = tiles[x, y].CheckTileCollision(newDirection);
                if (!canTilesMove) return false;
            }
        }
        // If no collisions are detected => All tiles can move = Entity can move
        return true;
    }

    public void Die()
    {
        // Iterate through tiles and Explode them | Tiles death, damage and explosion are managed in Tile class
        for (int y = 0; y < tiles.GetLength(1); y++)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                if (tiles[x, y] != null)
                {
                    tiles[x, y].Explode(tiles[x, y]);
                    // Set the tile in the entity tiles array as null
                    //tiles[x, y] = null;   // ! Not needed ???  Already set as null inside the Explode => Unsubscribe Method 
                }
            }
        }
        // Remove the Entity from the entities List
        if (entityType == EntityType.Enemy)EntityManager.entities.Remove(this);
        if (entityType == EntityType.Player) EntityManager.players.Remove(this);
    }

    // ! Add EnemyBrain() for shooting by shootDelay?

}