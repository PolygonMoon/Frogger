using static System.Console;

// === STRUCT // ! Move to Entity.EntyyiType.Goal
public struct Goal
{
    public int posX;
    public int posY;
}

public class Map
{
    // Map Status
    static Tile[,] tiles = new Tile[1, 1];

    // Map Setup
    static string name = "default map name";
    public static int lenghtX;
    public static int lenghtY;
    // Time Limit Setup
    static int timeLimit;
    static int time;

    public void MapInit(int newTimeLimit, string newName)
    {
        // Assign the size of the tileMap by window size
        tiles = new Tile[Game.mapLenghtX - 1, Game.mapLenghtY - 1]; // -1 is used to compensate tricks in other map size stuff in Game class
        timeLimit = newTimeLimit;
        name = newName;
    }


}