using static System.Console;
using System.Threading.Tasks;
using static UiTools;

public static class ShootManager
{
    // Bullet Setup // ! Move to Bullet class
    public static string shootGfx = "|";
    public static string explosionGfxStart = "+";
    public static string explosionGfxEnd = "X";

    // Gun Status
    public static bool canShoot = true;
    public static int shootTimer = 0;

    // Gun Setup    // ! Move to Gun class
    public const int maxBullets = 5;        // Max sontemporary bullets count
    public static int bulletSpeed = 30;     // 16 | Milliseconds between bullets movement
    public const int shootDelay = 5;        // Milliseconds between bullets shoot
    // Explosion Setup
    public const int explosionDelay = 5;        // Explosion first animation phase duration
    public const int explosionDuration = 12;    // Explosion total effect duration
    public const int explosionRefresh = 20;     // Explosion Status Refresh Rate

    // === STRUCT
    // ! Move to Bullet class
    public struct Bullet
    {
        public int posX;
        public int posY;
        public bool isPlayer;
        // TODO Add Destroy() method | set istance = null;  *After remove it from the parent list
    }

    // ? Move to Explosion class | Maybe later
    public struct Explosion
    {
        public int posX;
        public int posY;
        public bool isExploded;
        public int explosionTimer;
    }

    // Shoots List
    public static List<Bullet> bullets = new List<Bullet>();
    // Explosion List
    public static List<Explosion> explosions = new List<Explosion>();

    // === TASKS & LOOPS
    public static void BulletHandler()  // ! Move to Gun class due to custom shootSpeed needs
    {
        Task.Run(async () =>
               {
                   while (Game.isRunning)
                   {
                       if (shootTimer < shootDelay) shootTimer++;
                       if (shootTimer >= shootDelay) canShoot = true;

                       if (bullets.Count > 0)
                       {
                           for (int i = 0; i < bullets.Count; i++)
                           {
                               // Collision Detection
                               if (bullets[i].posY > Game.mapStartY - 1 && bullets[i].posY < Game.mapLenghtY + Game.mapStartY) //! need -1?
                               {
                                   int newPos;
                                   if (!bullets[i].isPlayer) { newPos = bullets[i].posY - 1; }
                                   else newPos = bullets[i].posY + 1;
                                   Bullet updatedBullet = new Bullet { posX = bullets[i].posX, posY = newPos };
                                   bullets[i] = updatedBullet;
                               }
                               else
                               {
                                   NewExplosion(bullets[i].posX, bullets[i].posY);
                                   bullets.RemoveAt(i);
                               }
                           }
                       }
                       await Task.Delay(bulletSpeed);
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

    public static void NewBullet(int spawnX, int spawnY, bool isPlayer)
    {
        if (canShoot)
        {
            Bullet newBullet = new Bullet();
            newBullet.posX = spawnX;
            newBullet.posY = spawnY;
            newBullet.isPlayer = isPlayer;
            bullets.Add(newBullet);
            // Reset shooting capability 
            canShoot = false;
            shootTimer = 0;
            // Debug Log
            SetCursorPosition(0, 2);
            Write("Spawning New Shoot");
        }
    }

}