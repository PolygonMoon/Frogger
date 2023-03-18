using static System.Console;

public class Map
{
    // Map Status
    public static Tile[,] tiles = new Tile[1, 1];

    // Map Path
    public List<Path> paths = new List<Path>();

    // Map Setup
    static string name = "default map name";
    public static int lenghtX;
    public static int lenghtY;
    // Time Limit Setup
    static int timeLimit;
    static int time;

    public void Load(int mapIndex)
    {
        // Load a Map from a Map list | the index is the level progression
    }

    public void LoadRandomMap()
    {
        // TODO Manage Random Map Setup
        // TODO Random Path Setup
    }

    public void MapInit(int newTimeLimit, string newName)
    {
        lenghtX = Game.mapLenghtX - 1;    // -1 is used to compensate tricks in other map size stuff in Game class
        lenghtY = Game.mapLenghtY - 1;    // -1 is used to compensate tricks in other map size stuff in Game class

        // Assign the size of the tileMap by map size => available length from Game.Init()
        tiles = new Tile[lenghtX, lenghtY];
        timeLimit = newTimeLimit;
        name = newName;
    }

    void Populate()
    {
        // TODO Check the leghtY value and find the maxPathCount available space
        // TODO Remember to consider the start empty path and the final goals path

        // TODO For other available Path space pick a random Path from another Path prefab list? | Check the best system to do that
    }

    public void SubscribeTile(Tile newTile)
    {
        int x = newTile.posX - Game.mapStartX - 1;
        int y = newTile.posY - Game.mapStartY - 1;
        //int x = 20;
        //int y = 10;

        tiles[x, y] = newTile;
        // ! DO THIS FIRST
        // TODO Refresh map tiles by all entities actual position
        // TODO for entities[e].tiles[i] 
    }

    public static Tile GetTile (int posX, int posY)
    {
        Tile nextTile = tiles[posX, posY];
        return nextTile;
    }
}

public class Path
{
    int lengthX;
    int lengthY;
    int entitiesAmountMax;
    int entitiesSpacingMin;

    public List<Entity> entities = new List<Entity>();

    public static Path PopulatePath(Path pathPrefab)
    {
        Path newPath = pathPrefab;
        // TODO the entities count
        // TODO calculate the entities total length
        // TODO check if the entitiesTotalLengthX < maplengthX
        // TODO calculate the spacing available
        // TODO check if available spacing > entitySpacingMin | if not reduce the amount of entities while (available spacing > entitySpacingMin)

        for (int i = 0; i < newPath.entities.Count; i++)
        {
            //entities[i].Instantiate()
            // ! entities list should work like a prefab? | Check for similar solution
            // TODO add each entities to the map tiles
        }
        return newPath;
    }
}