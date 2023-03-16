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
    public void Init()
    {
        // TODO Move to TilesInit
        lenghtX = gfx.GetLength(1);
        lenghtY = gfx.GetLength(0);
        tiles = new Tile[lenghtX, lenghtY];

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                Tile newTile = new Tile();
                newTile.gfx = gfx[y,x];     // Read gfx char array and write the value to new Tile char gfx
                newTile.posX = posX + x;    // Offset the Tile position by the x index value
                newTile.posY = posY + y;    // Offset the Tile position by the y index value
                tiles[x, y] = newTile;      // Copy the new Tile to the tiles Tile
            }
        }
        DefineDirection(moveType);

        // Initialization Debug Log
        Console.WriteLine($"Entity name: {name} | Tiles array length {tiles} | Type: {entityType} | Move: {moveType} | Direction X:{direction.x}, Y:{direction.y}");
        Console.WriteLine($"X: {tiles.GetLength(0)} | Y: {tiles.GetLength(1)} | BrainFps: {moveDelay}");
        Console.ReadKey();
    }

    public void Spawn(int x, int y, EntityType newEntityType, MoveType newMoveType, int moveFps )
    {
        entityType = newEntityType;
        moveType = newMoveType;
        moveDelay = moveFps;
        posX = x;
        posY = y;
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
    public void MoveEntity(Direction newDirection)
    {
        // Move Entity  // ! Evaluate to struct Position x y instead of separate int
        posX += newDirection.x;
        posY += newDirection.y;
        // Move Tiles
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                tiles[x, y].Move(newDirection);
            }
        }
        canMove = false;    // Reset move capability
        moveTimer = 0;      // Reset moveTimer Entity move
    }

    Entity CheckCollision(Direction newDirection)
    {
        // if map next tile by actual tile + direction != null -> Collision

        // else collisionEntity = null;
        Entity? collisionEntity = null;

        return collisionEntity;
    }

    void CollisionHandler(Entity collisionEntity)
    {
        // Select action by typw Switch case and do Action

        // Foreach tile in tiles collision = CheckCollision(direction)    <- return Entity
        // if collision(Entity) != null
        // switch (EntityType)  
        // different behaviour by EntityType|CollisionType
    }

    void Die()
    {
        // Simply Destroy the entity | Tiles death, damage and explosion are managed in Tile class
    }

    // ! Add EnemyBrain() for shooting by shootDelay?

}