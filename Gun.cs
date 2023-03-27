using static System.Console;
using static ShootManager;

public class Gun
{
    // Gun Status
    public bool canShoot = true;
    public int shootTimer = 0;
    public int bulletAmount = 20;
    public int damage = 1;
    public Entity? owner;           // Gun owner
    public Direction direction;     // TODO Define direction by using DefineDirection(owner.moveType)

    // Gun Setup
    public int maxBullets = 5;        // Max contemporary bullets count
    public int bulletSpeed = 30;      // 16 | Milliseconds between bullets movement
    public int shootDelay = 5;        // Milliseconds between bullets shoot
    public string bulletGfx = "|";

    // Local Bullets List
    public List<Bullet> bullets = new List<Bullet>();

    public void BulletHandler()
    {
        Task.Run(async () =>
               {
                   while (Game.isRunning)
                   {
                       if (shootTimer < shootDelay) shootTimer++;
                       if (shootTimer >= shootDelay) canShoot = true;

                       // ! TODO Move this on a dedicated method and excecute by Entity Manager loop?
                       if (bullets.Count > 0)
                       {
                           for (int i = 0; i < bullets.Count; i++)
                           {
                               // Collision Detection
                               if (bullets[i].posY > Game.mapStartY && bullets[i].posY < Game.mapLenghtY + Game.mapStartY) //! need -1?
                               {
                                   // ! TODO Read Direction from bullet shooter?
                                   if (bullets[i].isPlayer) { bullets[i].posY -= 1; }
                                   else bullets[i].posY += 1;
                               }
                               else
                               {
                                   NewExplosion(bullets[i].posX, bullets[i].posY);
                                   bullets[i].isExploded = true;
                                   bullets.RemoveAt(i);
                               }
                           }
                       }
                       await Task.Delay(bulletSpeed);
                   }
               });
    }

    // ! CONVERT BULLET TO ENTITY OR TILE?
    public void Shoot(int spawnX, int spawnY, Entity shooter, bool isPlayer)
    {
        {
            Bullet newBullet = new Bullet();
            newBullet.posX = spawnX;
            newBullet.posY = spawnY;
            newBullet.isPlayer = isPlayer;
            newBullet.shooter = shooter;
            bullets.Add(newBullet);
            ShootManager.bullets.Add(newBullet);
            // Reset shooting capability 
            canShoot = false;
            shootTimer = 0;
            // Debug Log
            SetCursorPosition(0, 2);
            Write("Spawning New Shoot");
        }
    }

}