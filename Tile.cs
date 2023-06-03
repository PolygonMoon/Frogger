using static System.Console;
using static Game;


public class Tile
{
    // Tile Status
    int hp;
    public int posX;
    public int posY;
    public bool isAlive = true;
    // ! ADD x,y index ??? | Use to recover the tile position within entity tiles array
    public int indexX;
    public int indexY;
    public char gfx = 'O';
    public Entity? collision;
    public Entity? parent;      // Used to save parent Entity or Shooter
    public Entity.EntityType? tileType;

    // === Tile Methods
    public void Move(Direction newDirection)
    {
        // Map Limits Check | Teleport Entities if out of the map
        if (posX + newDirection.x >= mapStartX + mapLenghtX) posX = mapStartX;
        else if (posX + newDirection.x < mapStartX) posX = mapStartX + mapLenghtX - 1;  // -1 is used to avoid scroll bar issues
        if (posY + newDirection.y >= mapStartY + mapLenghtY) posY = mapStartY;
        else if (posY + newDirection.y < mapStartY) posY = mapStartY + mapLenghtY - 1;  // -1 is used to avoid scroll bar issues
                                                                                        // Move
        else
        {
            posX += newDirection.x;
            posY += newDirection.y;
        }
    }

    public bool CheckTileCollision(Direction newDirection)
    {
        // * Excecute Collision Check only if entity tile is within map limits
        if (posX + newDirection.x < mapLenghtX + mapStartX
            && posY + newDirection.y < mapLenghtY + mapStartY
            && posX + newDirection.x > mapStartX
            && posY + newDirection.y > mapStartY)
        {
            // Save nextTile MapTiles Tile
            Tile? nextTile = Map.tiles[posX + newDirection.x, posY + newDirection.y];
            // Check nextTile content
            if (nextTile == null || nextTile.parent == parent)  // Avoid internal collision within same entitie tiles
            {
                return true;
            }
            // Skip collision between Bullet => Water
            else if (tileType == Entity.EntityType.Bullet && nextTile.tileType == Entity.EntityType.Water) return true;
            // TODO Skip collision between Trunk => Water
            else
            {
                // TODO Manage collision with different entity type
                CollisionHandler(this, nextTile);
                return false;
            }
        }
        else return true;   // * Skipping collision check due to map limits
    }

    public void CollisionHandler(Tile collidingTile, Tile collisionTile)
    {
        // TODO Manage different behaviour
        if (collidingTile.parent != null && collisionTile.parent != null)
        {
            // Enemy VS Wall
            if (collidingTile.tileType == Entity.EntityType.Enemy && collisionTile.tileType == Entity.EntityType.Wall)
            {
                // Kill colliding entity | TEST PURPOSE
                collidingTile.parent.Die();
            }
            // Enemy VS Player
            if (collidingTile.tileType == Entity.EntityType.Enemy && collisionTile.tileType == Entity.EntityType.Player)
            {
                // Kill collision entity | TEST PURPOSE
                collisionTile.parent.Die();
            }
            // Player VS Enemy
            if (collidingTile.tileType == Entity.EntityType.Player && collisionTile.tileType == Entity.EntityType.Enemy)
            {
                // Kill colliding entity | TEST PURPOSE
                collidingTile.parent.Die();
            }
            // Enemy VS Bullet
            if (collidingTile.tileType == Entity.EntityType.Enemy && collisionTile.tileType == Entity.EntityType.Bullet)
            {
                // Kill colliding entity | TEST PURPOSE
                collidingTile.parent.Die();
            }
            // Player VS Bullet
            if (collidingTile.tileType == Entity.EntityType.Player && collisionTile.tileType == Entity.EntityType.Bullet)
            {
                // Kill colliding entity | TEST PURPOSE
                collidingTile.parent.Die();
            }
            // Player VS Water
            if (collidingTile.tileType == Entity.EntityType.Player && collisionTile.tileType == Entity.EntityType.Water)
            {
                // Kill colliding entity | TEST PURPOSE
                collidingTile.parent.Die();
            }
            // Bullet VS Enemy
            if (collidingTile.tileType == Entity.EntityType.Bullet && collisionTile.tileType == Entity.EntityType.Enemy)
            {
                // Kill collision entity | TEST PURPOSE
                collisionTile.parent.Die();
            }
            // Bullet VS Player
            if (collidingTile.tileType == Entity.EntityType.Bullet && collisionTile.tileType == Entity.EntityType.Player)
            {
                // Kill collision entity | TEST PURPOSE
                collisionTile.parent.Die();
            }
        }
    }

    void Damage(int damage) // ! damage value from gun
    {
        if (hp > 0) hp--;
        // if (hp == 2) Set color to orange
        // if (hp == 1) Set color to red
        else Explode(this);
    }

    public void Explode(Tile tile)
    {
        // Start Explosion Vfx
        ShootManager.NewExplosion(tile.posX, tile.posY);
        // UnSubscribe the tile from the MapTile
        Map.UnSubscribeMapTile(tile);
    }
}