using static System.Console;
using System.Threading.Tasks;
using static UiTools;

public static class ShootManager
{
    // Bullet Setup
    public static string shootGfx = "|";
    public static string explosionGfxStart = "+";
    public static string explosionGfxEnd = "X";

    // Gun Status
    public static bool canShoot = true;
    public static int shootTimer = 0;

    // Gun Setup
    public const int maxShoots = 5;        // Max sontemporary shoots count
    public static int shootSpeed = 16;     // Milliseconds between shoot movement
    public const int shootDelay = 5;
    // Explosion Setup
    public const int explosionDelay = 5;        // Explosion first animation phase duration
    public const int explosionDuration = 12;    // Explosion total effect duration
    public const int explosionRefresh = 20;     // Explosion Status Refresh Rate

    // === STRUCT
    // ! Move to Bullet class
    public struct Shoot
    {
        public int posX;
        public int posY;
        public bool isPlayer;
    }

    public struct Explosion
    {
        public int posX;
        public int posY;
        public bool isExploded;
        public int explosionTimer;
    }

    // Shoots List
    public static List<Shoot> shoots = new List<Shoot>();
    // Explosion List
    public static List<Explosion> explosions = new List<Explosion>();

    // === TASKS & LOOPS
    public static void ShootsHandler()
    {
        Task.Run(async () =>
               {
                   while (Game.isRunning)
                   {
                       if (shootTimer < shootDelay) shootTimer++;
                       if (shootTimer >= shootDelay) canShoot = true;

                       if (shoots.Count > 0)
                       {
                           for (int i = 0; i < shoots.Count; i++)
                           {
                               // Collision Detection
                               if (shoots[i].posY > Game.mapStartY - 1 && shoots[i].posY < Game.mapStartX) //! need -1?
                               {
                                   int newPos;
                                   if (shoots[i].isPlayer) newPos = shoots[i].posY - 1;
                                   else newPos = shoots[i].posY + 1;
                                   Shoot updatedShoot = new Shoot { posX = shoots[i].posX, posY = newPos };
                                   shoots[i] = updatedShoot;
                               }
                               else
                               {
                                   NewExplosion(shoots[i].posX, shoots[i].posY);
                                   shoots.RemoveAt(i);
                               }
                           }
                       }
                       await Task.Delay(shootSpeed);
                   }
               });
    }
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

    public static void NewShoot(int spawnX, int spawnY)
    {
        if (canShoot)
        {
            Shoot newShoot = new Shoot();
            newShoot.posX = spawnX;
            newShoot.posY = spawnY;
            shoots.Add(newShoot);
            // Reset shooting capability 
            canShoot = false;
            shootTimer = 0;
            // Debug Log
            SetCursorPosition(0, 2);
            Write("Spawning New Shoot");
        }
    }

}