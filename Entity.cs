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
    public bool canMove = false;

    // Collision status
    Entity? collision = null;

    // Tiles Status
    public Tile[,]? tiles;

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
        Enemy,      // Used by Enemy & Car  | Kill Player if touched
        Player,     // Used by Player       | *Player*
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
        DefineDirection(moveType);
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
                Tile newTile = new Tile();
                newTile.parent = this;          // Set Tile Parent to this Entity
                newTile.tileType = entityType;
                // If type Player draw different Gfx
                if (entityType == EntityType.Player) newTile.gfx = charGfxA[y, x];
                else newTile.gfx = gfx[y, x];   // Read gfx char array and write the value to new Tile char gfx

                newTile.posX = posX + x;        // Offset the Tile position by the x index value
                newTile.posY = posY + y;        // Offset the Tile position by the y index value
                tiles[x, y] = newTile;          // Copy the new Tile to the tiles Tile
                mapInUse.SubscribeTile(newTile);
            }
        }
    }

    void DefineDirection(MoveType type)
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
    }

    // === RUN-TIME METHODS
    public bool MoveEntity(Direction newDirection)
    {
        bool isMoveValid = false;
        isMoveValid = CheckCollision(newDirection);
        //isMoveValid = true;

        // Move Tiles
        if (isMoveValid)
        {
            // Move Entity  // ! Evaluate to struct Position x y instead of separate int
            if (posX + newDirection.x < availableLenghtX
                && posY + newDirection.y < availableLenghtY
                && posX + newDirection.x > mapStartX
                && posY + newDirection.y > mapStartY)
            posX += newDirection.x;
            posY += newDirection.y;

            // Iterate within each entity tiles an move it
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    tiles[x, y].Move(newDirection);
                }
            }
            canMove = false;    // Reset move capability
            moveTimer = 0;      // Reset moveTimer Entity move
            return true;
        }
        else return false;
    }

    bool CheckCollision(Direction newDirection)
    {
        // Check the next tile status for each tile in tiles by movemenet direction
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                // ! NEED A FIX HERE, probably is not picking the right tile?
                // TODO Add a fucking permanet debug print of the next nextTile.entityType
                int tileXpos = tiles[x, y].posX;
                int tileYpos = tiles[x, y].posY;
                Tile nextTile = Map.tiles[tileXpos + newDirection.x - Game.mapStartX - 1, tileYpos + newDirection.y - Game.mapStartY - 1];

                if (nextTile != null && nextTile.tileType != entityType) return false;
                else
                {
                    // Collision Handler by Switch EntityType type
                }
            }
        }
        return true;
    }

    void CollisionHandler(Entity collisionEntity)
    {

    }

    void Die()
    {
        // Simply Destroy the entity | Tiles death, damage and explosion are managed in Tile class
    }

    // ! Add EnemyBrain() for shooting by shootDelay?

}