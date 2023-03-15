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

    // Collision status
    Entity collision;

    // Tiles Status
    public Tile[,] tiles;

    // Spawn Setup  // ! Not needed ??? | Directly use posX,posY within spawn method
    public int spawnX;
    public int spawnY;

    // Entity Setup
    int hpMax;      // ! Not Needed ??? | Hp is manage by single tiles | USE AS BRIDGE ONLY -> pass the value to tiles
    int moveDelay;
    public EntityType entityType;
    public MoveType moveType;
    bool haveCollision;
    bool canExplode;
    bool isBreakable;
    public Gun gun = new Gun();

    // Gfx Setup
    public int lenghtX;
    public int lenghtY;
    public string GfxTop = "XXX";
    public string GfxBottom = "XXX";

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
        LifeUp
    }
    public enum MoveType
    {
        Left,       // X - 1 | Y nochange
        Right,      // X + 1 | Y nochange
        Down,       // X nochange | Y + 1
        Up,         // X nochange | Y - 1
        Static,     // No Movement
    }

    // === METHODS
    public void Init()
    {
        // TODO Move to TilesInit
        lenghtX = GfxTop.Length;
        lenghtY = 2;
        tiles = new Tile[lenghtX, lenghtY];
        Console.WriteLine($"Tiles Array {tiles}");
        Console.WriteLine($"X {tiles.GetLength(0)}, Y {tiles.GetLength(1)}");
        Console.ReadKey();
        // for loop | assign tile in tiles by reading gfx string

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                Tile newTile = new Tile();
                newTile.gfx = "X";
                newTile.posX = posX + x;
                newTile.posY = posY + y;
                tiles[x, y] = newTile;
            }
        }

        DefineDirection(entityType);
    }

    public void Spawn(int x, int y, EntityType entityType, MoveType moveType)
    {
        posX = x;
        posY = y;
    }

    void DefineDirection(EntityType type)
    {
        // TODO Setup direction by EntityType within a Switch case
    }

    public void MoveEntity(Direction newDirection)
    {
        // TODO Start Task.Run with asynch moveDelay

        collision = CheckCollision(newDirection);
        //
        if (collision == null)
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
        }
        else CollisionHandler(collision);
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



    // ! Add EnemyShootHandler()  | use Gun istance var to 


}