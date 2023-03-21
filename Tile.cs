using static System.Console;
using static Game;


public class Tile
{
    int hp;
    public int posX;
    public int posY;
    public char gfx = 'O';
    public Entity? collision;
    public Entity? parent;
    public Entity.EntityType? tileType;

    public void Move(Direction newDirection)
    {
        // UnSubscribe the tile before the movement | // ! TESTING
        Map.UnSubscribeMapTile(this);

        // Check X Map Limits
        if (posX + newDirection.x >= mapStartX + mapLenghtX) posX = mapStartX;
        else if (posX + newDirection.x < mapStartX) posX = mapStartX + mapLenghtX - 1;  // -1 is used to avoid scroll bar issues
        // Check Y Map Limits
        if (posY + newDirection.y >= mapStartY + mapLenghtY) posY = mapStartY;
        else if (posY + newDirection.y < mapStartY) posY = mapStartY + mapLenghtY - 1;  // -1 is used to avoid scroll bar issues
        // Move
        else
        {
            posX += newDirection.x;
            posY += newDirection.y;
        }
        // UnSubscribe the tile after the movement | // ! TESTING | precision issue here
        //Map.UnSubscribeMapTile(this);
        // Subscribe the tile after the movement
        Map.SubscribeMapTile(this);
    }

    public bool CheckTileCollision(Direction newDirection)
    {
        // * Excecure Collision Check only if entity tile is within map limits
        if (posX + newDirection.x < mapLenghtX + mapStartX - 1
                        && posY + newDirection.y < mapLenghtY + mapStartY - 1
                        && posX + newDirection.x > mapStartX
                        && posY + newDirection.y > mapStartY)
        {
            // Save nextTile MapTiles Tile
            Tile? nextTile = Map.tiles[posX + newDirection.x, posY + newDirection.y];
            // Check nextTile content
            if (nextTile == null || nextTile.tileType == parent.entityType)
            {
                return true;
            }
            else
            {
                // TODO Manage collision with different entity type
                return false;
            }
        }
        else return true;   // * Skipping collision check due to map limits
    }

    void CollisionHandler(Tile collidingTile, Tile collisionTile)
    {
        // Damage
        // Kill
        // ForceMove | return true ?
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