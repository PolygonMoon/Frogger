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
        // Save old tile position
        Direction oldPosition = new Direction { x = posX, y = posY };

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
        Direction newPosition = new Direction { x = posX, y = posY };
    }

    public bool CheckTileCollision(Direction newDirection)
    {
        //return true;    // ! WORK UNTIL HERE

        // * Avoid collision check if tiles is on map limits
        if (posX + newDirection.x < mapLenghtX + mapStartX - 1
                        && posY + newDirection.y < mapLenghtY + mapStartY - 1
                        && posX + newDirection.x > mapStartX
                        && posY + newDirection.y > mapStartY)
        {
            //return true;
            // Debug Next Tile direction in MapTile
            //return true;

            Tile nextTile = Map.tiles[posX + newDirection.x, posY + newDirection.y];
            //return true;

            if (Map.tiles[posX + newDirection.x, posY + newDirection.y] == null
            || Map.tiles[posX + newDirection.x, posY + newDirection.y].tileType == parent.entityType)
            {
                //ForegroundColor = ConsoleColor.Red;
                //SetCursorPosition(posX + newDirection.x, posY + newDirection.y);
                //Write("+");
                //ForegroundColor = ConsoleColor.Gray;

                return true;
            }
            else
            {
                // TODO Manage collision with different entity type
                return false;
            }
        }
        else return true;   // * Skipping collisio due to map limits



        //if (nextTile != null && nextTile.tileType != entityType) return false;

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