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
        return true;    // ! WORK UNTIL HERE
        // Debug Actual Tile position in MapTile
        Tile actualTile = Map.tiles[posX, posY];
        // if (actualTile != null)
        // {
        //     ForegroundColor = ConsoleColor.Red;
        //     SetCursorPosition(actualTile.posX + newDirection.x, actualTile.posY + newDirection.y);
        //     Write("T");
        // }


        if (actualTile.posX + newDirection.x < mapLenghtX + mapStartX - 1
                        && actualTile.posY + newDirection.y < mapLenghtY + mapStartY - 1
                        && actualTile.posX + newDirection.x > mapStartX
                        && actualTile.posY + newDirection.y > mapStartY)
        {
            // Debug Next Tile direction in MapTile
            Tile nextTile = Map.tiles[posX + newDirection.x, posY + newDirection.y];
            if (nextTile != null && nextTile.tileType != tileType)
            {
                ForegroundColor = ConsoleColor.Green;
                SetCursorPosition(nextTile.posX, nextTile.posY);
                Write("+");
                return true;
            }

            if (nextTile != null && nextTile.tileType != tileType)
            {
                SetCursorPosition(40, mapLenghtY + mapStartY - 1);
                Write("NO COLLISION DETECTED");
                return true;
            }
            else
            {
                SetCursorPosition(40, mapLenghtY + mapStartY - 1);
                Write("SKIT MAP LIMIT");
                // Collision Handler by Switch EntityType type
                return true;
            }
            return true;
        }
        else return true;



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