using static System.Console;
using static Game;


public static class EntityManager
{
    // = Brain Setup
    const int brainDelay = 64; // Define Brain Refresh Delay | Default value 16ms = 60 FPS
    public static float brainFps;

    // = Entity Manager
    public static List<Entity> players = new List<Entity>();
    public static List<Entity> entities = new List<Entity>();   // Temp, contain all entities
    public static List<Entity> cars = new List<Entity>();
    public static List<Entity> enemies = new List<Entity>();
    public static List<Entity> trunks = new List<Entity>();
    // ? Use a single Pickable entities List? (Coins, PowerUp, LifeUp, Etc...)
    public static List<Entity> coins = new List<Entity>();
    public static List<Entity> powerUps = new List<Entity>();

    // === BRAIN LOOP THREAD
    public static void MovementHandler()
    {
        Task.Run(async () =>
               {
                   while (Game.isRunning)
                   {
                       // ! Updating MapTile array with an new empty array | TESTING PURPOSTE
                       // Tiles will be ReSubscribe to the maptile within the MoveEntity() method
                       //    Map.tiles = new Tile[Game.mapStartX + Game.mapLenghtX, Game.mapStartY + Game.mapLenghtY];
                       //    for (int e = 0; e < entities.Count; e++)    // Force all tiles subscribing for each entity every entity brain update
                       //    {
                       //        if (entities[e].isAlive)     // Check if entity is alive
                       //        {
                       //            foreach (var tile in entities[e].tiles)
                       //            {
                       //                Map.SubscribeMapTile(tile);
                       //            }
                       //        }
                       //    }

                       // Move all alive entities
                       for (int e = 0; e < entities.Count; e++)    // Iterate through entities list
                       {
                           if (entities[e].isAlive)     // Check if entity is alive
                           {
                               if (entities[e].moveTimer < entities[e].moveDelay) entities[e].moveTimer++;
                               if (entities[e].moveTimer >= entities[e].moveDelay) entities[e].entityCanMove = true;

                               if (entities[e].entityCanMove)
                               {
                                   entities[e].MoveEntity(entities[e].direction);
                               }
                           }
                       }
                       await Task.Delay(brainDelay);
                   }
               });
    }

    // === METHODS
    public static void CalculateBrainFrameRate()
    {
        brainFps = (1.0f / brainDelay) * 1000f;
    }

}