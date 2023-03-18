using static System.Console;
using static Game;


public class Tile
{
    int hp;
    public int posX;
    public int posY;
    public char gfx = 'O';
    public Entity? collision;
    public Entity parent; 
    public Entity.EntityType tileType;         

    public void Move(Direction newDirection)
    {
        if (posX + newDirection.x >= mapStartX + availableLenghtX) posX = mapStartX;
        else if (posX + newDirection.x < mapStartX) posX = mapStartX + availableLenghtX - 1;  // -1 is used to avoid scroll bar issues
        if (posY + newDirection.y >= mapStartY + availableLenghtY) posY = mapStartY;
        else if (posY + newDirection.y < mapStartY) posY = mapStartY + availableLenghtY - 1;  // -1 is used to avoid scroll bar issues
        else
        {
            posX += newDirection.x;
            posY += newDirection.y;
        }
    }

    void DealDamage(int damage) // ! damage value from gun
    {
        // If hp <= 0 | -> Explode()
    }

    void Explode()
    {
        // Start ExplosionVfx
        // Destroy  | Remove tile from parent entity array?

        // If tiles.count entity.Destroy()
    }
}