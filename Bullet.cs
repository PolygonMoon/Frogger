


public class Bullet
{
    // Bullet Status
    public int posX;
    public int posY;
    public bool isPlayer;
    public Entity? shooter;
    public bool isExploded = false;
    // TODO Add Direction | pick from parent entity

    // Bullet Setup
    public int damage;
    public string gfx = "|";



     // TODO Add Destroy() method | set istance = null;  *After remove it from the parent list

}