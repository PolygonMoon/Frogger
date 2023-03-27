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
    public char bulletGfx = '|';

    // Local Bullets List
    public List<Tile> bullets = new List<Tile>();

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
                               // UnSubscribe the tile before the movement
                               Map.UnSubscribeMapTile(bullets[i]);
                               bool isMoveValid = false;
                               isMoveValid = bullets[i].CheckTileCollision(direction);

                               if (isMoveValid && bullets[i].posY > Game.mapStartY && bullets[i].posY < Game.mapLenghtY + Game.mapStartY) //! need -1?
                               {
                                   bullets[i].Move(direction);
                                   // Subscribe the tile after the movement
                                   Map.SubscribeMapTile(bullets[i]);
                               }
                               else
                               {
                                   NewExplosion(bullets[i].posX, bullets[i].posY);
                                   bullets[i].isAlive = false;
                                   // UnSubscribe the tile after the collision
                                   Map.UnSubscribeMapTile(bullets[i]);
                                   bullets.RemoveAt(i);
                               }
                           }
                       }
                       await Task.Delay(bulletSpeed);
                   }
               });
    }

    public void Shoot(int spawnX, int spawnY, Entity shooter)
    {
        {
            Tile newBullet = new Tile();
            newBullet.posX = spawnX;
            newBullet.posY = spawnY;
            newBullet.gfx = bulletGfx;
            newBullet.tileType = Entity.EntityType.Bullet;
            newBullet.parent = shooter;
            bullets.Add(newBullet);
            EntityManager.bullets.Add(newBullet);
            // Reset shooting capability 
            canShoot = false;
            shootTimer = 0;
            // Debug Log
            SetCursorPosition(0, 2);
            Write("Spawning New Shoot");
        }
    }

}