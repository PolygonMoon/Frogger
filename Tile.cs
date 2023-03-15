


public class Tile
{
    int hp;
    public int posX;
    public int posY;
    public string gfx = "O";
    Entity collision;


    public void Move(Direction newDirection)
    {
        posX += newDirection.x;
        posY += newDirection.y;
    }

    void DealDamage(int damage) // ! damage value from gun
    {
        // If hp <= 0 | -> Explode()
    }

    void Explode()
    {
        // Start ExplosionVfx
        // Destroy  | Remove tile from entity array?

        // If tiles.count entity.Destroy()
    }
}