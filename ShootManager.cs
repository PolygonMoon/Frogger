using static System.Console;
using System.Threading.Tasks;

public struct Shoot
{
    public int posX;
    public int posY;
}

public static class ShootManager
{
    public static string shootGfx = "|";
    public const int maxShoots = 5;        // Max sontemporary shoots count
    public static int shootSpeed = 16;     // Milliseconds between shoot movement

    public static bool canShoot = true;
    public static int shootTimer = 0;
    public const int shootDelay = 5;

    // Shoots List
    public static List<Shoot> shoots = new List<Shoot>();

    public static void Spawn(int spawnX, int spawnY)
    {
        if (canShoot)
        {
            Shoot newshoot = new Shoot();
            newshoot.posX = spawnX;
            newshoot.posY = spawnY;
            shoots.Add(newshoot);
            // Reset shooting capability 
            canShoot = false;
            shootTimer = 0;
        }
    }

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
                           //    foreach (var shoot in shoots)
                           //    {
                           //        int newPos = shoot.posY + 1;
                           //        Shoot updatedShoot = new Shoot{posY = newPos};
                           //        shoot = updatedShoot;
                           //    }
                           for (int i = 0; i < shoots.Count; i++)
                           {
                               if (shoots[i].posY > Game.mapStartY - 1)
                               {
                                   int newPos = shoots[i].posY - 1;
                                   Shoot updatedShoot = new Shoot { posX = shoots[i].posX, posY = newPos };
                                   shoots[i] = updatedShoot;
                               }
                               else shoots.RemoveAt(i);
                           }
                       }
                       await Task.Delay(shootSpeed);
                   }
               });



    }
}