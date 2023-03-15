using static System.Console;
using System.Threading.Tasks;
using static UiTools;

public static class ShootManager
{
    // Bullet Setup // ! Move to Bullet class
    public static string shootGfx = "|";
    public static string explosionGfxStart = "+";
    public static string explosionGfxEnd = "X";

    
    // Explosion Setup
    public const int explosionDelay = 5;        // Explosion first animation phase duration
    public const int explosionDuration = 12;    // Explosion total effect duration
    public const int explosionRefresh = 20;     // Explosion Status Refresh Rate

    // ? Move to Explosion class | Maybe later
    public struct Explosion
    {
        public int posX;
        public int posY;
        public bool isExploded;
        public int explosionTimer;
    }

    // Global Bullets List
    public static List<Bullet> bullets = new List<Bullet>();
    // Global Explosion List
    public static List<Explosion> explosions = new List<Explosion>();

    // === TASKS & LOOPS
    
    public static void ExplosionsHandler()
    {
        Task.Run(async () =>
               {
                   while (Game.isRunning)
                   {
                       if (explosions.Count > 0)
                       {
                           for (int i = 0; i < explosions.Count; i++)
                           {
                               if (explosions[i].explosionTimer < explosionDuration)
                               {
                                   int newTimer = explosions[i].explosionTimer + 1;

                                   Explosion updatedExplosion = new Explosion
                                   {
                                       posX = explosions[i].posX,
                                       posY = explosions[i].posY,
                                       isExploded = false,
                                       explosionTimer = newTimer
                                   };
                                   if (newTimer > explosionDelay) updatedExplosion.isExploded = true;
                                   explosions[i] = updatedExplosion;
                               }
                               else explosions.RemoveAt(i);
                           }
                       }
                       await Task.Delay(explosionRefresh);
                   }
               });
    }

    // === METHODS
    public static void NewExplosion(int spawnX, int spawnY)
    {
        Explosion newExplosion = new Explosion();
        newExplosion.posX = spawnX;
        newExplosion.posY = spawnY;
        newExplosion.isExploded = false;
        newExplosion.explosionTimer = 0;
        explosions.Add(newExplosion);
        // Debug Log
        SetCursorPosition(20, 2);
        Write("Spawning New Explosions");
    }
}