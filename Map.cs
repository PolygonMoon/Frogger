using static System.Console;

public static class Map
{
    // Map Status
    public static Tile[,] tiles = new Tile[1, 1];

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

    // public static void UpdateMapTile(Tile tileToUpdate, Direction oldPosition, Direction newPosition)
    // {
    //     // Debug Old Position
    //     ForegroundColor = ConsoleColor.Red;
    //     SetCursorPosition(tiles[oldPosition.x, oldPosition.y].posX, tiles[oldPosition.x, oldPosition.y].posY);
    //     Write("P");

    //     // Check X Map Limits
    //     if (newPosition.x >= Game.mapStartX + Game.mapLenghtX) newPosition.x = Game.mapStartX;
    //     else if (newPosition.x < Game.mapStartX) newPosition.x = Game.mapStartX + Game.mapLenghtX - 1;  // -1 is used to avoid scroll bar issues
    //     // Check Y Map Limits
    //     if (newPosition.y >= Game.mapStartY + Game.mapLenghtY) newPosition.y = Game.mapStartY;
    //     else if (newPosition.y < Game.mapStartY) newPosition.y = Game.mapStartY + Game.mapLenghtY - 1;  // -1 is used to avoid scroll bar issues


    //     tiles[oldPosition.x, oldPosition.y].posX = newPosition.x;
    //     tiles[oldPosition.x, oldPosition.y].posY = newPosition.y; ;
    //     //Debug New Position
    //     ForegroundColor = ConsoleColor.Red;
    //     SetCursorPosition(tiles[oldPosition.x, oldPosition.y].posX, tiles[oldPosition.x, oldPosition.y].posY);
    //     Write("#");
    // }

    public static Tile GetTile(int posX, int posY)
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