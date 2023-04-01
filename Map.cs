using static System.Console;

public static class Map
{
    // Map Status
    public static Tile?[,] tiles = new Tile?[1, 1];
    public static Tile[,] tilesCopy = new Tile[1, 1];    // ! CHECK if is still needed after Subscribe/UnSubscribe implementation

    // Map Path
    public static List<Path> paths = new List<Path>();

    // Map Setup
    static string name = "default map name";

    // Time Limit Setup
    static int timeLimit;
    static int time;

    public static void Load(int mapIndex)
    {
        // Load a Map from a Map list | the index is the level progression
    }

    public static void LoadRandomMap()
    {
        // TODO Manage Random Map Setup
        // TODO Random Path Setup
    }

    public static void MapInit(int newTimeLimit, string newName)
    {
        // Assign the size of the tileMap by map size => available length from Game.Init()
        tiles = new Tile[Game.mapStartX + Game.mapLenghtX, Game.mapStartY + Game.mapLenghtY];
        tilesCopy = new Tile[Map.tiles.GetLength(0), Map.tiles.GetLength(1)];
        timeLimit = newTimeLimit;
        name = newName;
    }

    static void Populate()
    {
        // TODO Check the leghtY value and find the maxPathCount available space
        // TODO Remember to consider the start empty path and the final goals path

        // TODO For other available Path space pick a random Path from another Path prefab list? | Check the best system to do that
    }

    public static void SubscribeMapTile(Tile tileToSubscribe)
    {
        int x = tileToSubscribe.posX;
        int y = tileToSubscribe.posY;

        tiles[x, y] = tileToSubscribe;
    }

    public static void UnSubscribeMapTile(Tile tileToUnSubscribe)
    {
        int x = tileToUnSubscribe.posX;
        int y = tileToUnSubscribe.posY;

        tiles[x, y] = null;
    }

    public static Tile? GetTile(int posX, int posY)
    {
        Tile? nextTile = tiles[posX, posY];
        if (nextTile != null) return nextTile;
        else return null;
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