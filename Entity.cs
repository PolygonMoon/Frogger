


public class Entity
{
    // Entity Status
    public int posX;
    public int posY;
    Entity collision;
    int[] direction = new int[2];
    public Tile[,] tiles;

    // Entity Setup
    int hpMax;
    public EntityType entityType;
    public MoveType moveType;
    int moveDelay;

    bool haveCollision;
    bool canExplode;
    bool isBreakable;
    Gun? gun;
    // Gfx Setup
    public int lenghtX;
    public int lenghtY;
    public string GfxTop = "XXX";
    public string GfxBottom = "XXX";

    // === ENUMS
    public enum EntityType
    {
        Wall,       // Used by Wall
        Walkable,   // Used by Sidewalks
        Enemy,      // Used by Enemy & Bullet & Car
        Player,     // Used by Player   
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

    public void Spawn(int[] spawnPosition, EntityType entityType, MoveType moveType)
    {
        posX = spawnPosition[0];
        posY = spawnPosition[1];
    }

    void DefineDirection(EntityType type)
    {
        // TODO Setup direction by EntityType within a Switch case
    }

    public void MovementHandler()
    {
        // TODO Start Task.Run with asynch moveDelay

        collision = CheckCollision(direction);
        if (collision == null) Move(); //! RE ADD Move(direction)
        else CollisionHandler(collision);
    }

    Entity CheckCollision(int[] direction)
    {
        // if map next tile by actual tile + direction != null -> Collision

        // else collisionEntity = null;
        Entity? collisionEntity = null;

        return collisionEntity;
    }

    void CollisionHandler(Entity collisionEntity)
    {
        // Select action by typw Switch case and do Action
    }

    public void Move() // ! temp remove (int[] direction)
    {
        // ! Move each tile of the Entity by direction
        // foreach tiles move by direction



        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                tiles[x, y].posX -= 1;
            }
        }
    }

    void Explode()
    {

    }

    void DealDamage(int damage) // ! damage value from gun
    {

    }



    // ! Add EnemyShootHandler()  | use Gun istance var to 


}